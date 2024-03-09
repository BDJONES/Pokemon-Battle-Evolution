using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCommands : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        //NetworkObjectId
        GameObject player = NetworkManager.SpawnManager.GetLocalPlayerObject().gameObject;
        PopulateGameManager(player);
        //PopulateUIValues();
        return;
    }

    private void PopulateUIValues()
    {
        throw new NotImplementedException();
    }

    private void PopulateGameManager(GameObject player)
    {
        if (GameManager.Instance.GetTrainer1Controller() == null)
        {
            GameManager.Instance.PopulateGameManager(player, 1);
        }
        else
        {
            GameManager.Instance.PopulateGameManager(player, 2);
        }
    }
}
