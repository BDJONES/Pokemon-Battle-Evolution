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
    //public override void Awake()
    //{
    //    base.Awake();
    //    
    //}

    // Start is called before the first frame update
    private async void Start()
    {
        UIDocument uiDocument = uiController.GetComponent<UIDocument>();
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        float time = 0f;
        while (time < 3f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        UpdateGameState(GameState.BattleStart);
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        var player1Item = trainer1.activePokemon.GetItem();
        var player2Item = trainer2.activePokemon.GetItem();
        if (player1Item)
        {
            player1Item.TriggerEffect(trainer1.activePokemon);
        }
        if (player2Item)
        {
            player2Item.TriggerEffect(trainer2.activePokemon);
        }

        //var (player1Move, player2Move) = await UniTask.WhenAll(SelectMove(), SelectMove());
        //DecideWhoGoesFirst(player1Move, player2Move);

        //moveUsed.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
    }

    // Update is called once per frame
    private void Update()
    {

        // Trigger Tackle
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Debug.Log("Tackle was used");
        //    Attack move1 = trainer1.activePokemon.GetMoveset()[0];
        //    move1.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
        //    var player2Item = trainer2.activePokemon.GetItem();
        //    player2Item.RevertEffect(trainer2.activePokemon);
        //}
        //// Trigger Flamethrower
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log("Flamethrower was used");
        //    Attack move2 = trainer1.activePokemon.GetMoveset()[1];
        //    move2.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
        //}
        //// Trigger Earthquake
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Debug.Log("Earthquake was used");
        //    Attack move3 = trainer1.activePokemon.GetMoveset()[2];
        //    move3.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
        //}
        //// Trigger Thunder Wave
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Debug.Log("Thunder Wave was used");
        //    Attack move4 = trainer1.activePokemon.GetMoveset()[3];
        //    move4.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("Intimidate was used");
        //    Ability ability = trainer1.activePokemon.GetAbility();
        //    ability.TriggerEffect(trainer1.activePokemon, trainer2.activePokemon);
        //}
    }
    public void UpdateGameState(GameState newState)
    {
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
    private async UniTask<IPlayerAction> SelectMove()
    {
        float time = 0f;
        while (time < 3f)
        {
            //Debug.Log(Time.time);
            //if (pokemonButtonController)
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        var attacks = trainer1.activePokemon.GetMoveset();
        int randomMove = UnityEngine.Random.Range(0, 3);
        return attacks[randomMove];
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
                ExecuteSwitch(trainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
                ExecuteSwitch(trainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
            }
            else if (trainer1.activePokemon.GetSpeedStat() < trainer2.activePokemon.GetSpeedStat())
            {
                ExecuteSwitch(trainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
                ExecuteSwitch(trainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
            }
            else
            {
                int speedTie = UnityEngine.Random.Range(0, 99);
                if (speedTie < 50)
                {
                    ExecuteSwitch(trainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
                    ExecuteSwitch(trainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
                }
                else
                {
                    ExecuteSwitch(trainer2Action, trainer2, trainer2.pokemonTeam[1].GetComponent<Pokemon>());
                    ExecuteSwitch(trainer1Action, trainer1, trainer1.pokemonTeam[1].GetComponent<Pokemon>());
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
                convertedTrainer1Action.PerformAction(trainer1.activePokemon, trainer2.activePokemon);
                convertedTrainer2Action.PerformAction(trainer2.activePokemon, trainer1.activePokemon);
            }
            else if (convertedTrainer1Action.GetAttackPriority() <  convertedTrainer2Action.GetAttackPriority())
            {
                convertedTrainer2Action.PerformAction(trainer2.activePokemon, trainer1.activePokemon);
                convertedTrainer1Action.PerformAction(trainer1.activePokemon, trainer2.activePokemon);
            }
            else
            {
                if (trainer1.activePokemon.GetSpeedStat() > trainer2.activePokemon.GetSpeedStat())
                {
                    convertedTrainer1Action.PerformAction(trainer1.activePokemon, trainer2.activePokemon);
                    convertedTrainer2Action.PerformAction(trainer2.activePokemon, trainer1.activePokemon);
                }
                else if (trainer1.activePokemon.GetSpeedStat() < trainer2.activePokemon.GetSpeedStat())
                {
                    convertedTrainer2Action.PerformAction(trainer2.activePokemon, trainer1.activePokemon);
                    convertedTrainer1Action.PerformAction(trainer1.activePokemon, trainer2.activePokemon);
                }
                else
                {
                    int speedTie = UnityEngine.Random.Range(0, 99);
                    Debug.Log("Speed Tie");
                    if (speedTie < 50)
                    {
                        Debug.Log("Trainer 1 won the speed tie");
                        convertedTrainer1Action.PerformAction(trainer1.activePokemon, trainer2.activePokemon);
                        UpdateGameState(GameState.FirstAttack);
                        await opposingPokemonInfoBarController.UpdateHealthBar();
                        
                        convertedTrainer2Action.PerformAction(trainer2.activePokemon, trainer1.activePokemon);
                        await pokemonInfoController.UpdateHealthBar();
                        UpdateGameState(GameState.SecondAttack);
                    }
                    else
                    {
                        Debug.Log("Trainer 2 won the speed tie");
                        convertedTrainer2Action.PerformAction(trainer2.activePokemon, trainer1.activePokemon);
                        await pokemonInfoController.UpdateHealthBar();
                        UpdateGameState(GameState.FirstAttack);
                        convertedTrainer1Action.PerformAction(trainer1.activePokemon, trainer2.activePokemon);
                        await opposingPokemonInfoBarController.UpdateHealthBar();
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
