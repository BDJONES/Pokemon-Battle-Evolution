using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class StatStagesController : NetworkBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] private PokemonInfoUIElements pokemonInfoUIElements;
    private Label attackLabel;
    private Label defenseLabel;
    private Label specialAttackLabel;
    private Label specialDefenseLabel;
    private Label speedLabel;
    private Label accuracyLabel;
    private Label evasionLabel;
    private UIController uIController;
    private GameObject opponent;

    private void OnEnable()
    {
        //Debug.Log($"Trainer Name while enabling StatStages is {gameObject.name}");
        //if (IsOwner)
        //{
            NetworkCommands.UIControllerCreated += () =>
            {
                trainerController = transform.parent.parent.gameObject.GetComponent<TrainerController>();
                uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
                uIController.OnHostMenuChange += HandleMenuChange;
                uIController.OnClientMenuChange += HandleMenuChange;
                pokemonInfoUIElements = uIController.GetComponent<PokemonInfoUIElements>();
                opponent = GameObject.Find("Trainer(Clone)");
            };
        //}
    }

    private void OnDisable()
    {
        if (uIController != null)
        {
            uIController.OnHostMenuChange -= HandleMenuChange;
            uIController.OnClientMenuChange -= HandleMenuChange;
        }
    }
    private void InitializeFields()
    {
        VisualElement atk_and_def = pokemonInfoUIElements.StatStages.Query<VisualElement>("Atk_And_Def");
        VisualElement spattk_and_spdef = pokemonInfoUIElements.StatStages.Query<VisualElement>("SpAtk_And_SpDef");
        VisualElement acc_and_eva = pokemonInfoUIElements.StatStages.Query<VisualElement>("Acc_And_Eva");
        attackLabel = atk_and_def.Query<Label>("Attack");
        defenseLabel = atk_and_def.Query<Label>("Defense");
        specialAttackLabel = spattk_and_spdef.Query<Label>("SpecialAttack");
        specialDefenseLabel = spattk_and_spdef.Query<Label>("SpecialDefense");
        speedLabel = pokemonInfoUIElements.StatStages.Query<Label>("Speed");
        accuracyLabel = acc_and_eva.Query<Label>("Accuracy");
        evasionLabel = acc_and_eva.Query<Label>("Evasion");
    }


    private void HandleMenuChange(Menus menu)
    {
        //Debug.Log($"You are on this menu: {menu}");
        if (menu == Menus.PokemonInfoScreen)
        {
            InitializeFields();
            UpdateYourPokemonInfo();
        }
        else if (menu == Menus.OpposingPokemonInfoScreen)
        {
            InitializeFields();
            UpdateOpponentPokemonInfo();
        }
    }
    private void UpdateYourPokemonInfo()
    {
        trainerController = transform.parent.parent.gameObject.GetComponent<TrainerController>();
        string attackStage = trainerController.GetPlayer().GetActivePokemon().AttackStage >= 0 ? $"+{trainerController.GetPlayer().GetActivePokemon().AttackStage}" : $"{trainerController.GetPlayer().GetActivePokemon().AttackStage}";
        string defenseStage = trainerController.GetPlayer().GetActivePokemon().DefenseStage >= 0 ? $"+{trainerController.GetPlayer().GetActivePokemon().DefenseStage}" : $"{trainerController.GetPlayer().GetActivePokemon().DefenseStage}";
        string specialAttackStage = trainerController.GetPlayer().GetActivePokemon().SpecialAttackStage >= 0 ? $"+{trainerController.GetPlayer().GetActivePokemon().SpecialAttackStage}" : $"{trainerController.GetPlayer().GetActivePokemon().SpecialAttackStage}";
        string specialDefenseStage = trainerController.GetPlayer().GetActivePokemon().SpecialDefenseStage >= 0 ? $"+{trainerController.GetPlayer().GetActivePokemon().SpecialDefenseStage}" : $"{trainerController.GetPlayer().GetActivePokemon().SpecialDefenseStage}";
        string speedStage = trainerController.GetPlayer().GetActivePokemon().SpeedStage >= 0 ? $"+{trainerController.GetPlayer().GetActivePokemon().SpeedStage}" : $"{trainerController.GetPlayer().GetActivePokemon().SpeedStage}";

        attackLabel.text = $"Atk: {attackStage}";
        defenseLabel.text = $"Def: {defenseStage}";
        specialAttackLabel.text = $"Sp.Atk: {specialAttackStage}";
        specialDefenseLabel.text = $"Sp.Def: {specialDefenseStage}";
        speedLabel.text = $"Speed: {speedStage}";
    }

    private void UpdateOpponentPokemonInfo()
    {
        Debug.Log($"In UpdateOpponentPokemonInfo Function Finding Opponent {GameObject.Find("Trainer(Clone)").GetComponent<Trainer>().GetActivePokemon().GetSpeciesName()}");
        string attackStage = trainerController.GetOpponent().GetActivePokemon().AttackStage >= 0 ? $"+{trainerController.GetOpponent().GetActivePokemon().AttackStage}" : $"{trainerController.GetOpponent().GetActivePokemon().AttackStage}";
        string defenseStage = trainerController.GetOpponent().GetActivePokemon().DefenseStage >= 0 ? $"+{trainerController.GetOpponent().GetActivePokemon().DefenseStage}" : $"{trainerController.GetOpponent().GetActivePokemon().DefenseStage}";
        string specialAttackStage = trainerController.GetOpponent().GetActivePokemon().SpecialAttackStage >= 0 ? $"+{trainerController.GetOpponent().GetActivePokemon().SpecialAttackStage}" : $"{trainerController.GetOpponent().GetActivePokemon().SpecialAttackStage}";
        string specialDefenseStage = trainerController.GetOpponent().GetActivePokemon().SpecialDefenseStage >= 0 ? $"+{trainerController.GetOpponent().GetActivePokemon().SpecialDefenseStage}" : $"{trainerController.GetOpponent().GetActivePokemon().SpecialDefenseStage}";
        string speedStage = trainerController.GetOpponent().GetActivePokemon().SpeedStage >= 0 ? $"+{trainerController.GetOpponent().GetActivePokemon().SpeedStage}" : $"{trainerController.GetOpponent().GetActivePokemon().SpeedStage}";
        
        attackLabel.text = $"Atk: {attackStage}";
        defenseLabel.text = $"Def: {defenseStage}";
        specialAttackLabel.text = $"Sp.Atk: {specialAttackStage}";
        specialDefenseLabel.text = $"Sp.Def: {specialDefenseStage}";
        speedLabel.text = $"Speed: {speedStage}";
        //Debug.Log($"attackStage = {attackStage}");
        //Debug.Log(attackLabel.text);
        //Debug.Log(defenseLabel.text);
        //Debug.Log(specialAttackLabel.text);
        //Debug.Log(specialDefenseLabel.text);
        //Debug.Log(speedLabel.text);
    }
}
