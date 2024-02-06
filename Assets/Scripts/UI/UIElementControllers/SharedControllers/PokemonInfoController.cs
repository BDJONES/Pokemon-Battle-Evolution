using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonInfoController : MonoBehaviour
{
    [SerializeField] private GeneralBattleUIElements uIGBElements;
    [SerializeField] private MoveSelectionUIElements moveSelectionUIElements;
    private Label pokemonNameLabelGB;
    private Label pokemonLevelLabelGB;
    private ProgressBar hpBarGB;
    private Label hpStatLabelGB;
    private Button infoButtonGB;
    private Label pokemonNameLabelMS;
    private Label pokemonLevelLabelMS;
    private ProgressBar hpBarMS;
    private Button infoButtonMS;
    private Label hpStatLabelMS;

    private void OnEnable()
    {
        GameManager.OnStateChange += HandleGameStateChange;
        UIController.OnMenuChange += HandleMenuChange;
        //uiElements.PokemonButton.clicked += UpdateInfo;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= HandleGameStateChange;
        //if (uIGBElements.PokemonButton != null )
        //{
        //    uIGBElements.PokemonButton.clicked -= UpdateInfo;
        //}
    }

    private void InitializeFields() {
        VisualElement battleInfoVE = uIGBElements.PokemonInfoBar.Query<VisualElement>("Battle_Info");
        VisualElement nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
        VisualElement levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
        VisualElement hpBarVE = battleInfoVE.Query<VisualElement>("HPInfo");
        // General Battle Section
        pokemonNameLabelGB = nameAndGenderVE.Query<Label>("Name");
        pokemonLevelLabelGB = levelInfoVE.Query<Label>("Level");
        hpBarGB = hpBarVE.Query<ProgressBar>();
        hpStatLabelGB = hpBarGB.Query<Label>("HPStat");
        infoButtonGB = battleInfoVE.Query<Button>("OpposingPokemonInfoButton");
        // Move Selection Section
        battleInfoVE = moveSelectionUIElements.PokemonInfoBar.Query<VisualElement>("Battle_Info");
        nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
        levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
        hpBarVE = battleInfoVE.Query<VisualElement>("HPInfo");

        pokemonNameLabelMS = nameAndGenderVE.Query<Label>("Name");
        pokemonLevelLabelMS = levelInfoVE.Query<Label>("Level");
        hpBarMS = hpBarVE.Query<ProgressBar>();
        hpStatLabelMS = hpBarVE.Query<Label>("HPStat");
        infoButtonMS = battleInfoVE.Query<Button>("OpposingPokemonInfoButton");
    }

    private void Start()
    {
        InitializeFields();
    }

    public async UniTask UpdateHealthBar(Menus menu)
    {
        ProgressBar hpBar;
        Label hpStatLabel;
        if (menu == Menus.GeneralBattleMenu)
        {
            hpBar = hpBarGB;
            hpStatLabel = hpStatLabelGB;
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            hpBar = hpBarMS;
            hpStatLabel = hpStatLabelMS;
        }
        else
        {
            return;
        }
        int oldHPValue = Mathf.FloorToInt(hpBar.value);
        //Debug.Log($"oldHPValue = {oldHPValue}");
        int newHPValue = GameManager.Instance.trainer1.activePokemon.GetHPStat();
        //Debug.Log($"newHPValue = {newHPValue}");
        if (oldHPValue > newHPValue)
        {
            while (hpBar.value > newHPValue)
            {
                //hpBar.schedule.Execute(() =>
                //{
                hpBar.value -= 1f;
                hpStatLabel.text = $"{hpBar.value}/{GameManager.Instance.trainer1.activePokemon.GetMaxHPStat()}";
                //}).Every(30).Until(() => hpBar.value <= newHPValue);
                await UniTask.WaitForSeconds(0.02f);
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
                await UniTask.WaitForSeconds(0.02f);
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
        //else if (state == GameState.FirstAttack ||  state == GameState.SecondAttack)
        //{
        //    Debug.Log("The correct game state has been achieved");
        //int oldHPValue = Mathf.FloorToInt(hpBar.value);
        //Debug.Log($"oldHPValue = {oldHPValue}");
        //int newHPValue = GameManager.Instance.trainer1.activePokemon.GetHPStat();
        //Debug.Log($"newHPValue = {newHPValue}");
        //    await UpdateHealthBar(oldHPValue, newHPValue);
        //}
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu || menu == Menus.MoveSelectionMenu)
        {
            Debug.Log("The menu has changed back to normal");
            InitializeFields();
            UpdateInfo(menu);
        }
    }

    private void UpdateInfo(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            pokemonNameLabelGB.text = GameManager.Instance.trainer1.activePokemon.GetSpeciesName();
            pokemonLevelLabelGB.text = $"Lv. {GameManager.Instance.trainer1.activePokemon.GetLevel()}";
            //Debug.Log(hpBarGB.highValue);
            hpBarGB.highValue = GameManager.Instance.trainer1.activePokemon.GetMaxHPStat();
            hpBarGB.value = GameManager.Instance.trainer1.activePokemon.GetHPStat();
            hpStatLabelGB.text = $"{GameManager.Instance.trainer1.activePokemon.GetHPStat()}/{GameManager.Instance.trainer1.activePokemon.GetMaxHPStat()}";
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            pokemonNameLabelMS.text = GameManager.Instance.trainer1.activePokemon.GetSpeciesName();
            pokemonLevelLabelMS.text = $"Lv. {GameManager.Instance.trainer1.activePokemon.GetLevel()}";
            hpBarMS.highValue = GameManager.Instance.trainer1.activePokemon.GetMaxHPStat();
            hpBarMS.value = GameManager.Instance.trainer1.activePokemon.GetHPStat();
            hpStatLabelMS.text = $"{GameManager.Instance.trainer1.activePokemon.GetHPStat()}/{GameManager.Instance.trainer1.activePokemon.GetMaxHPStat()}";
        }

    }
}
