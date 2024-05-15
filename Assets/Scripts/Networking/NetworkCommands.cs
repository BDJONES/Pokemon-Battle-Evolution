using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class NetworkCommands : NetworkBehaviour
{
    private NetworkVariable<int> playerCount = new NetworkVariable<int>();
    private bool twoPlayersConnected = false;
    public static event Action TwoPlayersConnected;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject uIController;
    [SerializeField] private GameObject eventsToTriggerManager;
    [SerializeField] private GameObject effectQueueController;
    public static event Action<GameObject> HostConnected;
    public static event Action UIControllerCreated;
    private bool gameManagerSpawned = false;
    private bool uIControllerSpawned = false;
    public override void OnNetworkSpawn()
    {
        //var titleScreenUI = GameObject.Find("TitleScreenUI");
        //titleScreenUI.SetActive(false);

        //Destroy(titleScreenUI);
        if (!IsHost)
        {
            return;
        }
     
        if (!gameManagerSpawned)
        {
            var spawnedUIController = Instantiate(uIController);
            spawnedUIController.name = "UI Controller";
            spawnedUIController.GetComponent<NetworkObject>().Spawn(true);            
            var generatedGameManager = Instantiate(gameManager);
            generatedGameManager.name = "Game Manager";
            generatedGameManager.GetComponent<NetworkObject>().Spawn(true);
            var spawnedEventToTriggerManager = Instantiate(eventsToTriggerManager);
            spawnedEventToTriggerManager.name = "EventsToTriggerManager";
            spawnedEventToTriggerManager.GetComponent<NetworkObject>().Spawn(true);
            var spawnedEffectQueueController = Instantiate(effectQueueController);
            spawnedEffectQueueController.name = "EffectQueueController";
            spawnedEffectQueueController.GetComponent<NetworkObject>().Spawn(true);
            //Debug.Log("Created Game Manager");
            
            gameManagerSpawned = true;
            uIControllerSpawned = true;
            UIControllerCreated?.Invoke();
        }

        //PopulateGameManager(player);
        //if (!IsServer) return;
        //GameObject player = NetworkManager.SpawnManager.GetLocalPlayerObject().gameObject;
        //if (player == null)
        //{
        //    Debug.Log("The player is null");
        //}
        //if (HostConnected != null)
        //{
        //    HostConnected.Invoke(player);
        //}
        //else
        //{
        //    Debug.Log("There is an issue with the event");
        //}
        //PopulateUIValues();
        return;
    }

    private void PopulateUIValues()
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (IsHost)
        {
            //if (IsServer && NetworkManager.Singleton.ConnectedClients.Count > playerCount.Value)
            //{
            //    Debug.Log(NetworkManager.Singleton.ConnectedClients.Values.);
            //}
            playerCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
            //Debug.Log("I'm the server");
            //SendGameMangerToClientRpc(gM);
            if (gameManager != null)
            {
                //Debug.Log($"The Game Manager is not null, playerCount = {playerCount.Value}");
                if (playerCount.Value == 2 && GameObject.Find("Game Manager").GetComponent<GameManager>().GetTrainer2Controller() != null && !twoPlayersConnected)
                {
                    //SendGameMangerToClientRpc(gM);
                    //Debug.Log("Invoking TwoPlayersConnected");
                    twoPlayersConnected = true;
                    TwoPlayersConnected?.Invoke();
                }
            }
        }
        var uIControllerScript = FindAnyObjectByType<UIController>();
        GameObject spawnedUIControllerObject = null;
        if (uIControllerScript != null)
        {
            spawnedUIControllerObject = uIControllerScript.gameObject;
        }
        if (uIControllerSpawned == false && spawnedUIControllerObject != null)
        {
            //Debug.Log($"Found the UI Controller: Name = {spawnedUIControllerObject.name}");
            spawnedUIControllerObject.name = "UI Controller";
            UIControllerCreated?.Invoke();
            uIControllerSpawned = true;
        }
    }

    //[Rpc(SendTo.Server)]
    //private IPlayerAction playerMoveRequestRpc()
    //{
    //    return new Flamethrower();
    //}
}
