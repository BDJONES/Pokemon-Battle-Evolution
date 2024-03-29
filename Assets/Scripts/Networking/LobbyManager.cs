using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

#if UNITY_EDITOR
using ParrelSync;
#endif

public class LobbyManager : Singleton<LobbyManager>
{
    //private UIController uIController;
    private UnityTransport unityTransport;
    private Lobby connectedLobby;
    private const string joinCodeKey = "PokemonLobbyManager";
    private string playerId;
    private NetworkVariable<int> playerNumber = new NetworkVariable<int>();
    // Start is called before the first frame update
    private void OnEnable()
    {
        unityTransport = GameObject.Find("NetworkManager").GetComponent<UnityTransport>();
        //uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
    }

    public async void CreateOrJoinLobby()
    {
        //Debug.Log("Hello There");
        await Authenicate();
        connectedLobby = await QuickJoinLobby();
        if (connectedLobby == null)
        {
            connectedLobby = await CreateLobby();
            if (connectedLobby != null)
            {
                Debug.Log("Created a Lobby");
            }
        }
        else
        {
            Debug.Log("Quickjoined a match");
        }
        //Debug.Log($"{NetworkManager.Singleton.ConnectedClients.Count} players have connected to the game");
        if (connectedLobby != null)
        {
            Debug.Log("Successfully joined a game");
            //uIController.UpdateMenu(Menus.LoadingScreen);
        }
        else
        {
            Debug.Log("Failed to join a game");
        }
    }

    private async UniTask Authenicate()
    {

        InitializationOptions options = new InitializationOptions();
        #if UNITY_EDITOR
                // Remove this if you don't have ParrelSync installed. 
                // It's used to differentiate the clients, otherwise lobby will count them as the same
                options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
        #endif
        await UnityServices.InitializeAsync(options); 
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerId = AuthenticationService.Instance.PlayerId;
        Debug.Log("Finished Authenticating");
    }    

    private async UniTask<Lobby> QuickJoinLobby()
    {
        try
        {
            // Attempt to join a lobby in progress
            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            // If we found one, grab the relay allocation details
            var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[joinCodeKey].Value);

            // Set the details to the transform
            SetTransformAsClient(a);

            // Join the game room as a client
            NetworkManager.Singleton.StartClient();
            return lobby;
        }
        catch
        {
            Debug.Log($"No lobbies available via quick join");
            return null;
        }
    }

    private async UniTask<Lobby> CreateLobby()
    {
        try
        {
            const int maxPlayers = 2;

            // Create a relay allocation and generate a join code to share with the lobby
            var a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            // Create a lobby, adding the relay join code to the lobby data
            var options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> { { joinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, joinCode) } }
            };
            var lobby = await Lobbies.Instance.CreateLobbyAsync("Useless Lobby Name", maxPlayers, options);

            // Send a heartbeat every 15 seconds to keep the room alive
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            // Set the game room to use the relay allocation
            unityTransport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            // Start the room. I'm doing this immediately, but maybe you want to wait for the lobby to fill up
            NetworkManager.Singleton.StartHost();
            return lobby;
        }
        catch
        {
            Debug.LogFormat("Failed creating a lobby");
            return null;
        }
    }

    private void SetTransformAsClient(JoinAllocation a)
    {
        unityTransport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
    }

    private static IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            // todo: Add a check to see if you're host
            if (connectedLobby != null)
            {
                if (connectedLobby.HostId == playerId) Lobbies.Instance.DeleteLobbyAsync(connectedLobby.Id);
                else Lobbies.Instance.RemovePlayerAsync(connectedLobby.Id, playerId);
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error shutting down lobby: {e}");
        }
    }
}
