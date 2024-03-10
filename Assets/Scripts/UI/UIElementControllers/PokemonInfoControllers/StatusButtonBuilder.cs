using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StatusButtonBuilder : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset StatusButtonElement;
    [SerializeField] private TrainerController trainerController;
    [SerializeField] private PokemonInfoUIElements pokemonInfoUIElements;
    private UIController uIController;

    private void OnEnable()
    {
        uIController = transform.parent.gameObject.GetComponentInChildren<UIController>();
        uIController.OnMenuChange += HandleMenuChange;
        pokemonInfoUIElements = uIController.GetComponent<PokemonInfoUIElements>();
    }

    private void OnDisable()
    {
        uIController.OnMenuChange -= HandleMenuChange;
    }    
    
    private void HandleMenuChange(Menus menu)
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

    private void CreateYourStatusButtons()
    {
        Trainer trainer = trainerController.GetPlayer();
        //GameObject StatusButtonHolder = new GameObject
        //{
        //    name = "WidgetHolder"
        //};
        if (trainer.GetActivePokemon().Status != StatusConditions.Healthy)
        {
            var newButton = StatusButtonElement.Instantiate();
            VisualElement content = newButton.Query<VisualElement>("Content");
            Label text = content.Query<Label>();
            text.text = trainer.GetActivePokemon().Status.ToString();
            pokemonInfoUIElements.StatusButtons.Add(newButton);
        }
    }

    private void CreateOpponentStatusButtons()
    {
        Trainer trainer = trainerController.GetOpponent();
        //GameObject StatusButtonHolder = new GameObject
        //{
        //    name = "WidgetHolder"
        //};
        if (trainer.GetActivePokemon().Status != StatusConditions.Healthy)
        {
            var newButton = StatusButtonElement.Instantiate();
            Button content = newButton.Query<Button>("Element");
            Label text = content.Query<Label>();
            text.text = trainer.GetActivePokemon().Status.ToString();
            pokemonInfoUIElements.StatusButtons.Add(newButton);
        }
    }
}