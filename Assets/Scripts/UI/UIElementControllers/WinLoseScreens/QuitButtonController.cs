using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class QuitButtonController : NetworkBehaviour
{
    private GameObject titleScreenUI;
    private UIController uIController;
    private WinLoseUIElements winLoseUIElements;
    private LobbyManager lobbyManager;
    RPCManager rpcManager;
    private List<string> playerIDs;
    private PlayerDisconnectController playerDisconnectController;
    private void OnEnable()
    {
        //if (IsOwner)
        //{
            NetworkCommands.UIControllerCreated += () =>
            {
                lobbyManager = GameObject.Find("Lobby Manager").GetComponent<LobbyManager>();
                uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
                winLoseUIElements = uIController.GetComponent<WinLoseUIElements>();
                playerIDs = new List<string>();
                rpcManager = new RPCManager();
                uIController.OnHostMenuChange += HandleMenuChange;
                uIController.OnClientMenuChange += HandleMenuChange;
                playerDisconnectController = transform.parent.parent.gameObject.GetComponent<PlayerDisconnectController>();
                playerDisconnectController.SetPlayerID(lobbyManager.GetPlayerID());
                if (IsClient)
                {
                    playerDisconnectController.UpdateClientPlayerIDRpc(lobbyManager.GetPlayerID());
                }
            };
        //}
    }

    private void OnDisable()
    {
        if (uIController != null)
        {
            uIController.OnClientMenuChange -= HandleMenuChange;
            uIController.OnHostMenuChange -= HandleMenuChange;
        }
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.LoseScreen || menu == Menus.WinScreen)
        {
            //var player = transform.parent.parent.gameObject;
            if (IsHost)
            {
                Debug.Log("Player is the Host");
                UIEventSubscriptionManager.Subscribe(winLoseUIElements.QuitButton, HandleQuitButtonClick, 1);
            }
            else
            {
                Debug.Log("Player is the Client");
                UIEventSubscriptionManager.Subscribe(winLoseUIElements.QuitButton, HandleQuitButtonClick, 2);
            }
        }
    }

    private async void HandleQuitButtonClick()
    {
        //    if (IsHost)
        //    {
        //await RemoveAllPlayers();
        //}
        //else
        //{
        string lobbyID = lobbyManager.GetLobby().Id;
        string playerID = lobbyManager.GetPlayerID();
        if (IsHost)
        {
            
            await Lobbies.Instance.DeleteLobbyAsync(lobbyID);
        }
        else
        {
            await Lobbies.Instance.RemovePlayerAsync(lobbyID, playerID);
        }
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
        //}
    }

    private async UniTask RemoveAllPlayers()
    {
        //NetworkManager.Singleton.ConnectedClients[0].PlayerObject.IsLocalPlayer;
        string lobbyID = lobbyManager.GetLobby().Id;
        Lobby lobby = await LobbyService.Instance.GetLobbyAsync(lobbyID);
        Debug.Log($"There are {lobby.Players.Count} currently connected");
        string hostId = "";
        foreach (var player in lobby.Players)
        {
            Debug.Log($"PlayerID = {player.Id}");
            //playerDisconnectController.ResetToStartRpc(player.Id);
            if (lobby.HostId == player.Id)
            {
                Debug.Log("Found the Host");
                hostId = player.Id;
            }   
            else
            {
                playerDisconnectController.ResetToStartRpc(player.Id);
                await Lobbies.Instance.RemovePlayerAsync(lobby.Id, player.Id);
                float time = 0f;
                while (time < 10f)
                {
                    time += Time.deltaTime;
                    await UniTask.Yield();
                }
                //Debug.Log("Removed the player from the lobby");
            }
        }
        await Lobbies.Instance.DeleteLobbyAsync(lobby.Id);
        playerDisconnectController.ResetToStartRpc(hostId);
    }
}