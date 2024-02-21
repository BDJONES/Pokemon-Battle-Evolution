using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpposingPokemonInfoBarController : MonoBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] private GeneralBattleUIElements uIGBElements;
    [SerializeField] private MoveSelectionUIElements moveSelectionUIElements;
    [SerializeField] private OpposingPokemonDamagedUIElements opposingPokemonDamagedUIElements;
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


    private void OnEnable()
    {
        GameManager.OnStateChange += HandleGameStateChange;
        UIController.OnMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= HandleGameStateChange;
        UIController.OnMenuChange -= HandleMenuChange;
    }

    private void InitializeFields()
    {

        VisualElement pokemonInfoVE = uIGBElements.OpposingPokemonInfoBar.Query<VisualElement>("OpposingPokemonInfo");
        VisualElement battleInfoVE = pokemonInfoVE.Query<VisualElement>("Battle_Info");
        VisualElement nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
        VisualElement levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
        VisualElement hpBarVE = battleInfoVE.Query<VisualElement>("HPBar");
        // General Battle Section
        pokemonNameLabelGB = nameAndGenderVE.Query<Label>("Name");
        pokemonLevelLabelGB = levelInfoVE.Query<Label>("Level");
        hpBarGB = hpBarVE.Query<ProgressBar>("ProgressBar");
        infoButtonGB = battleInfoVE.Query<Button>("OpposingPokemonInfoButton");
        // Move Selection Section
        pokemonInfoVE = moveSelectionUIElements.OpposingPokemonInfoBar.Query<VisualElement>("OpposingPokemonInfo");
        battleInfoVE = pokemonInfoVE.Query<VisualElement>("Battle_Info");
        nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
        levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
        hpBarVE = battleInfoVE.Query<VisualElement>("HPBar");

        pokemonNameLabelMS = nameAndGenderVE.Query<Label>("Name");
        pokemonLevelLabelMS = levelInfoVE.Query<Label>("Level");
        hpBarMS = hpBarVE.Query<ProgressBar>("ProgressBar");
        infoButtonMS = battleInfoVE.Query<Button>("OpposingPokemonInfoButton");
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

    private void Start()
    {
        Debug.Log("Starting Opposing Pokemon");
        //InitializeFields();
    }

    public async UniTask UpdateHealthBar(Menus menu)
    {
        ProgressBar hpBar;
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
            return;
        }
        if (menu == Menus.GeneralBattleMenu || menu == Menus.OpposingPokemonDamagedScreen)
        {
            int oldHPValue = Mathf.FloorToInt(hpBar.value);
            int newHPValue = trainerController.GetOpponent().GetActivePokemon().GetHPStat();
            if (oldHPValue > newHPValue)
            {
                while (hpBar.value > newHPValue) { 
                    //hpBar.schedule.Execute(() =>
                    //{
                        hpBar.value -= 1f;
                    
                    //}).Every(30).Until(() => hpBar.value <= newHPValue);
                    await UniTask.WaitForSeconds(0.02f);
                }

            }
            else
            {
                while (hpBar.value < newHPValue)
                {
                        hpBar.value += 1f;
                    await UniTask.WaitForSeconds(0.02f);
                }
            }
            if (newHPValue == 0)
            {
                OpponentPokemonDeathEventManager.AlertOfDeath();
            }
                
        }
        
        return;
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
        if (menu == Menus.GeneralBattleMenu || menu == Menus.MoveSelectionMenu || menu == Menus.OpposingPokemonDamagedScreen) 
        {
            InitializeFields();
            UpdateInfo(menu);
        }
    }
    
    private void UpdateInfo(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            pokemonNameLabelGB.text = trainerController.GetOpponent().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelGB.text = $"Lv. {trainerController.GetOpponent().GetActivePokemon().GetLevel()}";
            hpBarGB.highValue = trainerController.GetOpponent().GetActivePokemon().GetMaxHPStat();
            hpBarGB.value = trainerController.GetOpponent().GetActivePokemon().GetHPStat();
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            pokemonNameLabelMS.text = trainerController.GetOpponent().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelMS.text = $"Lv. {trainerController.GetOpponent().GetActivePokemon().GetLevel()}";
            hpBarMS.highValue = trainerController.GetOpponent().GetActivePokemon().GetMaxHPStat();
            hpBarMS.value = trainerController.GetOpponent().GetActivePokemon().GetHPStat();
        }
        else if (menu == Menus.OpposingPokemonDamagedScreen)
        {
            pokemonNameLabelOD.text = trainerController.GetOpponent().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelOD.text = $"Lv. {trainerController.GetOpponent().GetActivePokemon().GetLevel()}";
            hpBarOD.highValue = trainerController.GetOpponent().GetActivePokemon().GetMaxHPStat();
            hpBarOD.value = trainerController.GetOpponent().GetActivePokemon().GetHPStat();
        }
    }
}
