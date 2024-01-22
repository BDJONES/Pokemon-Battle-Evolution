using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonInfoController : MonoBehaviour
{
    [SerializeField] private UIToolkitElements uiElements;
    private Label pokemonNameLabel;
    private Label pokemonLevelLabel;
    private ProgressBar hpBar;
    private Label hpStatLabel;
    private Button infoButton;

    private void OnEnable()
    {
        GameManager.OnStateChange += HandleGameStateChange;
        uiElements.PokemonButton.clicked += UpdateInfo;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= HandleGameStateChange;
        if (uiElements.PokemonButton != null )
        {
            uiElements.PokemonButton.clicked -= UpdateInfo;
        }
    }
    private void Start()
    {
        VisualElement pokemonInfoVE = uiElements.PokemonInfoBar.Query<VisualElement>("PokemonInfo");
        VisualElement battleInfoVE = pokemonInfoVE.Query<VisualElement>("Battle_Info");
        VisualElement nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
        VisualElement LevelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
        VisualElement hpInfoVE = battleInfoVE.Query<VisualElement>("HPInfo");

        pokemonNameLabel = nameAndGenderVE.Query<Label>("Name");
        pokemonLevelLabel = LevelInfoVE.Query<Label>("Level");
        hpBar = hpInfoVE.Query<ProgressBar>("HPBar");
        hpStatLabel = hpBar.Query<Label>("HPStat");
        infoButton = battleInfoVE.Query<Button>("PokemonInfoButton");
    }

    public async UniTask UpdateHealthBar()
    {
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
            UpdateInfo();
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
    private void UpdateInfo()
    {
        pokemonNameLabel.text = GameManager.Instance.trainer1.activePokemon.GetSpeciesName();
        pokemonLevelLabel.text = $"Lv. {GameManager.Instance.trainer1.activePokemon.GetLevel()}";
        hpBar.highValue = GameManager.Instance.trainer1.activePokemon.GetMaxHPStat();
        hpBar.value = GameManager.Instance.trainer1.activePokemon.GetHPStat();
        hpStatLabel.text = $"{GameManager.Instance.trainer1.activePokemon.GetHPStat()}/{GameManager.Instance.trainer1.activePokemon.GetMaxHPStat()}";
    }
}
