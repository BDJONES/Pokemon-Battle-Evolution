using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UIElements;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.Collections;
using UnityEngine.Rendering;
using UnityEditor.Experimental.GraphView;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private TrainerController trainer1Controller;
    [SerializeField] private TrainerController trainer2Controller;
    private static Trainer trainer1;
    private static Trainer trainer2;
    private static UIController uIController;
    [SerializeField] static private GameState gameState;
    public static event Action<GameState> OnStateChange;
    public static event Action GameManagerSpawned;
    //[SerializeField] private GameObject uiController;
    private static PokemonInfoController trainer1PokemonInfoController;
    private static OpposingPokemonInfoController trainer1OpposingPokemonInfoBarController;
    private static PokemonInfoController trainer2PokemonInfoController;
    private static OpposingPokemonInfoController trainer2OpposingPokemonInfoBarController;
    private static DialogueBoxController dialogueBoxController;
    //private static DialogueBoxController trainer2DialogueBoxController;
    private LobbyManager lobbyManager;
    private bool player1Inactive = false;
    private bool player2Inactive = false;
    public RPCManager RPCManager;
    IPlayerAction trainer1Selection;
    IPlayerAction trainer2Selection;
    //[SerializeField] private NetworkCommands networkCommands;

    //private void OnEnable()
    //{
        
    //}
    
    public TrainerController GetTrainer1Controller()
    {
        return trainer1Controller;
    }

    public TrainerController GetTrainer2Controller() { 
        return trainer2Controller;
    }

    private void SetTrainer1Controller(TrainerController trainerController)
    {
        if (IsOwner)
        {
            trainer1Controller = trainerController;
            trainer1 = trainer1Controller.GetPlayer();
            //trainer1Controller.SetCameraActive();
            //trainer1Controller.SetUIActive();
            trainer1PokemonInfoController = trainerController.gameObject.GetComponentInChildren<PokemonInfoController>();
            trainer1OpposingPokemonInfoBarController = trainerController.gameObject.GetComponentInChildren<OpposingPokemonInfoController>();
        }
    }

    private void SetTrainer2Controller(TrainerController trainerController)
    {
        //var id = trainer2Controller.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
        if (IsOwner)
        {
            trainer2Controller = trainerController;
            trainer2 = trainer2Controller.GetPlayer();
            //trainer2Controller.SetCameraActive();
            //trainer2Controller.SetUIActive();
            trainer2PokemonInfoController = trainerController.gameObject.GetComponentInChildren<PokemonInfoController>();
            trainer2OpposingPokemonInfoBarController = trainerController.gameObject.GetComponentInChildren<OpposingPokemonInfoController>();
        }

    }

    private void SetPokemonInfoController(GameObject player)
    {

        //Transform sharedControllers = player..Find("SharedControllers");
    }

    public void PopulateGameManager(GameObject gameObject, int type)
    {
        TrainerController trainerController = gameObject.GetComponent<TrainerController>();
        if (type == 1)
        {
            //Debug.Log("Setting Player 1 Data");
            SetTrainer1Controller(trainerController);
        }
        else
        {
            //Debug.Log("Setting Up Player 2 Data");
            SetTrainer2Controller(trainerController);
        }
        //Set Location of the gameobjects
        //SetPokemonInfoController(gameObject);
    }

    private void OnEnable()
    {
        //NetworkCommands.HostConnected += HandleHostConnection;
        NetworkCommands.TwoPlayersConnected += HandlePlayerConnection;
        NetworkManager.OnClientConnectedCallback += HandleNewPlayerJoin;
        NetworkCommands.UIControllerCreated += SetUIController;
        RPCManager = new RPCManager();
        Debug.Log("Enabled the Game Manager");//;
        if (!IsHost)
        {
            gameObject.name = "Game Manager";
            GameManagerSpawned?.Invoke();
        }
        //Debug.Log(typeof(Flamethrower).AssemblyQualifiedName);
    }

    private void SetUIController()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        dialogueBoxController = GameObject.Find("Me").GetComponentInChildren<DialogueBoxController>();
    }

    private void HandleNewPlayerJoin(ulong obj)
    {
        if (!IsServer) return;
        var playerObject = NetworkManager.ConnectedClients[obj].PlayerObject.gameObject;

        //Debug.Log("Detected that a new player joined");
        if (trainer1Controller == null)
        {
            //playerObject.name = "Trainer 1";
            Debug.Log("Populating the GameManager with Trainer 1 Data");
            PopulateGameManager(playerObject, 1);
        }
        else
        {
            //playerObject.name = "Trainer 2";
            Debug.Log("Populating the GameManager with Trainer 2 Data");
            PopulateGameManager(playerObject, 2);
        }
    }

    private void Start()
    {
        //trainer1 = trainer1Controller.GetPlayer();
        //trainer2 = trainer2Controller.GetPlayer();
        //trainer1UIController = trainer1Controller.gameObject.GetComponentInChildren<UIController>();

        //Debug.Log("Subscribed to Both Events");
    }

    private void HandleHostConnection(GameObject playerObject)
    {
        Debug.Log("Detected that a new player joined");
        if (trainer1Controller == null)
        {
            Debug.Log("Populating the GameManager with Trainer 1 Data");
            PopulateGameManager(playerObject, 1);
        }
        else
        {
            Debug.Log("Populating the GameManager with Trainer 2 Data");
            PopulateGameManager(playerObject, 2);
        }
    }

    private void HandlePlayerConnection()
    {
        if (!IsServer)
        {
            //Debug.Log("I'm not the host");
            return;
        }
        //else
        //{
        //    Debug.Log("I'm the Host!!!");
        //}
        // if two players have connected then we would like to start the game
        Debug.Log("Two Players have Joined");
        //while (IsOwner && trainer2Controller == null) ;
        StartGame();
    }

    private async void StartGame()
    {
        //UIDocument uiDocument = uiController.GetComponent<UIDocument>();
        //uiDocument.rootVisualElement.style.display = DisplayStyle.None;

        Debug.Log("Starting the Game");
        float time = 0f;
        while (time < 3f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }        
        trainer1Controller.SetOpponent(trainer2);
        trainer2Controller.SetOpponent(trainer1);
        //UpdateGameState(GameState.LoadingPokemonInfo);
        UpdateGameStateRpc(GameState.LoadingPokemonInfo);
        time = 0f;
        while (time < 2f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        //UpdateGameState(GameState.BattleStart);
        UpdateGameStateRpc(GameState.BattleStart);
        //TimeManager.MatchTimerEnd += HandleMatchTimeout;
        //trainer1Controller.playerTooInactive += Trainer1Inactive;
        //trainer2Controller.playerTooInactive += Trainer2Inactive;        
        //uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        await TurnSystem();
    }

    private void Trainer1Inactive()
    {
        player1Inactive = true;
    }

    private void Trainer2Inactive()
    {
        player2Inactive = true;
    }

    private async UniTask MenuBuffer()
    {
        Debug.Log("Giving the screen a buffer");
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private async UniTask TurnSystem()
    {
        //dialogueBoxController.SendDialogueToHostRpc("Hello There Server");
        //await dialogueBoxController.MakeHostReadFirstDialogueRpc();
        while (gameState != GameState.BattleEnd)
        {
            UpdateGameStateRpc(GameState.TurnStart);
            UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
            if (trainer1Selection != null)
            {
                Debug.Log("trainer1Selection is not null");
            }
            else
            {
                Debug.Log("trainer1Selection is null");
            }
            if (trainer2Selection != null)
            {
                Debug.Log("trainer2Selection is not null");
            }
            else
            {
                Debug.Log("trainer2Selection is null");
            }
            RPCManager.BeginRPCBatch();
            RequestMoveSelectionRpc();
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
            Debug.Log("Both Players have made a Selection");
            //uIController.UpdateMenu(Menus.BlankScreen, 1);
            //uIController.UpdateMenu(Menus.BlankScreen, 2);
            //await MenuBuffer();
            // Need to add for a case where both players have gone inactive
            //if (player1Inactive)
            //{
            //    UpdateGameState(GameState.BattleEnd);
            //    break;
            //}
            //else if (player2Inactive)
            //{
            //    UpdateGameState(GameState.BattleEnd);
            //    break;
            //}
            UpdateGameStateRpc(GameState.ProcessingInput);
            await DecideWhoGoesFirst(trainer1Selection, trainer2Selection);
            trainer1Selection = null;
            trainer2Selection = null;
            UpdateGameStateRpc(GameState.TurnEnd);
        }
    }

    [Rpc(SendTo.Server)]
    public void AddRPCTaskRpc()
    {
        if (IsHost)
        {
            RPCManager.RPCStarted();
        }
    }

    [Rpc(SendTo.Server)]
    public void FinishRPCTaskRpc()
    {
        if (IsHost)
        {
            RPCManager.RPCFinished();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void RequestHostSwitchRpc()
    {
        if (!IsHost)
        {
            return;
        }
        Debug.Log("Requesting a move from the player");
        //Debug.Log("Adding RPC Task on Host");
        AddRPCTaskRpc();
        var playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        playerTrainerController.SendMoveSelect(1);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void RequestClientSwitchRpc()
    {
        if (IsHost)
        {
            return;
        }
        Debug.Log("Requesting a move from the player");
        //Debug.Log("Adding RPC Task on Client");
        AddRPCTaskRpc();
        var playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        playerTrainerController.SendMoveSelect(2);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void RequestMoveSelectionRpc()
    {
        Debug.Log("Requesting a move from the player");
        //Debug.Log("Adding RPC Task on Client and Host");
        AddRPCTaskRpc();
        var playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        if (IsHost)
        {
            playerTrainerController.SendMoveSelect(1);
        }
        else
        {
            Debug.Log("Asking Client to Select a move");
            playerTrainerController.SendMoveSelect(2);
        }
    }

    [Rpc(SendTo.Server)]
    public void RecieveAttackSelectionRpc(int type, AttackRPCTransfer attackRPCTrasfer)
    {
        
        var action = (IPlayerAction)Activator.CreateInstance(System.Type.GetType(attackRPCTrasfer.attackName.ToString().Replace(" ", "")));
        Attack convertedAction = (Attack)action;
        uIController.UpdateMenu(Menus.DialogueScreen, type);
        if (type == 1) // Host
        {
            Debug.Log($"Player 1 selected = {convertedAction.GetAttackName()}");
            SetTrainer1Move(action);
            //moveSelectionRPCManager.RPCFinished();
        }
        else if (type == 2) // Client
        {
            Debug.Log($"Player 2 selected = {convertedAction.GetAttackName()}");
            SetTrainer2Move(action);
            //moveSelectionRPCManager.RPCFinished();
            
        }
        UpdateMoveInfoRpc(type, attackRPCTrasfer.attackName);
        //if (IsHost)
        //{
        //    Debug.Log("Host called this function");
            
        //}
        FinishRPCTaskRpc();
        return;
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateMoveInfoRpc(int type, FixedString128Bytes attackName)
    {

        GameObject player = GameObject.Find("Me");
        string atkName = attackName.ToString();
        if (type == 1 && IsHost) // Host
        {
            foreach (var attack in player.GetComponent<Trainer>().GetActivePokemon().GetMoveset())
            {
                if (attack.GetAttackName().Equals(atkName))
                {
                    attack.SetCurrentPP(attack.GetCurrentPP() - 1);
                }
            }

        }
        else if (type == 2 && !IsHost) // Client
        {
            foreach (var attack in player.GetComponent<Trainer>().GetActivePokemon().GetMoveset())
            {
                if (attack.GetAttackName().Equals(atkName))
                {
                    attack.SetCurrentPP(attack.GetCurrentPP() - 1);
                }
            }
        }
        return;
    }

    [Rpc(SendTo.Server)]
    public void RecieveSwitchSelectionRpc(int type, SwitchRPCTransfer switchRPCTrasfer)
    {
        var client = NetworkManager.ConnectedClients[switchRPCTrasfer.trainerClientID];
        GameObject player = client.PlayerObject.gameObject;
        Pokemon pokemon = player.GetComponent<TrainerController>().GetPlayer().GetPokemonTeam()[switchRPCTrasfer.pokemonIndex];
        Trainer trainer = player.GetComponent<Trainer>();
        Switch playerSwitch = new Switch(trainer, pokemon);
        Debug.Log("Recieved a Switch Selection");
        Debug.Log(pokemon.GetSpeciesName());
        uIController.UpdateMenu(Menus.DialogueScreen, type);
        if (type == 1) // Host
        {            
            SetTrainer1Move(playerSwitch);
        }
        else if (type == 2) // Client
        {   
            SetTrainer2Move(playerSwitch);
            SetClientDialogue();
        }
        FinishRPCTaskRpc();
    }

    private async void SetClientDialogue()
    {
        SendDialogueToClientRpc("Communicating...");
        while (RPCManager.ActiveRPCs() > 1)
        {
            await UniTask.Yield();
        }
        RequestClientReadFirstDialogueRpc();
        while (RPCManager.ActiveRPCs() > 1)
        {
            await UniTask.Yield();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void RequestHostReadFirstDialogueRpc()
    {
        if (!IsHost) return;
        //Debug.Log("Adding RPC Task on Host");
        AddRPCTaskRpc();
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        StartCoroutine(playerDialogue.ReadFirstQueuedDialogue());
        //FinishRPCTaskRpc();
    }    
    
    [Rpc(SendTo.ClientsAndHost)]
    public void RequestClientReadFirstDialogueRpc()
    {
        if (IsHost) return;
        //Debug.Log("Adding RPC Task on Client");
        AddRPCTaskRpc();
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        StartCoroutine(playerDialogue.ReadFirstQueuedDialogue());
        //FinishRPCTaskRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void RequestHostReadAllDialogueRpc()
    {
        if (!IsHost)
        {
            Debug.Log("I'm not the Host");
            return;
        }
        //Debug.Log("Requesting Host Read All Dialogue");
        AddRPCTaskRpc();
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        StartCoroutine(playerDialogue.ReadAllQueuedDialogue());
        //FinishRPCTaskRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void RequestClientReadAllDialogueRpc()
    {
        if (IsHost)
        {
            Debug.Log("I'm the Host");
            return;
        }
        //Debug.Log("Adding RPC Task on Client");
        AddRPCTaskRpc();
        //Debug.Log("Requesting Client to read all their dialogue");
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        StartCoroutine(playerDialogue.ReadAllQueuedDialogue());
        //FinishRPCTaskRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SendDialogueToHostRpc(string dialogue)
    {
        //if (!IsHost) return;
        GameObject player = GameObject.Find("Me");
        if (!IsHost)
        {
            Debug.Log("A Client tried to call a host RPC");
            return;
        }
        //Debug.Log("Adding RPC Task on Host");
        AddRPCTaskRpc();
        TrainerController playerTrainerController = player.GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        playerDialogue.AddDialogueToQueue(dialogue);
        FinishRPCTaskRpc();
    }    
    
    [Rpc(SendTo.ClientsAndHost)]
    public void SendDialogueToClientRpc(string dialogue)
    {
        
        GameObject player = GameObject.Find("Me");
        if (IsHost)
        {
            Debug.Log("A host tried to call a client RPC");
            return;
        }
        //Debug.Log("Adding RPC Task on Client");
        AddRPCTaskRpc();
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        playerDialogue.AddDialogueToQueue(dialogue);
        FinishRPCTaskRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void UpdateHealthBarRpc(Attacker attacker, int oldHPValue)
    {
        //Debug.Log("Adding RPC Task on Client and Host");
        AddRPCTaskRpc();
        GameObject player = GameObject.Find("Me");
        if (attacker == Attacker.Trainer1)
        {
            if (IsHost)
            {
                StartCoroutine(player.GetComponentInChildren<OpposingPokemonInfoController>().DrainHP(Menus.OpposingPokemonDamagedScreen, oldHPValue));
            }
            else
            {
                StartCoroutine(player.GetComponentInChildren<PokemonInfoController>().DrainHP(Menus.PokemonDamagedScreen, oldHPValue));
            }
        }
        else
        {
            if (!IsHost)
            {
                StartCoroutine(player.GetComponentInChildren<OpposingPokemonInfoController>().DrainHP(Menus.OpposingPokemonDamagedScreen, oldHPValue));
            }
            else
            {
                StartCoroutine(player.GetComponentInChildren<PokemonInfoController>().DrainHP(Menus.PokemonDamagedScreen, oldHPValue));
            }
        }
    }

    private async void SetTrainer1Move(IPlayerAction action)
    {
        if (IsHost)
        {     
            Debug.Log("Set Trainer 1 Move");
            trainer1Selection = action;
            if (trainer2Selection != null)
            {
                Debug.Log("Had an Early Exit");
                return;
            }
            TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
            DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
            playerDialogue.AddDialogueToQueue("Communicating...");
            RPCManager rpcManager = new RPCManager();
            rpcManager.RPCStarted();
            StartCoroutine(playerDialogue.ReadFirstQueuedDialogue(rpcManager));
            while (!rpcManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
        }
    }

    private void SetTrainer2Move(IPlayerAction action)
    {
        if (IsHost)
        {
            Debug.Log("Set Trainer 2 Move");
            trainer2Selection = action;
            if (trainer1Selection != null)
            {
                Debug.Log("Had an Early Exit");
                return;
            }
        }
    }

    private void UpdateGameState(GameState newState)
    {
        Debug.Log($"Changing GameState to {newState}");
        gameState = newState;
        switch (newState)
        {
            case GameState.BattleStart:
                break;
            case GameState.TurnStart: 
                break;
            case GameState.WaitingOnPlayerInput: 
                break;
            case GameState.ProcessingInput: 
                break;
            case GameState.FirstAttack: 
                break;
            case GameState.SecondAttack: 
                break;
            case GameState.TurnEnd:
                break;
            case GameState.BattleEnd:
                break;
        }

        if (OnStateChange != null)
        {
            // Notify Subscribed Functions of State Change
            OnStateChange.Invoke(gameState);
        }
    }
    
    private void HandleMatchTimeout()
    {
        UpdateGameState(GameState.BattleEnd);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void UpdateGameStateRpc(GameState state)
    {
        //Debug.Log($"New GameState = {state}");
        Instance.UpdateGameState(state);
    }

    private async UniTask ExecuteAttack(Attack playerAction, Pokemon attacker, Pokemon target)
    {
        RPCManager.BeginRPCBatch();
        SendDialogueToClientRpc($"{attacker.GetNickname()} used {playerAction.GetAttackName()}");
        SendDialogueToHostRpc($"{attacker.GetNickname()} used {playerAction.GetAttackName()}");
        while (!RPCManager.AreAllRPCsCompleted())
        {
            await UniTask.Yield();
        }
        uIController.UpdateMenu(Menus.DialogueScreen, 1);
        uIController.UpdateMenu(Menus.DialogueScreen, 2);
        RPCManager.BeginRPCBatch();
        RequestClientReadFirstDialogueRpc();
        RequestHostReadFirstDialogueRpc();
        while (!RPCManager.AreAllRPCsCompleted())
        {
            await UniTask.Yield();
        }
        if (playerAction.PerformAction(attacker, target) != null)
        {
            // May add a history later
            Debug.Log($"{target.GetHPStat()}");
        }
    }

    private async UniTask ExecuteSwitch(Switch playerAction, Trainer trainer, Pokemon newPokemon)
    {
        if (trainer.gameObject.GetComponent<NetworkObject>().IsLocalPlayer)
        {
            RPCManager.BeginRPCBatch();
            SendDialogueToClientRpc($"{trainer.trainerName} sent in {newPokemon.GetNickname()}");
            SendDialogueToHostRpc($"You sent in {newPokemon.GetNickname()}");
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
        }
        else
        {
            RPCManager.BeginRPCBatch();
            SendDialogueToClientRpc($"You sent in {newPokemon.GetNickname()}");
            SendDialogueToHostRpc($"{trainer.trainerName} sent in {newPokemon.GetNickname()}");
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
        }
        RPCManager.BeginRPCBatch();
        if (playerAction.PerformAction(trainer, newPokemon) != null)
        {
            while (!RPCManager.AreAllRPCsCompleted()) 
            { 
                await UniTask.Yield();
            }
            //Debug.Log("I switched pokemon");
            // May add a history here later
            uIController.UpdateMenu(Menus.DialogueScreen, 1);
            uIController.UpdateMenu(Menus.DialogueScreen, 2);
            RPCManager.BeginRPCBatch();
            RequestClientReadAllDialogueRpc();
            RequestHostReadAllDialogueRpc();
            EventsToTriggerManager.AlertEventTriggered(EventsToTrigger.YourPokemonSwitched);
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
            
        }
        Debug.Log("Finished Executing the Switch");
    }

    private async UniTask DecideWhoGoesFirst(IPlayerAction trainer1Action, IPlayerAction trainer2Action)
    {
        Debug.Log("Playing out turn");
        int oldHP;
        if (trainer1Action.GetType() == typeof(Switch) && trainer2Action.GetType() == typeof(Switch))
        {
            Debug.Log("Both players switched");
            Switch convertedTrainer1Action = (Switch)trainer1Action;
            Switch convertedTrainer2Action = (Switch)trainer2Action;
            if (trainer1.GetActivePokemon().GetSpeedStat() > trainer2.GetActivePokemon().GetSpeedStat())
            {

                await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                // May use UI Document here and make the UI Blank on the switch ins
                await MenuBuffer();
                await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                await MenuBuffer();
            }
            else if (trainer1.GetActivePokemon().GetSpeedStat() < trainer2.GetActivePokemon().GetSpeedStat())
            {
                await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                await MenuBuffer();
                await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                await MenuBuffer();
            }
            else
            {
                int speedTie = UnityEngine.Random.Range(0, 99);
                if (speedTie < 50)
                {
                    await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                    await MenuBuffer();
                    await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                    await MenuBuffer();
                }
                else
                {
                    await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                    await MenuBuffer();
                    await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                    await MenuBuffer();
                }
            }
        }
        else if (trainer1Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer1Action = (Switch)trainer1Action;
            await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());            
        }            
        else if (trainer2Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer2Action = (Switch)trainer2Action;
            await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());            
            await MenuBuffer();
        }

        if (trainer1Action.GetType().IsSubclassOf(typeof(Attack)) && trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Debug.Log("Both were Attacks");
            Attack convertedTrainer1Action = (Attack)trainer1Action;
            Attack convertedTrainer2Action = (Attack)trainer2Action;
            if (convertedTrainer1Action.GetAttackPriority() > convertedTrainer2Action.GetAttackPriority())
            {
                //UpdateGameState(GameState.FirstAttack);
                oldHP = trainer2.GetActivePokemon().GetHPStat();
                await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 1);
                uIController.UpdateMenu(Menus.PokemonDamagedScreen, 2);
                await MenuBuffer();
                RPCManager.BeginRPCBatch();
                UpdateHealthBarRpc(Attacker.Trainer1, oldHP);
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }
                //UpdateGameState(GameState.SecondAttack);
                uIController.UpdateMenu(Menus.DialogueScreen, 1);
                uIController.UpdateMenu(Menus.DialogueScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientReadAllDialogueRpc();
                RequestHostReadAllDialogueRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }
                // Give a small buffer inbetween the menu change
                //await MenuBuffer();
                if (!trainer2.GetActivePokemon().IsDead())
                {
                    oldHP = trainer1.GetActivePokemon().GetHPStat();
                    await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                    uIController.UpdateMenu(Menus.PokemonDamagedScreen, 1);
                    uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 2);
                    await MenuBuffer();
                    RPCManager.BeginRPCBatch();
                    UpdateHealthBarRpc(Attacker.Trainer2, oldHP);
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    uIController.UpdateMenu(Menus.DialogueScreen, 1);
                    uIController.UpdateMenu(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    // Give a small buffer inbetween the menu change
                    //await MenuBuffer();
                    // Add an extra if to trigger the same logic if trainer1 dies afterward
                    if (trainer1.GetActivePokemon().IsDead())
                    {
                        if (trainer1.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenu(Menus.LoseScreen, 1);
                            uIController.UpdateMenu(Menus.WinScreen, 2);
                            return;
                        }
                        SendDialogueToClientRpc("Communicating...");
                        uIController.UpdateMenu(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenu(Menus.DialogueScreen, 2);
                        UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadFirstDialogueRpc();
                        RequestHostSwitchRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        // Will not await as the other player will be making a decision and do not want to get in the way of that

                        //var action = await trainer1Controller.SwitchOutFaintedPokemon();
                        Switch playerSwitch = (Switch)trainer1Selection;
                        await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                    }
                }
                else
                {
                    // Force trainer2 to Switch
                    // If not, then end the battle
                    if (trainer2.IsTeamDead())
                    {
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenu(Menus.WinScreen, 1);
                        uIController.UpdateMenu(Menus.LoseScreen, 2);
                        return;
                    }
                    // Will not await as the other player will be making a decision and do not want to get in the way of that
                    UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                    SendDialogueToHostRpc("Communicating...");
                    uIController.UpdateMenu(Menus.PokemonFaintedScreen, 2);
                    uIController.UpdateMenu(Menus.DialogueScreen, 1);
                    RPCManager.BeginRPCBatch();
                    RequestHostReadFirstDialogueRpc();
                    RequestClientSwitchRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    Switch playerSwitch = (Switch)trainer2Selection;
                    await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                }
            }
            else if (convertedTrainer1Action.GetAttackPriority() < convertedTrainer2Action.GetAttackPriority())
            {
                oldHP = trainer1.GetActivePokemon().GetHPStat();
                await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                uIController.UpdateMenu(Menus.PokemonDamagedScreen, 1);
                uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 2);
                await MenuBuffer();
                RPCManager.BeginRPCBatch();
                UpdateHealthBarRpc(Attacker.Trainer2, oldHP);
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }
                uIController.UpdateMenu(Menus.DialogueScreen, 1);
                uIController.UpdateMenu(Menus.DialogueScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientReadAllDialogueRpc();
                RequestHostReadAllDialogueRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }
                if (!trainer1.GetActivePokemon().IsDead())
                {
                    //UpdateGameState(GameState.FirstAttack);
                    oldHP = trainer2.GetActivePokemon().GetHPStat();
                    await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                    uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 1);
                    uIController.UpdateMenu(Menus.PokemonDamagedScreen, 2);
                    await MenuBuffer();
                    //UpdateGameState(GameState.SecondAttack);
                    RPCManager.BeginRPCBatch();
                    UpdateHealthBarRpc(Attacker.Trainer1, oldHP);
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    uIController.UpdateMenu(Menus.DialogueScreen, 1);
                    uIController.UpdateMenu(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    // Give a small buffer inbetween the menu change
                    if (trainer2.GetActivePokemon().IsDead())
                    {
                        if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenu(Menus.WinScreen, 1);
                            uIController.UpdateMenu(Menus.LoseScreen, 2);
                            return;
                        }
                        SendDialogueToHostRpc("Communicating...");
                        uIController.UpdateMenu(Menus.DialogueScreen, 1);
                        uIController.UpdateMenu(Menus.PokemonFaintedScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestHostReadFirstDialogueRpc();
                        RequestClientSwitchRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        // Will not await as the other player will be making a decision and do not want to get in the way of that
                        UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                        Switch playerSwitch = (Switch)trainer2Selection;
                        await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                    }
                }
                else
                {
                    if (trainer1.IsTeamDead())
                    {
                        UpdateGameState(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenu(Menus.LoseScreen, 1);
                        uIController.UpdateMenu(Menus.WinScreen, 2);
                        return;
                    }
                    SendDialogueToClientRpc("Communicating...");
                    uIController.UpdateMenu(Menus.PokemonFaintedScreen, 1);
                    uIController.UpdateMenu(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadFirstDialogueRpc();
                    RequestHostSwitchRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    // Will not await as the other player will be making a decision and do not want to get in the way of that
                    UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                    Switch playerSwitch = (Switch)trainer1Selection;
                    await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                }
            }
            else
            {
                if (trainer1.GetActivePokemon().GetSpeedStat() > trainer2.GetActivePokemon().GetSpeedStat())
                {
                    oldHP = trainer2.GetActivePokemon().GetHPStat();
                    await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                    uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 1);
                    uIController.UpdateMenu(Menus.PokemonDamagedScreen, 2);
                    await MenuBuffer();
                    RPCManager.BeginRPCBatch();
                    UpdateHealthBarRpc(Attacker.Trainer1, oldHP);
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    //UpdateGameState(GameState.SecondAttack);
                    uIController.UpdateMenu(Menus.DialogueScreen, 1);
                    uIController.UpdateMenu(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    // Give a small buffer inbetween the menu change
                    //await MenuBuffer();
                    if (!trainer2.GetActivePokemon().IsDead())
                    {
                        oldHP = trainer1.GetActivePokemon().GetHPStat();
                        await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                        uIController.UpdateMenu(Menus.PokemonDamagedScreen, 1);
                        uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 2);
                        await MenuBuffer();
                        RPCManager.BeginRPCBatch();
                        UpdateHealthBarRpc(Attacker.Trainer2, oldHP);
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        uIController.UpdateMenu(Menus.DialogueScreen, 1);
                        uIController.UpdateMenu(Menus.DialogueScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadAllDialogueRpc();
                        RequestHostReadAllDialogueRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        // Give a small buffer inbetween the menu change
                        //await MenuBuffer();
                        // Add an extra if to trigger the same logic if trainer1 dies afterward
                        if (trainer1.GetActivePokemon().IsDead())
                        {
                            if (trainer1.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenu(Menus.LoseScreen, 1);
                                uIController.UpdateMenu(Menus.WinScreen, 2);
                                return;
                            }
                            SendDialogueToClientRpc("Communicating...");
                            uIController.UpdateMenu(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenu(Menus.DialogueScreen, 2);
                            UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                            RPCManager.BeginRPCBatch();
                            RequestClientReadFirstDialogueRpc();
                            RequestHostSwitchRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            // Will not await as the other player will be making a decision and do not want to get in the way of that

                            //var action = await trainer1Controller.SwitchOutFaintedPokemon();
                            Switch playerSwitch = (Switch)trainer1Selection;
                            await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                        }
                    }
                    else
                    {
                        // Force trainer2 to Switch
                        // If not, then end the battle
                        if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenu(Menus.WinScreen, 1);
                            uIController.UpdateMenu(Menus.LoseScreen, 2);
                            return;
                        }
                        // Will not await as the other player will be making a decision and do not want to get in the way of that
                        UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                        SendDialogueToHostRpc("Communicating...");
                        uIController.UpdateMenu(Menus.PokemonFaintedScreen, 2);
                        uIController.UpdateMenu(Menus.DialogueScreen, 1);
                        RPCManager.BeginRPCBatch();
                        RequestHostReadFirstDialogueRpc();
                        RequestClientSwitchRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        Switch playerSwitch = (Switch)trainer2Selection;
                        await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                    }
                }
                else if (trainer1.GetActivePokemon().GetSpeedStat() < trainer2.GetActivePokemon().GetSpeedStat())
                {
                    oldHP = trainer1.GetActivePokemon().GetHPStat();
                    await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                    uIController.UpdateMenu(Menus.PokemonDamagedScreen, 1);
                    uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 2);
                    await MenuBuffer();
                    RPCManager.BeginRPCBatch();
                    UpdateHealthBarRpc(Attacker.Trainer2, oldHP);
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    uIController.UpdateMenu(Menus.DialogueScreen, 1);
                    uIController.UpdateMenu(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    if (!trainer1.GetActivePokemon().IsDead())
                    {
                        //UpdateGameState(GameState.FirstAttack);
                        oldHP = trainer2.GetActivePokemon().GetHPStat();
                        await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                        uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 1);
                        uIController.UpdateMenu(Menus.PokemonDamagedScreen, 2);
                        await MenuBuffer();
                        //UpdateGameState(GameState.SecondAttack);
                        RPCManager.BeginRPCBatch();
                        UpdateHealthBarRpc(Attacker.Trainer1, oldHP);
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        uIController.UpdateMenu(Menus.DialogueScreen, 1);
                        uIController.UpdateMenu(Menus.DialogueScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadAllDialogueRpc();
                        RequestHostReadAllDialogueRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        // Give a small buffer inbetween the menu change
                        if (trainer2.GetActivePokemon().IsDead())
                        {
                            if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenu(Menus.WinScreen, 1);
                                uIController.UpdateMenu(Menus.LoseScreen, 2);
                                return;
                            }
                            SendDialogueToHostRpc("Communicating...");
                            uIController.UpdateMenu(Menus.DialogueScreen, 1);
                            uIController.UpdateMenu(Menus.PokemonFaintedScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestHostReadFirstDialogueRpc();
                            RequestClientSwitchRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            // Will not await as the other player will be making a decision and do not want to get in the way of that
                            UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                            Switch playerSwitch = (Switch)trainer2Selection;
                            await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                        }
                    }
                    else
                    {
                        if (trainer1.IsTeamDead())
                        {
                            UpdateGameState(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenu(Menus.LoseScreen, 1);
                            uIController.UpdateMenu(Menus.WinScreen, 2);
                            return;
                        }
                        SendDialogueToClientRpc("Communicating...");
                        uIController.UpdateMenu(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenu(Menus.DialogueScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadFirstDialogueRpc();
                        RequestHostSwitchRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        // Will not await as the other player will be making a decision and do not want to get in the way of that
                        UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                        Switch playerSwitch = (Switch)trainer1Selection;
                        await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                    }
                }
                else
                {
                    int speedTie = UnityEngine.Random.Range(0, 99);
                    Debug.Log("Speed Tie");
                    if (speedTie < 50)
                    {
                        oldHP = trainer2.GetActivePokemon().GetHPStat();
                        await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                        uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 1);
                        uIController.UpdateMenu(Menus.PokemonDamagedScreen, 2);
                        await MenuBuffer();
                        RPCManager.BeginRPCBatch();
                        UpdateHealthBarRpc(Attacker.Trainer1, oldHP);
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        //UpdateGameState(GameState.SecondAttack);
                        uIController.UpdateMenu(Menus.DialogueScreen, 1);
                        uIController.UpdateMenu(Menus.DialogueScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadAllDialogueRpc();
                        RequestHostReadAllDialogueRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        // Give a small buffer inbetween the menu change
                        //await MenuBuffer();
                        if (!trainer2.GetActivePokemon().IsDead())
                        {
                            oldHP = trainer1.GetActivePokemon().GetHPStat();
                            await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                            uIController.UpdateMenu(Menus.PokemonDamagedScreen, 1);
                            uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 2);
                            await MenuBuffer();
                            RPCManager.BeginRPCBatch();
                            UpdateHealthBarRpc(Attacker.Trainer2, oldHP);
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            uIController.UpdateMenu(Menus.DialogueScreen, 1);
                            uIController.UpdateMenu(Menus.DialogueScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestClientReadAllDialogueRpc();
                            RequestHostReadAllDialogueRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            // Give a small buffer inbetween the menu change
                            //await MenuBuffer();
                            // Add an extra if to trigger the same logic if trainer1 dies afterward
                            if (trainer1.GetActivePokemon().IsDead())
                            {
                                if (trainer1.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenu(Menus.LoseScreen, 1);
                                    uIController.UpdateMenu(Menus.WinScreen, 2);
                                    return;
                                }
                                SendDialogueToClientRpc("Communicating...");
                                uIController.UpdateMenu(Menus.PokemonFaintedScreen, 1);
                                uIController.UpdateMenu(Menus.DialogueScreen, 2);
                                UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                                RPCManager.BeginRPCBatch();
                                RequestClientReadFirstDialogueRpc();
                                RequestHostSwitchRpc();
                                while (!RPCManager.AreAllRPCsCompleted())
                                {
                                    await UniTask.Yield();
                                }
                                // Will not await as the other player will be making a decision and do not want to get in the way of that

                                //var action = await trainer1Controller.SwitchOutFaintedPokemon();
                                Switch playerSwitch = (Switch)trainer1Selection;
                                await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                            }
                        }
                        else
                        {
                            // Force trainer2 to Switch
                            // If not, then end the battle
                            if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenu(Menus.WinScreen, 1);
                                uIController.UpdateMenu(Menus.LoseScreen, 2);
                                return;
                            }
                            // Will not await as the other player will be making a decision and do not want to get in the way of that
                            UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                            SendDialogueToHostRpc("Communicating...");
                            uIController.UpdateMenu(Menus.PokemonFaintedScreen, 2);
                            uIController.UpdateMenu(Menus.DialogueScreen, 1);
                            RPCManager.BeginRPCBatch();
                            RequestHostReadFirstDialogueRpc();
                            RequestClientSwitchRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            Switch playerSwitch = (Switch)trainer2Selection;
                            await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                        }
                    }
                    else
                    {
                        oldHP = trainer1.GetActivePokemon().GetHPStat();
                        await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                        uIController.UpdateMenu(Menus.PokemonDamagedScreen, 1);
                        uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 2);
                        await MenuBuffer();
                        RPCManager.BeginRPCBatch();
                        UpdateHealthBarRpc(Attacker.Trainer2, oldHP);
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        uIController.UpdateMenu(Menus.DialogueScreen, 1);
                        uIController.UpdateMenu(Menus.DialogueScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadAllDialogueRpc();
                        RequestHostReadAllDialogueRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        if (!trainer1.GetActivePokemon().IsDead())
                        {
                            //UpdateGameState(GameState.FirstAttack);
                            oldHP = trainer2.GetActivePokemon().GetHPStat();
                            await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                            uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 1);
                            uIController.UpdateMenu(Menus.PokemonDamagedScreen, 2);
                            await MenuBuffer();
                            //UpdateGameState(GameState.SecondAttack);
                            RPCManager.BeginRPCBatch();
                            UpdateHealthBarRpc(Attacker.Trainer1, oldHP);
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            uIController.UpdateMenu(Menus.DialogueScreen, 1);
                            uIController.UpdateMenu(Menus.DialogueScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestClientReadAllDialogueRpc();
                            RequestHostReadAllDialogueRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            // Give a small buffer inbetween the menu change
                            if (trainer2.GetActivePokemon().IsDead())
                            {
                                if (trainer2.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenu(Menus.WinScreen, 1);
                                    uIController.UpdateMenu(Menus.LoseScreen, 2);
                                    return;
                                }
                                SendDialogueToHostRpc("Communicating...");
                                uIController.UpdateMenu(Menus.DialogueScreen, 1);
                                uIController.UpdateMenu(Menus.PokemonFaintedScreen, 2);
                                RPCManager.BeginRPCBatch();
                                RequestHostReadFirstDialogueRpc();
                                RequestClientSwitchRpc();
                                while (!RPCManager.AreAllRPCsCompleted())
                                {
                                    await UniTask.Yield();
                                }
                                // Will not await as the other player will be making a decision and do not want to get in the way of that
                                UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                                Switch playerSwitch = (Switch)trainer2Selection;
                                await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                            }
                        }
                        else
                        {
                            if (trainer1.IsTeamDead())
                            {
                                UpdateGameState(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenu(Menus.LoseScreen, 1);
                                uIController.UpdateMenu(Menus.WinScreen, 2);
                                return;
                            }
                            SendDialogueToClientRpc("Communicating...");
                            uIController.UpdateMenu(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenu(Menus.DialogueScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestClientReadFirstDialogueRpc();
                            RequestHostSwitchRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            // Will not await as the other player will be making a decision and do not want to get in the way of that
                            UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                            Switch playerSwitch = (Switch)trainer1Selection;
                            await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                        }
                    }
                }
            }
        }
        else if (trainer1Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer1Action = (Attack)trainer1Action;
            //UpdateGameState(GameState.FirstAttack);
            oldHP = trainer2.GetActivePokemon().GetHPStat();
            await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
            uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 1);
            uIController.UpdateMenu(Menus.PokemonDamagedScreen, 2);
            await MenuBuffer();
            RPCManager.BeginRPCBatch();
            UpdateHealthBarRpc(Attacker.Trainer1, oldHP);
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
            //UpdateGameState(GameState.SecondAttack);
            uIController.UpdateMenu(Menus.DialogueScreen, 1);
            uIController.UpdateMenu(Menus.DialogueScreen, 2);
            RPCManager.BeginRPCBatch();
            RequestClientReadAllDialogueRpc();
            RequestHostReadAllDialogueRpc();
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
            if (trainer2.GetActivePokemon().IsDead())
            {
                if (trainer2.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenu(Menus.WinScreen, 1);
                    uIController.UpdateMenu(Menus.LoseScreen, 2);
                    return;
                }
                // Will not await as the other player will be making a decision and do not want to get in the way of that
                UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                SendDialogueToHostRpc("Communicating...");
                uIController.UpdateMenu(Menus.PokemonFaintedScreen, 2);
                uIController.UpdateMenu(Menus.DialogueScreen, 1);
                RPCManager.BeginRPCBatch();
                RequestHostReadFirstDialogueRpc();
                RequestClientSwitchRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }
                Switch playerSwitch = (Switch)trainer2Selection;
                await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
            }
        }
        else if (trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer2Action = (Attack)trainer2Action;
            oldHP = trainer1.GetActivePokemon().GetHPStat();
            await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
            //await UniTask.WhenAll(trainer1PokemonInfoController.UpdateHealthBar(Menus.PokemonDamagedScreen), trainer2OpposingPokemonInfoBarController.UpdateHealthBar(Menus.OpposingPokemonDamagedScreen));
            uIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen, 2);
            uIController.UpdateMenu(Menus.PokemonDamagedScreen, 1);
            await MenuBuffer();
            RPCManager.BeginRPCBatch();
            UpdateHealthBarRpc(Attacker.Trainer2, oldHP);
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
            //UpdateGameState(GameState.SecondAttack);
            uIController.UpdateMenu(Menus.DialogueScreen, 1);
            uIController.UpdateMenu(Menus.DialogueScreen, 2);
            RPCManager.BeginRPCBatch();
            RequestClientReadAllDialogueRpc();
            RequestHostReadAllDialogueRpc();
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
            if (trainer1.GetActivePokemon().IsDead())
            {
                if (trainer1.IsTeamDead())
                {
                    UpdateGameState(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenu(Menus.LoseScreen, 1);
                    uIController.UpdateMenu(Menus.WinScreen, 2);
                    return;
                }
                UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                SendDialogueToClientRpc("Communicating...");
                uIController.UpdateMenu(Menus.PokemonFaintedScreen, 1);
                uIController.UpdateMenu(Menus.DialogueScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientReadFirstDialogueRpc();
                RequestHostSwitchRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }
                Switch playerSwitch = (Switch)trainer1Selection;
                await ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
            }
            //uIController.UpdateMenu(Menus.GeneralBattleMenu, 1);
        }
        // Also may add a forfiet button

        // Count the number of active RPCs before exiting the function
        RPCManager.CurrentRPCCount();
        uIController.UpdateMenu(Menus.GeneralBattleMenu, 1);
        uIController.UpdateMenu(Menus.GeneralBattleMenu, 2);
    }
}
