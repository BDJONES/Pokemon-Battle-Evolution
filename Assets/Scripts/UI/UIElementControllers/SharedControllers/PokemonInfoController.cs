using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonInfoController : MonoBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] private GeneralBattleUIElements uIGBElements;
    [SerializeField] private MoveSelectionUIElements moveSelectionUIElements;
    [SerializeField] private PokemonDamagedUIElements pokemonDamageUIElements;
    [SerializeField] private PokemonInfoUIElements pokemonInfoUIElements;
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
    private Label hpStatLabelPD;
    private Label pokemonNameLabelPI;
    private Label pokemonLevelLabelPI;
    private ProgressBar hpBarPI;
    private Label hpStatLabelPI;
    private UIController uIController;

    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        trainerController = transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
        uIGBElements = uIController.GetComponent<GeneralBattleUIElements>();
        moveSelectionUIElements = uIController.GetComponent<MoveSelectionUIElements>();
        pokemonDamageUIElements = uIController.GetComponent<PokemonDamagedUIElements>();
        pokemonInfoUIElements = uIController.GetComponent<PokemonInfoUIElements>();
        uIController.OnHostMenuChange += HandleMenuChange;
        uIController.OnClientMenuChange += HandleMenuChange;
        GameManager.OnStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        uIController.OnHostMenuChange -= HandleMenuChange;
        uIController.OnClientMenuChange -= HandleMenuChange;
        GameManager.OnStateChange -= HandleGameStateChange;
    }

    private void InitializeFields(Menus menu) {
        VisualElement battleInfoVE;
        VisualElement nameAndGenderVE;
        VisualElement levelInfoVE;
        VisualElement hpBarVE;
        if (menu == Menus.GeneralBattleMenu)
        {
            // General Battle Section
            battleInfoVE = uIGBElements.PokemonInfoBar.Query<VisualElement>("Battle_Info");
            nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
            levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
            hpBarVE = battleInfoVE.Query<VisualElement>("HPInfo");
            
            pokemonNameLabelGB = nameAndGenderVE.Query<Label>("Name");
            pokemonLevelLabelGB = levelInfoVE.Query<Label>("Level");
            hpBarGB = hpBarVE.Query<ProgressBar>();
            hpStatLabelGB = hpBarGB.Query<Label>("HPStat");
            infoButtonGB = uIGBElements.PokemonInfoBar.Query<Button>("PokemonInfoButton");
        }
        if (menu == Menus.MoveSelectionMenu)
        {
            // Move Selection Section
            battleInfoVE = moveSelectionUIElements.PokemonInfoBar.Query<VisualElement>("Battle_Info");
            nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
            levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
            hpBarVE = battleInfoVE.Query<VisualElement>("HPInfo");

            pokemonNameLabelMS = nameAndGenderVE.Query<Label>("Name");
            pokemonLevelLabelMS = levelInfoVE.Query<Label>("Level");
            hpBarMS = hpBarVE.Query<ProgressBar>();
            hpStatLabelMS = hpBarVE.Query<Label>("HPStat");
            infoButtonMS = uIGBElements.PokemonInfoBar.Query<Button>("PokemonInfoButton");
        }

        if (menu == Menus.PokemonDamagedScreen)
        {
            // Pokemon Damaged
            battleInfoVE = pokemonDamageUIElements.PokemonInfoBar.Query<VisualElement>("Battle_Info");
            nameAndGenderVE = battleInfoVE.Query<VisualElement>("Name_And_Gender");
            levelInfoVE = battleInfoVE.Query<VisualElement>("LevelInfo");
            hpBarVE = battleInfoVE.Query<VisualElement>("HPInfo");

            pokemonNameLabelPD = nameAndGenderVE.Query<Label>("Name");
            pokemonLevelLabelPD = levelInfoVE.Query<Label>("Level");
            hpBarPD = hpBarVE.Query<ProgressBar>();
            hpStatLabelPD = hpBarVE.Query<Label>("HPStat");
        }
        
        if (menu == Menus.PokemonInfoScreen)
        {
            // Pokemon Info
            Debug.Log("Getting the info screen elements");
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
        //InitializeFields();
    }

    public async void UpdateHealthBar(Menus menu)
    {
        Debug.Log("Starting HP Update");
        GameManager gameManager = GameManager.Instance;
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
        else if (menu == Menus.PokemonInfoScreen)
        {
            hpBar = hpBarPI;
            hpStatLabel = hpStatLabelPI;
        }
        else
        {
            Debug.Log("In Else for some reason");
            return;
        }
        if (menu == Menus.PokemonDamagedScreen) // || menu == Menus.GeneralBattleMenu
        {
            int oldHPValue = Mathf.FloorToInt(hpBar.value);
            //Debug.Log($"oldHPValue = {oldHPValue}");
            int newHPValue = trainerController.GetPlayer().GetActivePokemon().GetHPStat();
            //Debug.Log($"newHPValue = {newHPValue}");
            if (oldHPValue > newHPValue)
            {
                while (hpBar.value > newHPValue)
                {
                    //hpBar.schedule.Execute(() =>
                    //{
                    hpBar.value -= 1f;
                    hpStatLabel.text = $"{hpBar.value}/{trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat()}";
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
                YourPokemonDeathEventManager.AlertOfDeath();
            }
        }
        Debug.Log("Finished Updating Info");
        // If the pokemon reaches 0 hp, then play animation of faint
        gameManager.FinishRPCTaskRpc();
        return;
    }

    public void HandleGameStateChange(GameState state)
    {
        //if (state == GameState.BattleStart)
        //{
        //    //InitializeFields();
        //    UpdateInfo(Menus.GeneralBattleMenu);
        //}
        //else if (state == GameState.FirstAttack ||  state == GameState.SecondAttack)
        //{
        //    Debug.Log("The correct game state has been achieved");
        //int oldHPValue = Mathf.FloorToInt(hpBar.value);
        //Debug.Log($"oldHPValue = {oldHPValue}");
        //int newHPValue = GameManager.Instance.trainer1.GetActivePokemon().GetHPStat();
        //Debug.Log($"newHPValue = {newHPValue}");
        //    await UpdateHealthBar(oldHPValue, newHPValue);
        //}
    }

    private void HandleMenuChange(Menus menu)
    {
        var player = transform.parent.parent.gameObject;
        if (menu == Menus.GeneralBattleMenu || menu == Menus.MoveSelectionMenu || menu == Menus.PokemonDamagedScreen || menu == Menus.PokemonInfoScreen) 
        {
            InitializeFields(menu);
            if (menu == Menus.GeneralBattleMenu && infoButtonGB != null)
            {
                if (TrainerController.IsOwnerHost(player))
                {
                    UIEventSubscriptionManager.Subscribe(infoButtonGB, ClickedInfoButton, 1);
                }
                else
                {
                    UIEventSubscriptionManager.Subscribe(infoButtonGB, ClickedInfoButton, 2);
                }
                
            }
            else if (menu == Menus.GeneralBattleMenu && infoButtonGB == null)
            {
                Debug.Log("Info Button was null");
            }

            if (menu == Menus.MoveSelectionMenu && infoButtonMS != null)
            {
                if (TrainerController.IsOwnerHost(player))
                {
                    UIEventSubscriptionManager.Subscribe(infoButtonMS, ClickedInfoButton, 1);
                }
                else
                {
                    UIEventSubscriptionManager.Subscribe(infoButtonMS, ClickedInfoButton, 2);
                }
            }
            else if (menu == Menus.MoveSelectionMenu && infoButtonMS == null)
            {
                Debug.Log("Info Button was null");
            }
            //Debug.Log("Just initialized the fields");
            UpdateInfo(menu);
        }
    }

    private void ClickedInfoButton()
    {
        //Debug.Log("Clicked the infoButton");
        var player = transform.parent.parent.gameObject;
        if (TrainerController.IsOwnerHost(player))
        {
            uIController.UpdateMenu(Menus.PokemonInfoScreen, 1);
        }
        else
        {
            uIController.UpdateMenu(Menus.PokemonInfoScreen, 2);
        }
    }

    private void UpdateInfo(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            //Debug.Log("Perfectly fine");
            pokemonNameLabelGB.text = trainerController.GetPlayer().GetActivePokemon().GetNickname();
            pokemonLevelLabelGB.text = $"Lv. {trainerController.GetPlayer().GetActivePokemon().GetLevel()}";
            hpBarGB.highValue = trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat();
            hpBarGB.value = trainerController.GetPlayer().GetActivePokemon().GetHPStat();
            hpStatLabelGB.text = $"{trainerController.GetPlayer().GetActivePokemon().GetHPStat()}/{trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat()}";
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            pokemonNameLabelMS.text = trainerController.GetPlayer().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelMS.text = $"Lv. {trainerController.GetPlayer().GetActivePokemon().GetLevel()}";
            hpBarMS.highValue = trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat();
            hpBarMS.value = trainerController.GetPlayer().GetActivePokemon().GetHPStat();
            hpStatLabelMS.text = $"{trainerController.GetPlayer().GetActivePokemon().GetHPStat()}/{trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat()}";
        }
        else if (menu == Menus.PokemonDamagedScreen)
        {
            pokemonNameLabelPD.text = trainerController.GetPlayer().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelPD.text = $"Lv. {trainerController.GetPlayer().GetActivePokemon().GetLevel()}";
            hpBarPD.highValue = trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat();
            hpBarPD.value = trainerController.GetPlayer().GetActivePokemon().GetHPStat();
            hpStatLabelPD.text = $"{trainerController.GetPlayer().GetActivePokemon().GetHPStat()}/{trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat()}";
        }
        else if (menu == Menus.PokemonInfoScreen)
        {
            pokemonNameLabelPI.text = trainerController.GetPlayer().GetActivePokemon().GetSpeciesName();
            pokemonLevelLabelPI.text = $"Lv. {trainerController.GetPlayer().GetActivePokemon().GetLevel()}";
            hpBarPI.highValue = trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat();
            hpBarPI.value = trainerController.GetPlayer().GetActivePokemon().GetHPStat();
            hpStatLabelPI.text = $"{trainerController.GetPlayer().GetActivePokemon().GetHPStat()}/{trainerController.GetPlayer().GetActivePokemon().GetMaxHPStat()}";
        }
    }
}
