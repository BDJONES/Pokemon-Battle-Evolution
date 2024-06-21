using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDisconnectController : NetworkBehaviour
{
    [SerializeField] private string playerId;
    private event Action OnDisconnected;
    [SerializeField] private GameObject titleScreenUI;

    private void OnEnable()
    {
        //titleScreenUI = GameObject.Find("TitleScreenUI");
        if (GameObject.Find(titleScreenUI.name) != null)
        {
            Destroy(GameObject.Find(titleScreenUI.name));
        }
        
    }
    public void SetPlayerID(string id)
    {
        playerId = id;
        if (!IsHost)
        {
            UpdateClientPlayerIDRpc(playerId);
        }
    }

    public string GetPlayerID()
    {
        return playerId;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void ResetToStartRpc(string id)
    {
        if (IsHost)
        {
            Debug.Log($"Host ID = {playerId}, id passed in is {id}");
            
        }
        if (!IsHost)
        {
            Debug.Log($"Client ID = {playerId}, id passed in is {id}");
        }
        if (id == playerId) 
        {
            Debug.Log("ResetToStartRpc was called");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void UpdateClientPlayerIDRpc(string id)
    {
        if (IsHost)
        {
            return;
        }
        Debug.Log("Setting the PlayerId via RPC");
        playerId = id;
        Debug.Log($"playerId = {playerId}");
    }
}