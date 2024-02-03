using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpposingPokemonInfoBarController : MonoBehaviour
{
    [SerializeField] private GeneralBattleUIElements uIGBElements;
    [SerializeField] private MoveSelectionUIElements moveSelectionUIElements;
    private Label pokemonNameLabelGB;
    private Label pokemonLevelLabelGB;
    private ProgressBar hpBarGB;
    private Button infoButtonGB;
    private Label pokemonNameLabelMS;
    private Label pokemonLevelLabelMS;
    private ProgressBar hpBarMS;
    private Button infoButtonMS;


    private void OnEnable()
    {
        GameManager.OnStateChange += HandleGameStateChange;
        UIController.OnMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= HandleGameStateChange;
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
    }

    private void Start()
    {
        InitializeFields();
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
        else
        {
            return;
        }
        if (menu == Menus.GeneralBattleMenu)
        {
            int oldHPValue = Mathf.FloorToInt(hpBar.value);
            int newHPValue = GameManager.Instance.trainer2.activePokemon.GetHPStat();
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
        }
        return;
    }

    public void HandleGameStateChange(GameState state)
    {
        if (state == GameState.BattleStart)
        {
            UpdateInfo(Menus.GeneralBattleMenu);
        }
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu || menu == Menus.MoveSelectionMenu)
        {
            InitializeFields();
            UpdateInfo(menu);
        }
    }
    
    private void UpdateInfo(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            pokemonNameLabelGB.text = GameManager.Instance.trainer2.activePokemon.GetSpeciesName();
            pokemonLevelLabelGB.text = $"Lv. {GameManager.Instance.trainer2.activePokemon.GetLevel()}";
            hpBarGB.highValue = GameManager.Instance.trainer2.activePokemon.GetMaxHPStat();
            hpBarGB.value = GameManager.Instance.trainer2.activePokemon.GetHPStat();
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            pokemonNameLabelMS.text = GameManager.Instance.trainer2.activePokemon.GetSpeciesName();
            pokemonLevelLabelMS.text = $"Lv. {GameManager.Instance.trainer2.activePokemon.GetLevel()}";
            hpBarMS.highValue = GameManager.Instance.trainer2.activePokemon.GetMaxHPStat();
            hpBarMS.value = GameManager.Instance.trainer2.activePokemon.GetHPStat();
        }
    }
}
