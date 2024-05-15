using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class StatusButtonBuilder : NetworkBehaviour
{
    [SerializeField] private VisualTreeAsset StatusButtonElement;
    [SerializeField] private PokemonInfoUIElements pokemonInfoUIElements;
    private UIController uIController;
    private TrainerController trainerController;
    private void OnEnable()
    {
        NetworkCommands.UIControllerCreated += () =>
        {
            uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
            uIController.OnHostMenuChange += HandleHostMenuChange;
            uIController.OnClientMenuChange += HandleClientMenuChange;
            pokemonInfoUIElements = uIController.GetComponent<PokemonInfoUIElements>();
            trainerController = gameObject.transform.parent.parent.gameObject.GetComponent<TrainerController>();
        };
    }

    private void OnDisable()
    {
        if (uIController != null)
        {
            uIController.OnHostMenuChange -= HandleHostMenuChange;
            uIController.OnClientMenuChange -= HandleClientMenuChange;
        }
    }

    private void HandleHostMenuChange(Menus menu)
    {
        if (IsOwner)
        {
            if (menu == Menus.PokemonInfoScreen)
            {
                CreateYourStatusButtons();
            }
            else if (menu == Menus.OpposingPokemonInfoScreen)
            {
                CreateOpponentStatusButtons();
            }
        }
    }

    private void HandleClientMenuChange(Menus menu)
    {
        if (IsOwner)
        {
            if (menu == Menus.PokemonInfoScreen)
            {
                CreateYourStatusButtons();
            }
            else if (menu == Menus.OpposingPokemonInfoScreen)
            {
                CreateOpponentStatusButtons();
            }
        }
    }

    private void CreateYourStatusButtons()
    {

        Trainer trainer = trainerController.GetPlayer();
        Debug.Log("Creating Your Status Buttons");
        if (trainer.GetActivePokemon().Status != StatusConditions.Healthy)
        {
            var StatusButton = StatusButtonElement.Instantiate();
            VisualElement statusContent = StatusButton.Query<VisualElement>("Content");
            Label statusText = statusContent.Query<Label>();
            statusText.text = trainer.GetActivePokemon().Status.ToString();
            pokemonInfoUIElements.StatusButtons.Add(StatusButton);
        }
        if (trainer.GetActivePokemon().GetItem() != null)
        {
            var ItemButton = StatusButtonElement.Instantiate();
            VisualElement itemContent = ItemButton.Query<VisualElement>("Content");
            Label itemText = itemContent.Query<Label>();
            itemText.text = trainer.GetActivePokemon().GetItem().GetItemName();
            pokemonInfoUIElements.StatusButtons.Add(ItemButton);
        }
        var newButton = StatusButtonElement.Instantiate();
        VisualElement content = newButton.Query<VisualElement>("Content");
        Label text = content.Query<Label>();
        text.text = trainer.GetActivePokemon().GetAbility().GetAbilityName();
        pokemonInfoUIElements.StatusButtons.Add(newButton);
    }

    private void CreateOpponentStatusButtons()
    {
        Trainer trainer = trainerController.GetOpponent();
        Debug.Log("Creating Opponent Status Buttons");
        if (trainer.GetActivePokemon().Status != StatusConditions.Healthy)
        {
            var newButton = StatusButtonElement.Instantiate();
            Button content = newButton.Query<Button>("Content");
            Label text = content.Query<Label>();
            text.text = trainer.GetActivePokemon().Status.ToString();
            pokemonInfoUIElements.StatusButtons.Add(newButton);
        }
    }
}