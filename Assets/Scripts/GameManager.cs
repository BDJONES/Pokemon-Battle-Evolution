using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    public TrainerController trainer1Controller;
    public TrainerController trainer2Controller;
    public Trainer trainer1;
    public Trainer trainer2;
    [SerializeField] private GameState gameState;
    public static event Action<GameState> OnStateChange;
    [SerializeField] private GameObject uiController;
    [SerializeField] private PokemonInfoController pokemonInfoController;
    [SerializeField] private OpposingPokemonInfoBarController opposingPokemonInfoBarController;
    [SerializeField] private PokemonButtonController pokemonButtonController;
    

    //private void OnEnable()
    //{
        
    //}



    private async void Start()
    {
        trainer1 = trainer1Controller.GetPlayer();
        trainer2 = trainer2Controller.GetPlayer();
        UIDocument uiDocument = uiController.GetComponent<UIDocument>();
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        float time = 0f;
        while (time < 3f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        UpdateGameState(GameState.LoadingPokemonInfo);
        time = 0f;
        while (time < 2f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        UpdateGameState(GameState.BattleStart);
        TimeManager.MatchTimerEnd += HandleMatchTimeout;
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        TurnSystem();
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
            UpdateGameState(GameState.ProcessingInput);
            await DecideWhoGoesFirst(player1Move, player2Move);
            UpdateGameState(GameState.TurnEnd);
        }
    }
    public void UpdateGameState(GameState newState)
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
            OnStateChange.Invoke(newState);
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
                        UIController.Instance.UpdateMenu(Menus.OpposingPokemonDamagedScreen);
                        ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                        //UpdateGameState(GameState.FirstAttack);
                        await opposingPokemonInfoBarController.UpdateHealthBar(Menus.OpposingPokemonDamagedScreen);

                        if (!trainer2.GetActivePokemon().IsDead())
                        {
                            UIController.Instance.UpdateMenu(Menus.PokemonDamagedScreen);
                            ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                            await pokemonInfoController.UpdateHealthBar(Menus.PokemonDamagedScreen);
                            //UpdateGameState(GameState.SecondAttack);
                            //UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);

                            // Add an extra if to trigger the same logic if trainer1 dies afterward
                            if (trainer1.GetActivePokemon().IsDead())
                            {
                                UIController.Instance.UpdateMenu(Menus.PokemonFaintedScreen);
                                UpdateGameState(GameState.WaitingOnPlayerInput);
                                var action = await trainer1Controller.SwitchOutFaintedPokemon();
                                //Debug.Log(action.GetType());
                                //Debug.Log("Something");
                                Switch playerSwitch = (Switch)action;
                                Debug.Log($"Name = {playerSwitch.GetTrainer().trainerName}, Pokemon = {playerSwitch.GetPokemon().GetSpeciesName()}");
                                ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                                Debug.Log("Just about to change to the general battle menu");
                                UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
                                //await UniTask.WaitForSeconds(1);
                            }
                        }
                        //else
                        //{
                        //    UIController.Instance.UpdateMenu(Menus.PokemonFaintedScreen);
                        //    var playerSwitch = (Switch) await trainer2Controller.SelectMove();
                        //    ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                        //}
                    }
                    else
                    {
                        Debug.Log("Trainer 2 won the speed tie");
                        UIController.Instance.UpdateMenu(Menus.PokemonDamagedScreen);
                        ExecuteAttack(convertedTrainer2Action, trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
                        await pokemonInfoController.UpdateHealthBar(Menus.PokemonDamagedScreen);

                        if (!trainer1.GetActivePokemon().IsDead())
                        {
                            //UpdateGameState(GameState.FirstAttack);
                            UIController.Instance.UpdateMenu(Menus.OpposingPokemonDamagedScreen);
                            ExecuteAttack(convertedTrainer1Action, trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
                            //
                            //UpdateGameState(GameState.SecondAttack);
                            await opposingPokemonInfoBarController.UpdateHealthBar(Menus.OpposingPokemonDamagedScreen);
                            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
                        }
                        else
                        {
                            UIController.Instance.UpdateMenu(Menus.PokemonFaintedScreen);
                            UpdateGameState(GameState.WaitingOnPlayerInput);
                            var action = await trainer1Controller.SwitchOutFaintedPokemon();
                            Debug.Log(action.GetType());
                            Debug.Log("Something");
                            Switch playerSwitch = (Switch) action;
                            Debug.Log($"Name = {playerSwitch.GetTrainer().trainerName}, Pokemon = {playerSwitch.GetPokemon().GetSpeciesName()}");
                            ExecuteSwitch(playerSwitch, playerSwitch.GetTrainer(), playerSwitch.GetPokemon());
                            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
                            await UniTask.WaitForSeconds(1);
                        }
                    }
                }
            }
            
        }
        else if (trainer1Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer1Action = (Attack)trainer1Action;
            convertedTrainer1Action.PerformAction(trainer1.GetActivePokemon(), trainer2.GetActivePokemon());
        }
        else if (trainer2Action.GetType().IsSubclassOf(typeof(Attack)))
        {
            Attack convertedTrainer2Action = (Attack)trainer2Action;

            convertedTrainer2Action.PerformAction(trainer2.GetActivePokemon(), trainer1.GetActivePokemon());
        }

        // Also may add a forfiet button
    }
}
