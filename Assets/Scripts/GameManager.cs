using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    public Trainer trainer1;
    public Trainer trainer2;
    [SerializeField] private GameState gameState;
    public static event Action<GameState> OnStateChange;
    [SerializeField] private GameObject uiController;
    [SerializeField] private PokemonInfoController pokemonInfoController;
    [SerializeField] private OpposingPokemonInfoBarController opposingPokemonInfoBarController;
    [SerializeField] private PokemonButtonController pokemonButtonController;
    private IPlayerAction selectedAction;
<<<<<<< Updated upstream
    UIInputGrabber uIGrabber;
=======
<<<<<<< Updated upstream
>>>>>>> Stashed changes
    //public override void Awake()
    //{
    //    base.Awake();
    //    
    //}
=======
    UIInputGrabber uIGrabber;
>>>>>>> Stashed changes

    private void OnEnable()
    {
        TimeManager.MatchTimerEnd += HandleMatchTimeout;
        //TimeManager.TurnTimerEnd += HandleTurnTimeout;
    }



    private async void Start()
    {
        UIDocument uiDocument = uiController.GetComponent<UIDocument>();
        uIGrabber = new UIInputGrabber();
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        float time = 0f;
        
        while (time < 3f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        time = 0f;
        UpdateGameState(GameState.LoadingPokemonInfo);
        while (time < 2f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        UpdateGameState(GameState.BattleStart);
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
<<<<<<< Updated upstream
        TurnSystem();
=======
<<<<<<< Updated upstream
        TimeManager.Instance.StartMatchTimer();
        var player1Item = trainer1.activePokemon.GetItem();
        var player2Item = trainer2.activePokemon.GetItem();
        if (player1Item)
=======
        TurnSystem();
    }

    // Update is called once per frame
    private async void TurnSystem()
    {
        while (gameState != GameState.BattleEnd)
>>>>>>> Stashed changes
        {
            UpdateGameState(GameState.TurnStart);
            //var player1Item = trainer1.activePokemon.GetItem();
            //var player2Item = trainer2.activePokemon.GetItem();
            //if (player1Item != null)
            //{
            //    player1Item.TriggerEffect(trainer1.activePokemon);
            //}
            //if (player2Item != null)
            //{
            //    player2Item.TriggerEffect(trainer2.activePokemon);
            //}
        
            UpdateGameState(GameState.WaitingOnPlayerInput);
            UpdateGameState(GameState.ProcessingInput);
            var (player1Move, player2Move) = await UniTask.WhenAll(SelectMove(), SelectMove());
            DecideWhoGoesFirst(player1Move, player2Move);
            UpdateGameState(GameState.TurnEnd);
        // if Battle over end loop
        }
<<<<<<< Updated upstream
        if (player2Item)
        {
            player2Item.TriggerEffect(trainer2.activePokemon);
        }

        var (player1Move, player2Move) = await UniTask.WhenAll(SelectMove(), SelectMove());
        DecideWhoGoesFirst(player1Move, player2Move);
=======
        
>>>>>>> Stashed changes
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    private async void TurnSystem()
    {
        var player1Item = trainer1.activePokemon.GetItem();
        var player2Item = trainer2.activePokemon.GetItem();
        if (player1Item != null)
        {
            player1Item.TriggerEffect(trainer1.activePokemon);
        }
        if (player2Item != null)
        {
            player2Item.TriggerEffect(trainer2.activePokemon);
        }
        UpdateGameState(GameState.TurnStart);
        var (player1Move, player2Move) = await UniTask.WhenAll(SelectMove(), SelectMove());
        DecideWhoGoesFirst(player1Move, player2Move);
    }
    public void UpdateGameState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {
            case GameState.BattleStart:
                TimeManager.Instance.StartMatchTimer();
                break;
            case GameState.TurnStart: 
                TimeManager.Instance.StartTurnTimer();
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
            OnStateChange.Invoke(newState);
        }
    }
    
    private void HandleMatchTimeout()
    {
        UpdateGameState(GameState.BattleEnd);
    }    
    private void HandleTurnTimeout()
    {
        UpdateGameState(GameState.TurnEnd);
    }

    private async UniTask<IPlayerAction> SelectMove()
    {
        var receiver = GameObject.Find("AttackSelectionControllers").GetComponent<MoveSelectButton>();
        
        IPlayerAction selection = null;

        Action<IPlayerAction> anonFunc = (input) =>
        {
            // Invoke the handler and set value of selection to returned Action
            selection = uIGrabber.HandleInput(input);
            return;
        };
<<<<<<< Updated upstream

        MoveSelectButton.InputReceived += anonFunc;
        PartyPokemonController.InputReceived += anonFunc;
        while (selection == null)
=======
<<<<<<< Updated upstream
=======

        MoveSelectButton.InputReceived += anonFunc;
        PartyPokemonController.InputReceived += anonFunc;
>>>>>>> Stashed changes
        while (selection == null && TimeManager.Instance.IsTurnTimerActive())
>>>>>>> Stashed changes
        {
            Debug.Log("In The Loop");
            await UniTask.Yield();
        }
<<<<<<< Updated upstream
        MoveSelectButton.InputReceived -= anonFunc;
        PartyPokemonController.InputReceived -= anonFunc;
        return selection;

        //var attacks = trainer1.activePokemon.GetMoveset();
        //int randomMove = UnityEngine.Random.Range(0, 3);
        //return attacks[randomMove];
=======
<<<<<<< Updated upstream
        // If a move was selected return that value
=======
        MoveSelectButton.InputReceived -= anonFunc;
        PartyPokemonController.InputReceived -= anonFunc;
>>>>>>> Stashed changes
        if (selection != null)
        {
            return selection;
        }
<<<<<<< Updated upstream
        // Otherwise, perform a random move
        var attacks = trainer1.activePokemon.GetMoveset();
        int randomMove = UnityEngine.Random.Range(0, 3);
        return attacks[randomMove];
=======
        else
        {
            Debug.Log("Time out of the turn. Will select a random move.");
            var attacks = trainer1.activePokemon.GetMoveset();
            int randomMove = UnityEngine.Random.Range(0, 3);
            return attacks[randomMove];
        }
>>>>>>> Stashed changes
>>>>>>> Stashed changes
    }

    private void ExecuteAttack(IPlayerAction playerAction, Pokemon attacker, Pokemon target)
    {
        
        if (playerAction.PerformAction(attacker, target) != null)
        {
            // May add a history later
        }
    }

    private void ExecuteSwitch(IPlayerAction playerAction, Trainer trainer, Pokemon newPokemon)
    {
        if (playerAction.PerformAction(trainer, newPokemon) != null)
        {
            // May add a history here later
        }
    }

    private async void DecideWhoGoesFirst(IPlayerAction trainer1Action, IPlayerAction trainer2Action)
    {
        if (trainer1Action.GetType() == typeof(Switch) && trainer2Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer1Action = (Switch)trainer1Action;
            Switch convertedTrainer2Action = (Switch)trainer2Action;
            if (trainer1.activePokemon.GetSpeedStat() > trainer2.activePokemon.GetSpeedStat())
            {
                ExecuteSwitch(convertedTrainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
                ExecuteSwitch(convertedTrainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
            }
            else if (trainer1.activePokemon.GetSpeedStat() < trainer2.activePokemon.GetSpeedStat())
            {
                ExecuteSwitch(convertedTrainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
                ExecuteSwitch(convertedTrainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
            }
            else
            {
                int speedTie = UnityEngine.Random.Range(0, 99);
                if (speedTie < 50)
                {
                    ExecuteSwitch(convertedTrainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
                    ExecuteSwitch(convertedTrainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
                }
                else
                {
                    ExecuteSwitch(convertedTrainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
                    ExecuteSwitch(convertedTrainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
                }
            }
        }
        else if (trainer1Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer1Action = (Switch)trainer1Action;
            ExecuteSwitch(convertedTrainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
        }            
        else if (trainer2Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer2Action = (Switch)trainer2Action;
            ExecuteSwitch(convertedTrainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
        }

        if (trainer1Action.GetType().IsSubclassOf(typeof(Attack)) && trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Debug.Log("Both were Attacks");
            Attack convertedTrainer1Action = (Attack)trainer1Action;
            Attack convertedTrainer2Action = (Attack)trainer2Action;
            if (convertedTrainer1Action.GetAttackPriority() > convertedTrainer2Action.GetAttackPriority())
            {
                ExecuteAttack(convertedTrainer1Action, trainer1.activePokemon, trainer2.activePokemon);
                ExecuteAttack(convertedTrainer2Action, trainer2.activePokemon, trainer1.activePokemon);
            }
            else if (convertedTrainer1Action.GetAttackPriority() <  convertedTrainer2Action.GetAttackPriority())
            {
                ExecuteAttack(convertedTrainer2Action, trainer2.activePokemon, trainer1.activePokemon);
                ExecuteAttack(convertedTrainer1Action, trainer1.activePokemon, trainer2.activePokemon);
            }
            else
            {
                if (trainer1.activePokemon.GetSpeedStat() > trainer2.activePokemon.GetSpeedStat())
                {
                    ExecuteAttack(convertedTrainer1Action, trainer1.activePokemon, trainer2.activePokemon);
                    ExecuteAttack(convertedTrainer2Action, trainer2.activePokemon, trainer1.activePokemon);
                }
                else if (trainer1.activePokemon.GetSpeedStat() < trainer2.activePokemon.GetSpeedStat())
                {
                    ExecuteAttack(convertedTrainer2Action, trainer2.activePokemon, trainer1.activePokemon);
                    ExecuteAttack(convertedTrainer1Action, trainer1.activePokemon, trainer2.activePokemon);
                }
                else
                {
                    int speedTie = UnityEngine.Random.Range(0, 99);
                    Debug.Log("Speed Tie");
                    if (speedTie < 50)
                    {
                        Debug.Log("Trainer 1 won the speed tie");
                        ExecuteAttack(convertedTrainer1Action, trainer1.activePokemon, trainer2.activePokemon);
                        UpdateGameState(GameState.FirstAttack);
                        UIController.Instance.UpdateMenu(Menus.OpposingPokemonDamagedScreen);
                        await opposingPokemonInfoBarController.UpdateHealthBar(Menus.GeneralBattleMenu);
                        ExecuteAttack(convertedTrainer2Action, trainer2.activePokemon, trainer1.activePokemon);
                        UIController.Instance.UpdateMenu(Menus.PokemonDamagedScreen);
                        await pokemonInfoController.UpdateHealthBar(Menus.GeneralBattleMenu);
                        UpdateGameState(GameState.SecondAttack);
                    }
                    else
                    {
                        Debug.Log("Trainer 2 won the speed tie");
                        ExecuteAttack(convertedTrainer2Action, trainer2.activePokemon, trainer1.activePokemon);
                        UIController.Instance.UpdateMenu(Menus.PokemonDamagedScreen);
                        await pokemonInfoController.UpdateHealthBar(Menus.GeneralBattleMenu);
                        UpdateGameState(GameState.FirstAttack);
                        ExecuteAttack(convertedTrainer1Action, trainer1.activePokemon, trainer2.activePokemon);
                        UIController.Instance.UpdateMenu(Menus.OpposingPokemonDamagedScreen);
                        await opposingPokemonInfoBarController.UpdateHealthBar(Menus.GeneralBattleMenu);
                        UpdateGameState(GameState.SecondAttack);
                    }
                }
            }
            
        }
        else if (trainer1Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer1Action = (Attack)trainer1Action;
            convertedTrainer1Action.PerformAction(trainer1.activePokemon, trainer2.activePokemon);
        }
        else if (trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer2Action = (Attack)trainer1Action;
            convertedTrainer2Action.PerformAction(trainer2.activePokemon, trainer1.activePokemon);
        }

        // Also may add a forfiet button
    }
}
