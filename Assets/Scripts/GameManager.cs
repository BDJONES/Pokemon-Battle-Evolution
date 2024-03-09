using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TrainerController trainer1Controller;
    [SerializeField] private TrainerController trainer2Controller;
    private Trainer trainer1;
    private Trainer trainer2;
    private UIController trainer1UIController;
    private UIController trainer2UIController;
    [SerializeField] private GameState gameState;
    public static event Action<GameState> OnStateChange;
    [SerializeField] private GameObject uiController;
    [SerializeField] private PokemonInfoController pokemonInfoController;
    [SerializeField] private OpposingPokemonInfoController opposingPokemonInfoBarController;
    private LobbyManager lobbyManager;
    private bool player1Inactive = false;
    private bool player2Inactive = false;

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
        trainer1Controller = trainerController;
        trainer1 = trainer1Controller.GetPlayer();
    }

    private void SetTrainer2Controller(TrainerController trainerController)
    {
        trainer2Controller = trainerController;
        trainer2 = trainer2Controller.GetPlayer();
    }

    private void SetPokemonInfoController(GameObject player)
    {

        Transform sharedControllers = player.transform.Find("SharedControllers");
        pokemonInfoController = sharedControllers.GetComponent<PokemonInfoController>();
        opposingPokemonInfoBarController = sharedControllers.GetComponent<OpposingPokemonInfoController>();
    }

    public void PopulateGameManager(GameObject gameObject, int type)
    {
        TrainerController trainerController = gameObject.GetComponent<TrainerController>();
        if (type == 1)
        {
            SetTrainer1Controller(trainerController);
            trainer1UIController = gameObject.transform.Find("UIController").GetComponent<UIController>();
        }
        else
        {
            SetTrainer2Controller(trainerController);
            trainer2UIController = gameObject.transform.Find("UIController").GetComponent<UIController>();
        }
        //Set Location of the gameobjects
        SetPokemonInfoController(gameObject);
    }

    private void Start()
    {
        //trainer1 = trainer1Controller.GetPlayer();
        //trainer2 = trainer2Controller.GetPlayer();
        //trainer1UIController = trainer1Controller.gameObject.GetComponentInChildren<UIController>();

        LobbyManager.TwoPlayersConnected += HandlePlayerConnection;
    }

    private void HandlePlayerConnection()
    {
        // if two players have connected then we would like to start the game
        StartGame();
    }

    private async void StartGame()
    {        
        //UIDocument uiDocument = uiController.GetComponent<UIDocument>();
        //uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        float time = 0f;
        while (time < 3f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }        
        trainer1Controller.SetOpponent(trainer2);
        trainer2Controller.SetOpponent(trainer1);
        UpdateGameState(GameState.LoadingPokemonInfo);
        time = 0f;
        while (time < 2f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        UpdateGameState(GameState.BattleStart);
        TimeManager.MatchTimerEnd += HandleMatchTimeout;
        trainer1Controller.playerTooInactive += Trainer1Inactive;
        trainer1Controller.playerTooInactive += Trainer2Inactive;
        //uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        TurnSystem();
    }

    private void Trainer1Inactive()
    {
        player1Inactive = true;
    }

    private void Trainer2Inactive()
    {
        player2Inactive = true;
    }

    private async UniTask screenBuffer()
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private async void TurnSystem()
    {
        //float time = 0f;
        while (gameState != GameState.BattleEnd)
        {
            UpdateGameState(GameState.TurnStart);
            //while (time < 3f)
            //{
            //    time += Time.deltaTime;
            //    await UniTask.Yield();
            //}
            //time = 0f;
            //UpdateGameState(GameState.WaitingOnPlayerInput);
            var (player1Move, player2Move) = await UniTask.WhenAll(trainer1Controller.SelectMove(), trainer2Controller.SelectMove());
            
            // Need to add for a case where both players have gone inactive
            if (player1Inactive)
            {
                UpdateGameState(GameState.BattleEnd);
                break;
            }
            else if (player2Inactive)
            {
                UpdateGameState(GameState.BattleEnd);
                break;
            }
            UpdateGameState(GameState.ProcessingInput);
            await DecideWhoGoesFirst(player1Move, player2Move);
            UpdateGameState(GameState.TurnEnd);
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
            //Debug.Log("I switched pokemon");
            // May add a history here later
        }
    }

    private async UniTask DecideWhoGoesFirst(IPlayerAction trainer1Action, IPlayerAction trainer2Action)
    {
        if (trainer1Action.GetType() == typeof(Switch) && trainer2Action.GetType() == typeof(Switch))
        {
            Debug.Log("Both players switched");
            Switch convertedTrainer1Action = (Switch)trainer1Action;
            Switch convertedTrainer2Action = (Switch)trainer2Action;
            if (trainer1.GetActivePokemon().GetSpeedStat() > trainer2.GetActivePokemon().GetSpeedStat())
            {
                ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
            }
            else if (trainer1.GetActivePokemon().GetSpeedStat() < trainer2.GetActivePokemon().GetSpeedStat())
            {
                ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
            }
            else
            {
                int speedTie = UnityEngine.Random.Range(0, 99);
                if (speedTie < 50)
                {
                    ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                    ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                }
                else
                {
                    ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
                    ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
                }
            }
        }
        else if (trainer1Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer1Action = (Switch)trainer1Action;
            ExecuteSwitch(convertedTrainer1Action, convertedTrainer1Action.GetTrainer(), convertedTrainer1Action.GetPokemon());
        }            
        else if (trainer2Action.GetType() == typeof(Switch))
        {
            Switch convertedTrainer2Action = (Switch)trainer2Action;
            ExecuteSwitch(convertedTrainer2Action, convertedTrainer2Action.GetTrainer(), convertedTrainer2Action.GetPokemon());
        }

        if (trainer1Action.GetType().IsSubclassOf(typeof(Attack)) && trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Debug.Log("Both were Attacks");
            Attack convertedTrainer1Action = (Attack)trainer1Action;
            Attack convertedTrainer2Action = (Attack)trainer2Action;
            if (convertedTrainer1Action.GetAttackPriority() > convertedTrainer2Action.GetAttackPriority())
            {
                ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
            }
            else if (convertedTrainer1Action.GetAttackPriority() <  convertedTrainer2Action.GetAttackPriority())
            {
                ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
            }
            else
            {
                if (trainer1.GetActivePokemon().GetSpeedStat() > trainer2.GetActivePokemon().GetSpeedStat())
                {
                    ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                    ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                }
                else if (trainer1.GetActivePokemon().GetSpeedStat() < trainer2.GetActivePokemon().GetSpeedStat())
                {
                    ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                    ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                }
                else
                {
                    int speedTie = UnityEngine.Random.Range(0, 99);
                    Debug.Log("Speed Tie");
                    if (speedTie < 50)
                    {
                        Debug.Log("Trainer 1 won the speed tie");
                        trainer1UIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen);
                        ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                        //UpdateGameState(GameState.FirstAttack);
                        await opposingPokemonInfoBarController.UpdateHealthBar(Menus.OpposingPokemonDamagedScreen);
                        // Give a small buffer inbetween the menu change
                        await screenBuffer();

                        if (!trainer2.GetActivePokemon().IsDead())
                        {
                            trainer1UIController.UpdateMenu(Menus.PokemonDamagedScreen);
                            ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                            await pokemonInfoController.UpdateHealthBar(Menus.PokemonDamagedScreen);
                            //UpdateGameState(GameState.SecondAttack);
                            // Give a small buffer inbetween the menu change
                            await screenBuffer();
                            //UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);

                            // Add an extra if to trigger the same logic if trainer1 dies afterward
                            if (trainer1.GetActivePokemon().IsDead())
                            {
                                trainer1UIController.UpdateMenu(Menus.PokemonFaintedScreen);
                                UpdateGameState(GameState.WaitingOnPlayerInput);
                                var action = await trainer1Controller.SwitchOutFaintedPokemon();
                                //Debug.Log(action.GetType());
                                //Debug.Log("Something");
                                Switch playerSwitch = (Switch)action;
                                Debug.Log($"Name = {playerSwitch.GetTrainer().trainerName}, Pokemon = {playerSwitch.GetPokemon().GetSpeciesName()}");
                                ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                                Debug.Log("Just about to change to the general battle menu");
                                trainer1UIController.UpdateMenu(Menus.GeneralBattleMenu);
                                //await UniTask.WaitForSeconds(1);
                            }
                            trainer1UIController.UpdateMenu(Menus.GeneralBattleMenu);
                        }
                        else
                        {
                            // Force trainer2 to Switch
                            // If not, then end the battle
                            if (trainer2.isTeamDead())
                            {
                                UpdateGameState(GameState.BattleEnd);
                                //Give out the corresponding screen to each player
                                //UIController.Instance.UpdateMenu(Menus.VictoryScreen);
                                return;
                            }


                            //UIController.Instance.UpdateMenu(Menus.PokemonFaintedScreen);
                            //var playerSwitch = (Switch)await trainer2Controller.SelectMove();
                            //ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                            trainer1UIController.UpdateMenu(Menus.GeneralBattleMenu);
                        }
                    }
                    else
                    {
                        Debug.Log("Trainer 2 won the speed tie");
                        trainer1UIController.UpdateMenu(Menus.PokemonDamagedScreen);
                        ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                        await pokemonInfoController.UpdateHealthBar(Menus.PokemonDamagedScreen);
                        // Give a small buffer inbetween the menu change
                        await screenBuffer();
                        if (!trainer1.GetActivePokemon().IsDead())
                        {
                            //UpdateGameState(GameState.FirstAttack);
                            trainer1UIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen);
                            ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                            //
                            //UpdateGameState(GameState.SecondAttack);
                            await opposingPokemonInfoBarController.UpdateHealthBar(Menus.OpposingPokemonDamagedScreen);
                            // Give a small buffer inbetween the menu change
                            await screenBuffer();
                            trainer1UIController.UpdateMenu(Menus.GeneralBattleMenu);
                        }
                        else
                        {
                            trainer1UIController.UpdateMenu(Menus.PokemonFaintedScreen);
                            UpdateGameState(GameState.WaitingOnPlayerInput);
                            var action = await trainer1Controller.SwitchOutFaintedPokemon();
                            Debug.Log(action.GetType());
                            Debug.Log("Something");
                            Switch playerSwitch = action;
                            Debug.Log($"Name = {playerSwitch.GetTrainer().trainerName}, Pokemon = {playerSwitch.GetPokemon().GetSpeciesName()}");
                            ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                            trainer1UIController.UpdateMenu(Menus.GeneralBattleMenu);
                            await UniTask.WaitForSeconds(1);
                        }
                    }
                }
            }
            
        }
        else if (trainer1Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer1Action = (Attack)trainer1Action;
            trainer1UIController.UpdateMenu(Menus.OpposingPokemonDamagedScreen);
            convertedTrainer1Action.PerformAction(trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
            ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
            await pokemonInfoController.UpdateHealthBar(Menus.OpposingPokemonDamagedScreen);
            trainer1UIController.UpdateMenu(Menus.GeneralBattleMenu);
        }
        else if (trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer2Action = (Attack)trainer2Action;
            trainer1UIController.UpdateMenu(Menus.PokemonDamagedScreen);
            ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
            await pokemonInfoController.UpdateHealthBar(Menus.PokemonDamagedScreen);
            trainer1UIController.UpdateMenu(Menus.GeneralBattleMenu);
        }

        // Also may add a forfiet button
    }
}
