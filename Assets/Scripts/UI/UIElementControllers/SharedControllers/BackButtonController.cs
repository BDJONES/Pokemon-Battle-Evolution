using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BackButtonController : MonoBehaviour
{
    [SerializeField] private InBattlePartyUIElements uIInBattleParty;
    [SerializeField] private MoveSelectionUIElements uIMoveSelection;
    [SerializeField] private PokemonInfoUIElements pokemonInfoUIElements;
    [SerializeField] private AttackInfoUIElements attackInfoUIElements;

    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
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

        if (UIController.Instance.GetCurrentMenu() == Menus.MoveSelectionMenu)
        {
            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        }
        else
        {
            Menus? prevMenu = UIController.Instance.GetPreviousMenu()!;
            if (prevMenu != null)
            {
                UIController.Instance.UpdateMenu((Menus)prevMenu);
            }
        }
        
    }
}