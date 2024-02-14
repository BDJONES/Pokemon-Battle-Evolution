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
    [SerializeField] private PokemonDamagedUIElements pokemonDamageUIElements;
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
    private Label pokemonNameLabelPD;
    private Label pokemonLevelLabelPD;
    private ProgressBar hpBarPD;
    private Button infoButtonPD;
    private Label hpStatLabelPD;

    private void OnEnable()
    {
        GameManager.OnStateChange += HandleGameStateChange;
        UIController.OnMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= HandleGameStateChange;
        UIController.OnMenuChange -= HandleMenuChange;
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
        // Pokemon Damaged
        battleInfoVE = pokemonDamageUIElements.PokemonInfoBar.Query<VisualElement>("Battle_Info");
        nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
        levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
        hpBarVE = battleInfoVE.Query<VisualElement>("HPInfo");

        pokemonNameLabelPD = nameAndGenderVE.Query<Label>("Name");
        pokemonLevelLabelPD = levelInfoVE.Query<Label>("Level");
        hpBarPD = hpBarVE.Query<ProgressBar>();
        hpStatLabelPD = hpBarVE.Query<Label>("HPStat");
        infoButtonPD = battleInfoVE.Query<Button>("OpposingPokemonInfoButton");
    }

    private void Start()
    {
        Debug.Log("Starting In Pokemon");
        //InitializeFields();
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
        else if (menu == Menus.PokemonDamagedScreen)
        {
            hpBar = hpBarPD;
            hpStatLabel = hpStatLabelPD;
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
        if (newHPValue == 0)
        {
            //Debug.Log("Pokemon has fainted");
            YourPokemonDeathEventManager.AlertOfDeath();
        }
        // If the pokemon reaches 0 hp, then play animation of faint
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
        if (menu == Menus.GeneralBattleMenu || menu == Menus.MoveSelectionMenu || menu == Menus.PokemonDamagedScreen) 
        {
            //Debug.Log("The menu has changed back to normal");
            InitializeFields();
            UpdateInfo(menu);
        }
    }

    private void UpdateInfo(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            Debug.Log("Perfectly fine");
            pokemonNameLabelGB.text = GameManager.Instance.trainer1.activePokemon.GetSpeciesName();
            pokemonLevelLabelGB.text = $"Lv. {GameManager.Instance.trainer1.activePokemon.GetLevel()}";
            //Debug.Log(hpBarGB.highValue);
            hpBarGB.highValue = GameManager.Instance.trainer1.activePokemon.GetMaxHPStat();
            hpBarGB.value = GameManager.Instance.trainer1.activePokemon.GetHPStat();
            hpStatLabelGB.text = $"{GameManager.Instance.trainer1.activePokemon.GetHPStat()}/{GameManager.Instance.trainer1.activePokemon.GetMaxHPStat()}";
            Debug.Log("Perfectly fine");
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            pokemonNameLabelMS.text = GameManager.Instance.trainer1.activePokemon.GetSpeciesName();
            pokemonLevelLabelMS.text = $"Lv. {GameManager.Instance.trainer1.activePokemon.GetLevel()}";
            hpBarMS.highValue = GameManager.Instance.trainer1.activePokemon.GetMaxHPStat();
            hpBarMS.value = GameManager.Instance.trainer1.activePokemon.GetHPStat();
            hpStatLabelMS.text = $"{GameManager.Instance.trainer1.activePokemon.GetHPStat()}/{GameManager.Instance.trainer1.activePokemon.GetMaxHPStat()}";
        }
        else if (menu == Menus.PokemonDamagedScreen)
        {
            pokemonNameLabelPD.text = GameManager.Instance.trainer1.activePokemon.GetSpeciesName();
            pokemonLevelLabelPD.text = $"Lv. {GameManager.Instance.trainer1.activePokemon.GetLevel()}";
            hpBarPD.highValue = GameManager.Instance.trainer1.activePokemon.GetMaxHPStat();
            hpBarPD.value = GameManager.Instance.trainer1.activePokemon.GetHPStat();
            hpStatLabelPD.text = $"{GameManager.Instance.trainer1.activePokemon.GetHPStat()}/{GameManager.Instance.trainer1.activePokemon.GetMaxHPStat()}";
        }
    }
}
