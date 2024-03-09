using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BackButtonController : MonoBehaviour
{
    [SerializeField] private InBattlePartyUIElements uIInBattleParty;
    [SerializeField] private MoveSelectionUIElements uIMoveSelection;
    [SerializeField] private PokemonInfoUIElements pokemonInfoUIElements;
    [SerializeField] private AttackInfoUIElements attackInfoUIElements;
    private UIController uIController;

    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uIInBattleParty = uIController.GetComponent<InBattlePartyUIElements>();
        uIMoveSelection = uIController.GetComponent <MoveSelectionUIElements>();
        pokemonInfoUIElements = uIController.GetComponent<PokemonInfoUIElements>();
        attackInfoUIElements = uIController.GetComponent<AttackInfoUIElements>();
        uIController.OnMenuChange += HandleMenuChange;
    }


    private void OnDisable()
    {

    }
    
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.InBattlePartyMenu)
        {
            UIEventSubscriptionManager.Subscribe(uIInBattleParty.BackButton, BackButtonClicked);
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            UIEventSubscriptionManager.Subscribe(uIMoveSelection.BackButton, BackButtonClicked);
        }
        else if (menu == Menus.PokemonInfoScreen)
        {
            UIEventSubscriptionManager.Subscribe(pokemonInfoUIElements.BackButton, BackButtonClicked);
        }
        else if (menu == Menus.OpposingPokemonInfoScreen)
        {
            UIEventSubscriptionManager.Subscribe(pokemonInfoUIElements.BackButton, BackButtonClicked);
        }
        else if (menu == Menus.AttackInfoScreen)
        {
            UIEventSubscriptionManager.Subscribe(attackInfoUIElements.BackButton, BackButtonClicked);
        }
    }

    private void BackButtonClicked()
    {

        if (uIController.GetCurrentMenu() == Menus.MoveSelectionMenu)
        {
            uIController.UpdateMenu(Menus.GeneralBattleMenu);
        }
        else
        {
            Menus? prevMenu = uIController.GetPreviousMenu()!;
            if (prevMenu != null)
            {
                uIController.UpdateMenu((Menus)prevMenu);
            }
        }
        
    }
}