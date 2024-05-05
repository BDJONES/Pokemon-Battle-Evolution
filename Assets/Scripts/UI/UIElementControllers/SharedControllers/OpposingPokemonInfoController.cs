using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class OpposingPokemonInfoController : NetworkBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] private GeneralBattleUIElements uIGBElements;
    [SerializeField] private MoveSelectionUIElements moveSelectionUIElements;
    [SerializeField] private OpposingPokemonDamagedUIElements opposingPokemonDamagedUIElements;
    [SerializeField] private PokemonInfoUIElements pokemonInfoUIElements;
    private Label pokemonNameLabelGB;
    private Label pokemonLevelLabelGB;
    private ProgressBar hpBarGB;
    private Button infoButtonGB;
    private Label pokemonNameLabelMS;
    private Label pokemonLevelLabelMS;
    private ProgressBar hpBarMS;
    private Button infoButtonMS;
    private Label pokemonNameLabelOD;
    private Label pokemonLevelLabelOD;
    private ProgressBar hpBarOD;
    private Button infoButtonOD;
    private Label pokemonNameLabelPI;
    private Label pokemonLevelLabelPI;
    private ProgressBar hpBarPI;
    private Label hpStatLabelPI;
    private UIController uIController;
    private float? oldHP;
    private void OnEnable()
    {
        //if (IsOwner)
        //{   
        NetworkCommands.UIControllerCreated += () =>
        {
            uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
            uIGBElements = uIController.GetComponent<GeneralBattleUIElements>();
            moveSelectionUIElements = uIController.GetComponent<MoveSelectionUIElements>();
            opposingPokemonDamagedUIElements = uIController.GetComponent<OpposingPokemonDamagedUIElements>();
            pokemonInfoUIElements = uIController.GetComponent<PokemonInfoUIElements>();
            trainerController = transform.parent.parent.gameObject.GetComponent<TrainerController>();
            //if (trainerController.GetPlayer())
            //{
            //    Debug.Log("The player exist");
            //}
            //else
            //{
            //    Debug.Log("The player does not exist");
            //}
            //if (trainerController.GetOpponent())
            //{
            //    Debug.Log($"GameObject = {transform.parent.parent.gameObject.name}, The opponent exist");
            //}
            //else
            //{
            //    Debug.Log($"GameObject = {transform.parent.parent.gameObject.name}, The opponent doesn't exist");
            //}
            uIController.OnHostMenuChange += HandleMenuChange;
            uIController.OnClientMenuChange += HandleMenuChange;
            GameManager.OnStateChange += HandleGameStateChange;
        };
        //}
    }

    private void OnDisable()
    {
        if (uIController != null)
        {
            uIController.OnHostMenuChange -= HandleMenuChange;
            uIController.OnClientMenuChange -= HandleMenuChange;
            GameManager.OnStateChange -= HandleGameStateChange;
        }
    }

    private void InitializeFields(Menus menu)
    {
        VisualElement pokemonInfoVE;
        VisualElement battleInfoVE;
        VisualElement nameAndGenderVE;
        VisualElement levelInfoVE;
        VisualElement hpBarVE;
        if (menu == Menus.GeneralBattleMenu)
        {
            // General Battle Section
            pokemonInfoVE = uIGBElements.OpposingPokemonInfoBar.Query<VisualElement>("OpposingPokemonInfo");
            battleInfoVE = pokemonInfoVE.Query<VisualElement>("Battle_Info");
            nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
            levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
            hpBarVE = battleInfoVE.Query<VisualElement>("HPBar");
        
            pokemonNameLabelGB = nameAndGenderVE.Query<Label>("Name");
            pokemonLevelLabelGB = levelInfoVE.Query<Label>("Level");
            hpBarGB = hpBarVE.Query<ProgressBar>("ProgressBar");
            infoButtonGB = uIGBElements.OpposingPokemonInfoBar.Query<Button>("OpposingPokemonInfoButton");
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            // Move Selection Section
            pokemonInfoVE = moveSelectionUIElements.OpposingPokemonInfoBar.Query<VisualElement>("OpposingPokemonInfo");
            battleInfoVE = pokemonInfoVE.Query<VisualElement>("Battle_Info");
            nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
            levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
            hpBarVE = battleInfoVE.Query<VisualElement>("HPBar");

            pokemonNameLabelMS = nameAndGenderVE.Query<Label>("Name");
            pokemonLevelLabelMS = levelInfoVE.Query<Label>("Level");
            hpBarMS = hpBarVE.Query<ProgressBar>("ProgressBar");
            infoButtonMS = uIGBElements.OpposingPokemonInfoBar.Query<Button>("OpposingPokemonInfoButton");
        }
        else if (menu == Menus.OpposingPokemonDamagedScreen)
        {
            // Oppoing Pokemon Damaged Screen
            pokemonInfoVE = opposingPokemonDamagedUIElements.OpposingPokemonInfoBar.Query<VisualElement>("OpposingPokemonInfo");
            battleInfoVE = pokemonInfoVE.Query<VisualElement>("Battle_Info");
            nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
            levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
            hpBarVE = battleInfoVE.Query<VisualElement>("HPBar");

            pokemonNameLabelOD = nameAndGenderVE.Query<Label>("Name");
            pokemonLevelLabelOD = levelInfoVE.Query<Label>("Level");
            hpBarOD = hpBarVE.Query<ProgressBar>("ProgressBar");
            infoButtonOD = battleInfoVE.Query<Button>("OpposingPokemonInfoButton");
        }
        else if (menu == Menus.OpposingPokemonInfoScreen)
        {
            nameAndGenderVE = pokemonInfoUIElements.PokemonInfo.Query<VisualElement>("Name_And_Gender");
            levelInfoVE = pokemonInfoUIElements.PokemonInfo.Query<VisualElement>("LevelInfo");
            hpBarVE = pokemonInfoUIElements.PokemonInfo.Query<VisualElement>("HPInfo");

            pokemonNameLabelPI = nameAndGenderVE.Query<Label>("Name");
            pokemonLevelLabelPI = levelInfoVE.Query<Label>("Level");
            hpBarPI = hpBarVE.Query<ProgressBar>();
            hpStatLabelPI = hpBarVE.Query<Label>("HPStat");
        }
    }

    private void Start()
    {

    }

    public IEnumerator DrainHP(Menus menu, int oldHPValue)
    {
        ProgressBar hpBar = null!;
        if (menu == Menus.GeneralBattleMenu)
        {
            hpBar = hpBarGB;
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            hpBar = hpBarMS;
        }
        else if (menu == Menus.OpposingPokemonDamagedScreen)
        {
            hpBar = hpBarOD;
        }
        else
        {
            GameManager.Instance.FinishRPCTaskRpc();
            yield return null;
        }
        float newHPValue = trainerController.GetOpponent().GetActivePokemon().GetHPStat();
        if (oldHPValue > newHPValue)
        {
            while (hpBar.value > newHPValue)
            {
                //hpBar.schedule.Execute(() =>
                //{
                    hpBar.value -= 1f;
                //}).Every(30).Until(() => hpBar.value <= newHPValue);
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
        else
        {
            while (hpBar.value < newHPValue)
            {
                //hpBar.schedule.Execute(() =>
                //{
                    hpBar.value += 1f;
                //}).Every(50).Until(() => hpBar.value >= newHPValue);
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
        oldHP = newHPValue;
        if (newHPValue == 0)
        {
            oldHP = null;
            YourPokemonDeathEventManager.AlertOfDeath();
        }
        GameManager.Instance.FinishRPCTaskRpc();
        yield return null;
    }

    public void HandleGameStateChange(GameState state)
    {
        //if (state == GameState.LoadingPokemonInfo)
        //{
        //    
        //}
        //if (state == GameState.BattleStart)
        //{
        //    InitializeFields();
        //    UpdateInfo(Menus.GeneralBattleMenu);
        //}
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu || menu == Menus.MoveSelectionMenu || menu == Menus.OpposingPokemonDamagedScreen || menu == Menus.OpposingPokemonInfoScreen)
        {
            InitializeFields(menu);
            if (menu == Menus.GeneralBattleMenu && infoButtonGB != null)
            {
                var player = transform.parent.parent.gameObject;

                if (IsHost)
                {
                    UIEventSubscriptionManager.Subscribe(infoButtonGB, ClickedInfoButton, 1);
                }
                else
                {
                    Debug.Log("Subscribing to the info button on General Battle Menu");
                    UIEventSubscriptionManager.Subscribe(infoButtonGB, ClickedInfoButton, 2);
                }
            }
            else if (menu == Menus.GeneralBattleMenu && infoButtonGB == null)
            {
                Debug.Log("Info Button was null");
            }

            if (menu == Menus.MoveSelectionMenu && infoButtonMS != null)
            {
                var player = transform.parent.parent.gameObject;

                if (IsHost)
                {
                    UIEventSubscriptionManager.Subscribe(infoButtonMS, ClickedInfoButton, 1);
                }
                else
                {
                    Debug.Log("Subscribing to the info button on MoveSelection");
                    UIEventSubscriptionManager.Subscribe(infoButtonMS, ClickedInfoButton, 2);
                }
            }
            else if (menu == Menus.MoveSelectionMenu && infoButtonMS == null)
            {
                Debug.Log("Info Button was null");
            }
            UpdateInfo(menu);
        }
    }

    private void ClickedInfoButton()
    {
        Debug.Log("Clicked the opposing pokemon info button");
        var player = transform.parent.parent.gameObject;

        if (IsHost)
        {
            uIController.UpdateMenuRpc(Menus.OpposingPokemonInfoScreen, 1);
        }
        else
        {
            uIController.UpdateMenuRpc(Menus.OpposingPokemonInfoScreen, 2);
        }
    }

    private void UpdateInfo(Menus menu)
    {
        if (!IsOwner) return;
        trainerController = transform.parent.parent.gameObject.GetComponent<TrainerController>();
        Debug.Log($"trainerController.GetOpponent().GetActivePokemon() = {trainerController.GetOpponent().GetActivePokemon().GetNickname()}");
        if (oldHP == null)
        {
            oldHP = trainerController.GetOpponent().GetActivePokemon().GetHPStat();
        }
        if (menu == Menus.GeneralBattleMenu)
        {
            pokemonNameLabelGB.text = trainerController.GetOpponent().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelGB.text = $"Lv. {trainerController.GetOpponent().GetActivePokemon().GetLevel()}";
            hpBarGB.highValue = trainerController.GetOpponent().GetActivePokemon().GetMaxHPStat();
            hpBarGB.value = (float)oldHP;
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            pokemonNameLabelMS.text = trainerController.GetOpponent().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelMS.text = $"Lv. {trainerController.GetOpponent().GetActivePokemon().GetLevel()}";
            hpBarMS.highValue = trainerController.GetOpponent().GetActivePokemon().GetMaxHPStat();
            hpBarMS.value = (float)oldHP;
        }
        else if (menu == Menus.OpposingPokemonDamagedScreen)
        {
            pokemonNameLabelOD.text = trainerController.GetOpponent().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelOD.text = $"Lv. {trainerController.GetOpponent().GetActivePokemon().GetLevel()}";
            hpBarOD.highValue = trainerController.GetOpponent().GetActivePokemon().GetMaxHPStat();
            hpBarOD.value = (float)oldHP;
        }
        else if (menu == Menus.OpposingPokemonInfoScreen)
        {
            pokemonNameLabelPI.text = trainerController.GetOpponent().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelPI.text = $"Lv. {trainerController.GetOpponent().GetActivePokemon().GetLevel()}";
            hpBarPI.highValue = trainerController.GetOpponent().GetActivePokemon().GetMaxHPStat();
            hpBarPI.value = (float)oldHP;
        }
    }
}
