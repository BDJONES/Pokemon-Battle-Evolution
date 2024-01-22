using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpposingPokemonInfoBarController : MonoBehaviour
{
    [SerializeField] private UIToolkitElements uiElements;
    private Label pokemonNameLabel;
    private Label pokemonLevelLabel;
    private ProgressBar hpBar;
    private Button infoButton;


    private void OnEnable()
    {
        GameManager.OnStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= HandleGameStateChange;
    }

    private void Start()
    {
        VisualElement pokemonInfoVE = uiElements.OpposingPokemonInfoBar.Query<VisualElement>("OpposingPokemonInfo");
        VisualElement battleInfoVE = pokemonInfoVE.Query<VisualElement>("Battle_Info");
        VisualElement nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
        VisualElement LevelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
        VisualElement hpBarVE = battleInfoVE.Query<VisualElement>("HPBar");

        pokemonNameLabel = nameAndGenderVE.Query<Label>("Name");
        pokemonLevelLabel = LevelInfoVE.Query<Label>("Level");
        hpBar = hpBarVE.Query<ProgressBar>("ProgressBar");
        infoButton = battleInfoVE.Query<Button>("OpposingPokemonInfoButton");
    }

    public async UniTask UpdateHealthBar()
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
                //hpBar.schedule.Execute(() =>
                //{
                    hpBar.value += 1f;

                //}).Every(50).Until(() => hpBar.value >= newHPValue);
                await UniTask.WaitForSeconds(0.02f);
            }
        }
        //await UniTask.Yield();
        return;
    }

    public void HandleGameStateChange(GameState state)
    {
        if (state == GameState.BattleStart)
        {
            pokemonNameLabel.text = GameManager.Instance.trainer2.activePokemon.GetSpeciesName();
            pokemonLevelLabel.text = $"Lv. {GameManager.Instance.trainer2.activePokemon.GetLevel()}";
            hpBar.highValue = GameManager.Instance.trainer2.activePokemon.GetMaxHPStat();
            hpBar.value = GameManager.Instance.trainer2.activePokemon.GetHPStat();
        }
        //else if (state == GameState.FirstAttack || state == GameState.SecondAttack)
        //{
        //    Debug.Log("The correct game state has been achieved");
        //    int oldHPValue = Mathf.FloorToInt(hpBar.value);
        //    //Debug.Log($"oldHPValue = {oldHPValue}");
        //    int newHPValue = GameManager.Instance.trainer2.activePokemon.GetHPStat();
        //    //Debug.Log($"newHPValue = {newHPValue}");
        //    await UpdateHealthBar(oldHPValue, newHPValue);
        //}
    }
}
