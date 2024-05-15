using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Unity.Netcode;
using Unity.Collections;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private TrainerController trainer1Controller;
    [SerializeField] private TrainerController trainer2Controller;
    private static Trainer trainer1;
    private static Trainer trainer2;
    private static UIController uIController;
    public static event Action GameManagerSpawned;
    [SerializeField] private static GameState gameState;
    public static event Action<GameState> OnStateChange;
    public static event Action<Attack> LastAttack;
    private LobbyManager lobbyManager;
    [SerializeField] private EventsToTriggerManager EventsToTriggerManager;
    private bool player1Inactive = false;
    private bool player2Inactive = false;
    public RPCManager RPCManager;
    IPlayerAction trainer1Selection;
    IPlayerAction trainer2Selection;
    [SerializeField] private EffectQueueController effectQueueController;
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
        }

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
        GameObject eventsToTriggerGO = GameObject.Find("EventsToTriggerManager");
        if (eventsToTriggerGO != null)
        {
            EventsToTriggerManager = eventsToTriggerGO.GetComponent<EventsToTriggerManager>();
        }
        else
        {
            Debug.Log("Unable to find the GameObject called \'EventsToTriggerManager\'");
        }
        GameObject effectQueueControllerGO = GameObject.Find("EffectQueueController");
        if (effectQueueControllerGO != null)
        {
            effectQueueController = effectQueueControllerGO.GetComponent<EffectQueueController>();
        }
        else
        {
            Debug.Log("Unable to find the GameObject called \'EffectQueueController\'");
        }
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
            PlaceTrainer(playerObject, 1); 
        }
        else
        {
            //playerObject.name = "Trainer 2";
            Debug.Log("Populating the GameManager with Trainer 2 Data");
            PopulateGameManager(playerObject, 2);
            PlaceTrainer(playerObject, 2);
        }
    }

    private void PlaceTrainer(GameObject gameObject, int player)
    {
        if (player == 1) // If this is the host joining
        {            
            gameObject.transform.position = new Vector3(43.09f, 0, 41.97f);
            gameObject.transform.eulerAngles = new Vector3(0f, -127f, 0f);
        }
        else // If this is a client joining
        {
            gameObject.transform.position = new Vector3(31.93f, 0f, 33.32f);
            gameObject.transform.eulerAngles = new Vector3(0f, 52f, 0f);
        }
    }

    private void HandlePlayerConnection()
    {
        if (!IsServer)
        {
            //Debug.Log("I'm not the host");
            return;
        }
        // if two players have connected then we would like to start the game
        Debug.Log("Two Players have Joined");
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
        //UpdateGameStateRpc(GameState.LoadingPokemonInfo);
        UpdateGameStateRpc(GameState.LoadingPokemonInfo);
        time = 0f;
        while (time < 2f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        //UpdateGameStateRpc(GameState.BattleStart);
        UpdateGameStateRpc(GameState.BattleStart);
        uIController.UpdateMenuRpc(Menus.BlankScreen, 1);
        uIController.UpdateMenuRpc(Menus.BlankScreen, 2);
        EventsToTriggerManager.AlertEventTriggered(EventsToTrigger.YourPokemonSwitched);
        EventsToTriggerManager.AlertEventTriggered(EventsToTrigger.OpposingPokemonSwitched);
        await MenuBuffer();
        if (trainer1Controller.GetDialogueBoxController().QueueSize() > 0)
        {
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
            RPCManager.BeginRPCBatch();
            RequestClientReadAllDialogueRpc();
            RequestHostReadAllDialogueRpc();
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
        }
        uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 1);
        uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 2);
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

    public async UniTask MenuBuffer()
    {
        Debug.Log("Giving the screen a buffer");
        await UniTask.WaitForSeconds(1);
    }

    private async UniTask TurnSystem()
    {
        //dialogueBoxController.SendDialogueToHostRpc("Hello There Server");
        //await dialogueBoxController.MakeHostReadFirstDialogueRpc();
        await effectQueueController.EmptyQueue();
        while (gameState != GameState.BattleEnd)
        {
            UpdateGameStateRpc(GameState.TurnStart);
            UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
            RPCManager.BeginRPCBatch();
            RequestMoveSelectionRpc();
            while (!RPCManager.AreAllRPCsCompleted())
            {
                //RPCManager.CurrentRPCCount();
                await UniTask.Yield();
            }
            Debug.Log("Both Players have made a Selection");

            //uIController.UpdateMenuRpc(Menus.BlankScreen, 1);
            //uIController.UpdateMenuRpc(Menus.BlankScreen, 2);
            //await MenuBuffer();
            // Need to add for a case where both players have gone inactive
            //if (player1Inactive)
            //{
            //    UpdateGameStateRpc(GameState.BattleEnd);
            //    break;
            //}
            //else if (player2Inactive)
            //{
            //    UpdateGameStateRpc(GameState.BattleEnd);
            //    break;
            //}
            UpdateGameStateRpc(GameState.ProcessingInput);
            await PlayOutTurn(trainer1Selection, trainer2Selection);
            trainer1Selection = null;
            trainer2Selection = null;
        }
    }

    //[Rpc(SendTo.ClientsAndHost)]
    //private void TestRpc()
    //{
    //    Debug.Log("This is a test to see if the client recieved an RPC");
    //}
    [Rpc(SendTo.ClientsAndHost)]
    public void UpdateGameStateRpc(GameState state)
    {
        UpdateGameState(state);
    }

    [Rpc(SendTo.Server)]
    public void AddRPCTaskRpc()
    {
        Debug.Log("Adding an RPC");
        RPCManager.RPCStarted();
    }    
    [Rpc(SendTo.Server)]
    public void AddRPCTaskRpc(string functionName)
    {
        Debug.Log($"Adding an RPC, requested by {functionName}");
        RPCManager.RPCStarted();
    }

    [Rpc(SendTo.Server)]
    public void FinishRPCTaskRpc()
    {
        RPCManager.RPCFinished();
    }

    [Rpc(SendTo.Server)]
    private void RequestHostSwitchRpc()
    {
        Debug.Log("Requesting a move from the player");
        //Debug.Log("Adding RPC Task on Host");
        AddRPCTaskRpc();
        var playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        playerTrainerController.SendMoveSelect(1);
    }

    [Rpc(SendTo.NotServer)]
    private void RequestClientSwitchRpc()
    {
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
        Debug.Log("Adding RPC Task on Client and Host");
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
        uIController.UpdateMenuRpc(Menus.DialogueScreen, type);
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
            //SetClientDialogue();
            //moveSelectionRPCManager.RPCFinished();
            
        }
        UpdateMoveInfoRpc(type, attackRPCTrasfer.attackName);
        FinishRPCTaskRpc();
        return;
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateMoveInfoRpc(int type, FixedString128Bytes attackName)
    {
        Debug.Log($"Hello from Move Update Method, type {type}");
        GameObject player = GameObject.Find("Me");
        string atkName = attackName.ToString();
        if (type == 1 && IsHost) // Host
        {
            foreach (var attack in player.GetComponent<Trainer>().GetActivePokemon().GetMoveset())
            {
                if (attack.GetType().Name.Equals(atkName))
                {
                    if (player.GetComponent<Trainer>().GetActivePokemon().IsPressured)
                    {
                        if (attack.GetCurrentPP() <= 1)
                        {
                            attack.SetCurrentPP(0);
                        }
                        else
                        {
                            attack.SetCurrentPP(attack.GetCurrentPP() - 2);
                        }

                    }
                    else
                    {
                        attack.SetCurrentPP(attack.GetCurrentPP() - 1);
                    }
                    Debug.Log($"Host: Current BP after RPC {attack.GetCurrentPP()}");
                }
            }

        }
        else if (type == 2 && !IsHost) // Client
        {
            foreach (var attack in player.GetComponent<Trainer>().GetActivePokemon().GetMoveset())
            {
                if (attack.GetType().Name.Equals(atkName))
                {
                    if (player.GetComponent<Trainer>().GetActivePokemon().IsPressured)
                    {
                        if (attack.GetCurrentPP() <= 1)
                        {
                            attack.SetCurrentPP(0);
                        }
                        else
                        {
                            attack.SetCurrentPP(attack.GetCurrentPP() - 2);
                        }
                    }
                    else
                    {
                        attack.SetCurrentPP(attack.GetCurrentPP() - 1);
                    }
                    Debug.Log($"Client: Current BP after RPC {attack.GetCurrentPP()}");
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
        uIController.UpdateMenuRpc(Menus.DialogueScreen, type);
        if (type == 1) // Host
        {            
            SetTrainer1Move(playerSwitch);
        }
        else if (type == 2) // Client
        {   
            SetTrainer2Move(playerSwitch);
            //SetClientDialogue();
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

    [Rpc(SendTo.Server)]
    public void RequestHostReadFirstDialogueRpc()
    {
        //Debug.Log("Adding RPC Task on Host");
        AddRPCTaskRpc("RequestHostReadFirstDialogueRpc");
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        StartCoroutine(playerDialogue.ReadFirstQueuedDialogue());
        //FinishRPCTaskRpc();
    }    
    
    [Rpc(SendTo.NotServer)]
    public void RequestClientReadFirstDialogueRpc()
    {
        //Debug.Log("Adding RPC Task on Client");
        AddRPCTaskRpc("RequestClientReadFirstDialogueRpc");
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        StartCoroutine(playerDialogue.ReadFirstQueuedDialogue());
        //FinishRPCTaskRpc();
    }

    [Rpc(SendTo.Server)]
    public void RequestHostReadAllDialogueRpc()
    {
        //Debug.Log("Requesting Host Read All Dialogue");
        AddRPCTaskRpc("RequestHostReadAllDialogueRpc");
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        StartCoroutine(playerDialogue.ReadAllQueuedDialogue());
        //FinishRPCTaskRpc();
    }

    [Rpc(SendTo.NotServer)]
    public void RequestClientReadAllDialogueRpc()
    {
        if (IsHost)
        {
            return;
        }
        //Debug.Log("Adding RPC Task on Client");
        AddRPCTaskRpc("RequestClientReadAllDialogueRpc");
        //Debug.Log("Requesting Client to read all their dialogue");
        TrainerController playerTrainerController = GameObject.Find("Me").GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        StartCoroutine(playerDialogue.ReadAllQueuedDialogue());
        //FinishRPCTaskRpc();
    }

    [Rpc(SendTo.Server)]
    public void SendDialogueToHostRpc(string dialogue)
    {
        GameObject player = GameObject.Find("Me");
        //Debug.Log("Adding RPC Task on Host");
        AddRPCTaskRpc("SendDialogueToHostRpc");
        TrainerController playerTrainerController = player.GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        playerDialogue.AddDialogueToQueue(dialogue);
        Debug.Log("Added Dialogue to the Host");
        FinishRPCTaskRpc();
    }    
    
    [Rpc(SendTo.NotServer)]
    public void SendDialogueToClientRpc(string dialogue)
    {
        GameObject player = GameObject.Find("Me");
        AddRPCTaskRpc("SendDialogueToClientRpc");
        TrainerController playerTrainerController = player.GetComponent<TrainerController>();
        DialogueBoxController playerDialogue = playerTrainerController.GetDialogueBoxController();
        playerDialogue.AddDialogueToQueue(dialogue);
        Debug.Log("Added Dialogue to the Client");
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
                //uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 2);
                //uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 1);
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
                //uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 1);
                //uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 2);
                StartCoroutine(player.GetComponentInChildren<PokemonInfoController>().DrainHP(Menus.PokemonDamagedScreen, oldHPValue));
            }
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SendLastAttackToClientRpc(string attackName)
    {
        AddRPCTaskRpc();
        Attack attack = (Attack)Activator.CreateInstance(System.Type.GetType(attackName.ToString().Replace(" ", "")));
        LastAttack?.Invoke(attack);
        FinishRPCTaskRpc();
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
            rpcManager.BeginRPCBatch();
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
        Debug.Log("Set Trainer 2 Move");
        trainer2Selection = action;
        if (trainer1Selection != null)
        {
            Debug.Log("Had an Early Exit");
            return;
        }
        SetClientDialogue();
        //int activeRPCs = RPCManager.ActiveRPCs();
        //SendDialogueToClientRpc("Communicating...");
        //while (RPCManager.ActiveRPCs() > activeRPCs)
        //{
        //    await UniTask.Yield();
        //}
        //RequestClientReadFirstDialogueRpc();
        //while (RPCManager.ActiveRPCs() > activeRPCs)
        //{
        //    await UniTask.Yield();
        //}
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

    public void AlertOfLastAttack(Attack attack)
    {
        LastAttack?.Invoke(attack);
        SendLastAttackToClientRpc(attack.GetType().Name);
    }

    private void HandleMatchTimeout()
    {
        UpdateGameStateRpc(GameState.BattleEnd);
    }



    private async UniTask ExecuteAttack(Attack playerAction, Pokemon attacker, Pokemon target)
    {
        int oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
        int oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
        bool isLocalPlayer = attacker.gameObject.transform.parent.parent.GetComponent<NetworkObject>().IsLocalPlayer;
        RPCManager.BeginRPCBatch();
        if (isLocalPlayer)
        {
            RPCManager.BeginRPCBatch();
            SendDialogueToClientRpc($"Your Opponent's {attacker.GetNickname()} used {playerAction.GetAttackName()}");
            SendDialogueToHostRpc($"Your {attacker.GetNickname()} used {playerAction.GetAttackName()}");
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
        }
        else
        {
            RPCManager.BeginRPCBatch();
            SendDialogueToClientRpc($"Your {attacker.GetNickname()} used {playerAction.GetAttackName()}");
            SendDialogueToHostRpc($"Your Opponent's {attacker.GetNickname()} used {playerAction.GetAttackName()}");
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
        }
        while (!RPCManager.AreAllRPCsCompleted())
        {
            
            await UniTask.Yield();
        }
        uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
        uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
        RPCManager.BeginRPCBatch();
        RequestClientReadFirstDialogueRpc();
        RequestHostReadFirstDialogueRpc();
        while (!RPCManager.AreAllRPCsCompleted())
        {
            //RPCManager.CurrentRPCCount();
            await UniTask.Yield();
        }
        if (playerAction.PerformAction(attacker, target) != null)
        {
            AlertOfLastAttack(playerAction);
            while (!RPCManager.AreAllRPCsCompleted())
            {
                //RPCManager.CurrentRPCCount();
                await UniTask.Yield();
            }
            if (isLocalPlayer)
            {
                if (trainer2.GetActivePokemon().GetHPStat() < oldHPTrainer2 || trainer2.GetActivePokemon().GetHPStat() > oldHPTrainer2)
                {
                    uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 1);
                    uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 2);
                    await MenuBuffer();
                    RPCManager.BeginRPCBatch();
                    UpdateHealthBarRpc(Attacker.Trainer1, oldHPTrainer2);
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                }

                uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientReadAllDialogueRpc();
                RequestHostReadAllDialogueRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }

                if (trainer1.GetActivePokemon().GetHPStat() < oldHPTrainer1 || trainer1.GetActivePokemon().GetHPStat() > oldHPTrainer1)
                {
                    uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 1);
                    uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 2);
                    await MenuBuffer();
                    RPCManager.BeginRPCBatch();
                    UpdateHealthBarRpc(Attacker.Trainer2, oldHPTrainer1);
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                }
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientReadAllDialogueRpc();
                RequestHostReadAllDialogueRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                if (trainer1.GetActivePokemon().GetHPStat() < oldHPTrainer1 || trainer1.GetActivePokemon().GetHPStat() > oldHPTrainer1)
                {
                    uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 1);
                    uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 2);
                    await MenuBuffer();
                    RPCManager.BeginRPCBatch();
                    UpdateHealthBarRpc(Attacker.Trainer2, oldHPTrainer1);
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                }
                if (trainer1Controller.GetDialogueBoxController().QueueSize() > 0)
                {
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                }

                if (trainer2.GetActivePokemon().GetHPStat() < oldHPTrainer2 || trainer2.GetActivePokemon().GetHPStat() > oldHPTrainer2)
                {
                    uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 1);
                    uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 2);
                    await MenuBuffer();
                    RPCManager.BeginRPCBatch();
                    UpdateHealthBarRpc(Attacker.Trainer1, oldHPTrainer2);
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                }
                if (trainer1Controller.GetDialogueBoxController().QueueSize() > 0)
                {
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                }
            }
            if (isLocalPlayer)
            {
                EventsToTriggerManager.AlertEventTriggered(EventsToTrigger.YourPokemonAttackedOpposingPokemon);
            }
            else
            {
                EventsToTriggerManager.AlertEventTriggered(EventsToTrigger.OpposingPokemonAttackedYourPokemon);
            }
            //Buffer for dialogue not to overlap
            uIController.UpdateMenuRpc(Menus.BlankScreen, 1);
            uIController.UpdateMenuRpc(Menus.BlankScreen, 2);
            await MenuBuffer();
            // May add a history later
            //Debug.Log($"{target.GetHPStat()}");
        }
    }

    private async UniTask ExecuteSwitch(Switch playerAction, Trainer trainer, Pokemon newPokemon)
    {
        Debug.Log("Executing Switch");
        Debug.Log($"Pokemon is {playerAction.GetPokemon().GetNickname()}");
        Debug.Log($"Trainer Name is {playerAction.GetTrainer().trainerName}");
        bool isLocalPlayer = trainer.gameObject.GetComponent<NetworkObject>().IsLocalPlayer;
        if (isLocalPlayer)
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
            await MenuBuffer();
            if (isLocalPlayer)
            {
                EventsToTriggerManager.AlertEventTriggered(EventsToTrigger.YourPokemonSwitched);
            }
            else
            {
                EventsToTriggerManager.AlertEventTriggered(EventsToTrigger.OpposingPokemonSwitched);
            }
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
            RPCManager.BeginRPCBatch();
            RequestClientReadAllDialogueRpc();
            RequestHostReadAllDialogueRpc();
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
        }
        Debug.Log("Finished Executing the Switch");
    }

    private async UniTask PlayOutTurn(IPlayerAction trainer1Action, IPlayerAction trainer2Action)
    {
        Debug.Log("Playing out turn");
        int oldHPTrainer1;
        int oldHPTrainer2;
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
                oldHPTrainer1 = convertedTrainer1Action.GetPokemon().GetHPStat();
                await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                await MenuBuffer();
                oldHPTrainer2 = convertedTrainer2Action.GetPokemon().GetHPStat();
            }
            else if (trainer1.GetActivePokemon().GetSpeedStat() < trainer2.GetActivePokemon().GetSpeedStat())
            {
                await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                await MenuBuffer();
                oldHPTrainer2 = convertedTrainer2Action.GetPokemon().GetHPStat();
                await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                await MenuBuffer();
                oldHPTrainer1 = convertedTrainer1Action.GetPokemon().GetHPStat();
            }
            else
            {
                int speedTie = UnityEngine.Random.Range(0, 99);
                if (speedTie < 50)
                {
                    await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                    await MenuBuffer();
                    oldHPTrainer1 = convertedTrainer1Action.GetPokemon().GetHPStat();
                    await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                    await MenuBuffer();
                    oldHPTrainer2 = convertedTrainer2Action.GetPokemon().GetHPStat();
                }
                else
                {
                    await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                    await MenuBuffer();
                    oldHPTrainer2 = convertedTrainer2Action.GetPokemon().GetHPStat();
                    await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                    await MenuBuffer();
                    oldHPTrainer1 = convertedTrainer1Action.GetPokemon().GetHPStat();
                }
            }
        }
        else if (trainer1Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer1Action = (Switch)trainer1Action;
            await ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
            await MenuBuffer();
            oldHPTrainer1 = convertedTrainer1Action.GetPokemon().GetHPStat();
        }            
        else if (trainer2Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer2Action = (Switch)trainer2Action;
            await ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());            
            await MenuBuffer();
            oldHPTrainer2 = convertedTrainer2Action.GetPokemon().GetHPStat();
        }

        if (trainer1Action.GetType().IsSubclassOf(typeof(Attack)) && trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Debug.Log("Both were Attacks");
            Attack convertedTrainer1Action = (Attack)trainer1Action;
            Attack convertedTrainer2Action = (Attack)trainer2Action;
            if (convertedTrainer1Action.GetAttackPriority() > convertedTrainer2Action.GetAttackPriority())
            {
                //UpdateGameStateRpc(GameState.FirstAttack);
                oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                await effectQueueController.EmptyQueue();
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientReadAllDialogueRpc();
                RequestHostReadAllDialogueRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }
                //UpdateGameStateRpc(GameState.SecondAttack);
                if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                {
                    // Let attacker win
                    if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                    {
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                        uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                        return;
                    }
                    else if (trainer1.IsTeamDead())
                    {
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                        uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                        return;
                    }
                    else if (trainer2.IsTeamDead())
                    {
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                        uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                        return;
                    }
                    uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                    uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientSwitchRpc();
                    RequestHostSwitchRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }

                    Switch playerSwitch1 = (Switch)trainer1Selection;
                    Switch playerSwitch2 = (Switch)trainer2Selection;
                    await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                }
                else if (!trainer2.GetActivePokemon().IsDead())
                {
                    oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                    oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                    await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                    await effectQueueController.EmptyQueue();
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                    if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                    {
                        // Let attacker win
                        if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        else if (trainer1.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                            return;
                        }
                        else if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientSwitchRpc();
                        RequestHostSwitchRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }

                        Switch playerSwitch1 = (Switch)trainer1Selection;
                        Switch playerSwitch2 = (Switch)trainer2Selection;
                        await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                    }
                    else if (trainer1.GetActivePokemon().IsDead())
                    {
                        if (trainer1.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                            uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                            return;
                        }
                        SendDialogueToClientRpc("Communicating...");
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                    else if (trainer2.GetActivePokemon().IsDead())
                    {
                        if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        SendDialogueToHostRpc("Communicating...");
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
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
                    // Force trainer2 to Switch
                    // If not, then end the battle
                    if (trainer2.IsTeamDead())
                    {
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                        uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                        return;
                    }
                    // Will not await as the other player will be making a decision and do not want to get in the way of that
                    UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                    SendDialogueToHostRpc("Communicating...");
                    uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
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
                oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                await effectQueueController.EmptyQueue();
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientReadAllDialogueRpc();
                RequestHostReadAllDialogueRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }

                if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                {
                    // Let attacker win
                    if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                    {
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                        uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                        return;
                    }
                    else if (trainer1.IsTeamDead())
                    {
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                        uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                        return;
                    }
                    else if (trainer2.IsTeamDead())
                    {
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                        uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                        return;
                    }
                    uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                    uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientSwitchRpc();
                    RequestHostSwitchRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }

                    Switch playerSwitch1 = (Switch)trainer1Selection;
                    Switch playerSwitch2 = (Switch)trainer2Selection;
                    await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                }
                else if (!trainer1.GetActivePokemon().IsDead())
                {
                    //UpdateGameStateRpc(GameState.FirstAttack);
                    oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                    oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                    await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                    await effectQueueController.EmptyQueue();
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    // Give a small buffer inbetween the menu change
                    if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                    {
                        // Let attacker win
                        if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        else if (trainer1.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                            return;
                        }
                        else if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientSwitchRpc();
                        RequestHostSwitchRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }

                        Switch playerSwitch1 = (Switch)trainer1Selection;
                        Switch playerSwitch2 = (Switch)trainer2Selection;
                        await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                    }
                    else if (trainer1.GetActivePokemon().IsDead())
                    {
                        if (trainer1.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                            uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                            return;
                        }
                        SendDialogueToClientRpc("Communicating...");
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                    else if (trainer2.GetActivePokemon().IsDead())
                    {
                        if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        SendDialogueToHostRpc("Communicating...");
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
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
                        UpdateGameStateRpc(GameState.BattleEnd);
                        //Give out the corresponding screen to each player
                        uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                        uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                        return;
                    }
                    SendDialogueToClientRpc("Communicating...");
                    uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                    oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                    oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                    await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                    await effectQueueController.EmptyQueue();
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }
                    // Give a small buffer inbetween the menu change
                    //await MenuBuffer();

                    if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                    {
                        // Let attacker win
                        if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        else if (trainer1.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                            return;
                        }
                        else if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientSwitchRpc();
                        RequestHostSwitchRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }

                        Switch playerSwitch1 = (Switch)trainer1Selection;
                        Switch playerSwitch2 = (Switch)trainer2Selection;
                        await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                    }
                    else if (!trainer2.GetActivePokemon().IsDead())
                    {
                        oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                        oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                        await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                        await effectQueueController.EmptyQueue();
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                        if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                        {
                            // Let attacker win
                            if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            else if (trainer1.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                return;
                            }
                            else if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestClientSwitchRpc();
                            RequestHostSwitchRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }

                            Switch playerSwitch1 = (Switch)trainer1Selection;
                            Switch playerSwitch2 = (Switch)trainer2Selection;
                            await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                        }
                        else if (trainer1.GetActivePokemon().IsDead())
                        {
                            if (trainer1.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                return;
                            }
                            SendDialogueToClientRpc("Communicating...");
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                        else if (trainer2.GetActivePokemon().IsDead())
                        {
                            if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            SendDialogueToHostRpc("Communicating...");
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
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
                        // Force trainer2 to Switch
                        // If not, then end the battle
                        if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        // Will not await as the other player will be making a decision and do not want to get in the way of that
                        UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                        SendDialogueToHostRpc("Communicating...");
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
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
                    oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                    oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                    await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                    await effectQueueController.EmptyQueue();
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                    uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                    RPCManager.BeginRPCBatch();
                    RequestClientReadAllDialogueRpc();
                    RequestHostReadAllDialogueRpc();
                    while (!RPCManager.AreAllRPCsCompleted())
                    {
                        await UniTask.Yield();
                    }

                    if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                    {
                        // Let attacker win
                        if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        else if (trainer1.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                            return;
                        }
                        else if (trainer2.IsTeamDead())
                        {
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                            return;
                        }
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientSwitchRpc();
                        RequestHostSwitchRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }

                        Switch playerSwitch1 = (Switch)trainer1Selection;
                        Switch playerSwitch2 = (Switch)trainer2Selection;
                        await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                    }
                    else if (!trainer1.GetActivePokemon().IsDead())
                    {
                        //UpdateGameStateRpc(GameState.FirstAttack);
                        oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                        oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                        await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                        await effectQueueController.EmptyQueue();
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadAllDialogueRpc();
                        RequestHostReadAllDialogueRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        // Give a small buffer inbetween the menu change
                        if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                        {
                            // Let attacker win
                            if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            else if (trainer1.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                return;
                            }
                            else if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestClientSwitchRpc();
                            RequestHostSwitchRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }

                            Switch playerSwitch1 = (Switch)trainer1Selection;
                            Switch playerSwitch2 = (Switch)trainer2Selection;
                            await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                        }
                        else if (trainer1.GetActivePokemon().IsDead())
                        {
                            if (trainer1.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                return;
                            }
                            SendDialogueToClientRpc("Communicating...");
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                        else if (trainer2.GetActivePokemon().IsDead())
                        {
                            if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            SendDialogueToHostRpc("Communicating...");
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
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
                            UpdateGameStateRpc(GameState.BattleEnd);
                            //Give out the corresponding screen to each player
                            uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                            uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                            return;
                        }
                        SendDialogueToClientRpc("Communicating...");
                        uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                        oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                        oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                        await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                        await effectQueueController.EmptyQueue();
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadAllDialogueRpc();
                        RequestHostReadAllDialogueRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }
                        // Give a small buffer inbetween the menu change
                        //await MenuBuffer();
                        if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                        {
                            // Let attacker win
                            if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            else if (trainer1.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                return;
                            }
                            else if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestClientSwitchRpc();
                            RequestHostSwitchRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }

                            Switch playerSwitch1 = (Switch)trainer1Selection;
                            Switch playerSwitch2 = (Switch)trainer2Selection;
                            await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                        }
                        else if (!trainer2.GetActivePokemon().IsDead())
                        {
                            oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                            oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                            await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                            await effectQueueController.EmptyQueue();
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                            if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                            {
                                // Let attacker win
                                if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                    return;
                                }
                                else if (trainer1.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                    return;
                                }
                                else if (trainer2.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                    return;
                                }
                                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                                RPCManager.BeginRPCBatch();
                                RequestClientSwitchRpc();
                                RequestHostSwitchRpc();
                                while (!RPCManager.AreAllRPCsCompleted())
                                {
                                    await UniTask.Yield();
                                }

                                Switch playerSwitch1 = (Switch)trainer1Selection;
                                Switch playerSwitch2 = (Switch)trainer2Selection;
                                await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                            }
                            else if (trainer1.GetActivePokemon().IsDead())
                            {
                                if (trainer1.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                    return;
                                }
                                SendDialogueToClientRpc("Communicating...");
                                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                                uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
                            else if (trainer2.GetActivePokemon().IsDead())
                            {
                                if (trainer2.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                    return;
                                }
                                SendDialogueToHostRpc("Communicating...");
                                uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
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
                            // Force trainer2 to Switch
                            // If not, then end the battle
                            if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            // Will not await as the other player will be making a decision and do not want to get in the way of that
                            UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                            SendDialogueToHostRpc("Communicating...");
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
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
                        oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                        oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                        await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                        await effectQueueController.EmptyQueue();
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                        uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                        RPCManager.BeginRPCBatch();
                        RequestClientReadAllDialogueRpc();
                        RequestHostReadAllDialogueRpc();
                        while (!RPCManager.AreAllRPCsCompleted())
                        {
                            await UniTask.Yield();
                        }

                        if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                        {
                            // Let attacker win
                            if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            else if (trainer1.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                return;
                            }
                            else if (trainer2.IsTeamDead())
                            {
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                return;
                            }
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestClientSwitchRpc();
                            RequestHostSwitchRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }

                            Switch playerSwitch1 = (Switch)trainer1Selection;
                            Switch playerSwitch2 = (Switch)trainer2Selection;
                            await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                        }
                        else if (!trainer1.GetActivePokemon().IsDead())
                        {
                            //UpdateGameStateRpc(GameState.FirstAttack);
                            oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
                            oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
                            await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                            await effectQueueController.EmptyQueue();
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
                            RPCManager.BeginRPCBatch();
                            RequestClientReadAllDialogueRpc();
                            RequestHostReadAllDialogueRpc();
                            while (!RPCManager.AreAllRPCsCompleted())
                            {
                                await UniTask.Yield();
                            }
                            // Give a small buffer inbetween the menu change
                            if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
                            {
                                // Let attacker win
                                if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                    return;
                                }
                                else if (trainer1.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                    return;
                                }
                                else if (trainer2.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                    return;
                                }
                                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                                RPCManager.BeginRPCBatch();
                                RequestClientSwitchRpc();
                                RequestHostSwitchRpc();
                                while (!RPCManager.AreAllRPCsCompleted())
                                {
                                    await UniTask.Yield();
                                }

                                Switch playerSwitch1 = (Switch)trainer1Selection;
                                Switch playerSwitch2 = (Switch)trainer2Selection;
                                await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
                            }
                            else if (trainer2.GetActivePokemon().IsDead())
                            {
                                if (trainer2.IsTeamDead())
                                {
                                    UpdateGameStateRpc(GameState.BattleEnd);
                                    //Give out the corresponding screen to each player
                                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                                    return;
                                }
                                SendDialogueToHostRpc("Communicating...");
                                uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
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
                                UpdateGameStateRpc(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                                uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                                return;
                            }
                            SendDialogueToClientRpc("Communicating...");
                            uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
            //UpdateGameStateRpc(GameState.FirstAttack);
            oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
            oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
            await ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
            await effectQueueController.EmptyQueue();
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
            RPCManager.BeginRPCBatch();
            RequestClientReadAllDialogueRpc();
            RequestHostReadAllDialogueRpc();
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }

            if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
            {
                // Let attacker win
                if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                    return;
                }
                else if (trainer1.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                    return;
                }
                else if (trainer2.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                    return;
                }
                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientSwitchRpc();
                RequestHostSwitchRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }

                Switch playerSwitch1 = (Switch)trainer1Selection;
                Switch playerSwitch2 = (Switch)trainer2Selection;
                await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
            }
            else if (trainer1.GetActivePokemon().IsDead())
            {
                if (trainer1.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                    uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                    return;
                }
                UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                SendDialogueToClientRpc("Communicating...");
                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
            else if (trainer2.GetActivePokemon().IsDead())
            {
                if (trainer2.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                    return;
                }
                SendDialogueToHostRpc("Communicating...");
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
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
        else if (trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer2Action = (Attack)trainer2Action;
            oldHPTrainer1 = trainer1.GetActivePokemon().GetHPStat();
            oldHPTrainer2 = trainer2.GetActivePokemon().GetHPStat();
            await ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
            await effectQueueController.EmptyQueue();
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
            RPCManager.BeginRPCBatch();
            RequestClientReadAllDialogueRpc();
            RequestHostReadAllDialogueRpc();
            while (!RPCManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }

            if (trainer1.GetActivePokemon().IsDead() && trainer2.GetActivePokemon().IsDead())
            {
                // Let attacker win
                if (trainer1.IsTeamDead() && trainer2.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                    return;
                }
                else if (trainer1.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                    return;
                }
                else if (trainer2.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                    return;
                }
                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
                RPCManager.BeginRPCBatch();
                RequestClientSwitchRpc();
                RequestHostSwitchRpc();
                while (!RPCManager.AreAllRPCsCompleted())
                {
                    await UniTask.Yield();
                }

                Switch playerSwitch1 = (Switch)trainer1Selection;
                Switch playerSwitch2 = (Switch)trainer2Selection;
                await UniTask.WhenAll(ExecuteSwitch(playerSwitch1, playerSwitch1.GetTrainer(), playerSwitch1.GetPokemon()), ExecuteSwitch(playerSwitch2, playerSwitch2.GetTrainer(), playerSwitch2.GetPokemon()));
            }
            else if (trainer1.GetActivePokemon().IsDead())
            {
                if (trainer1.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 1);
                    uIController.UpdateMenuRpc(Menus.WinScreen, 2);
                    return;
                }
                UpdateGameStateRpc(GameState.WaitingOnPlayerInput);
                SendDialogueToClientRpc("Communicating...");
                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 1);
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
            else if (trainer2.GetActivePokemon().IsDead())
            {
                if (trainer2.IsTeamDead())
                {
                    UpdateGameStateRpc(GameState.BattleEnd);
                    //Give out the corresponding screen to each player
                    uIController.UpdateMenuRpc(Menus.WinScreen, 1);
                    uIController.UpdateMenuRpc(Menus.LoseScreen, 2);
                    return;
                }
                SendDialogueToHostRpc("Communicating...");
                uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
                uIController.UpdateMenuRpc(Menus.PokemonFaintedScreen, 2);
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
            //uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 1);
        }
        // Also may add a forfiet button

        // Count the number of active RPCs before exiting the function
        //RPCManager.CurrentRPCCount();
        UpdateGameStateRpc(GameState.TurnEnd);
        //await MenuBuffer();
        await effectQueueController.EmptyQueue();

        uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 1);
        uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 2);
    }
}
