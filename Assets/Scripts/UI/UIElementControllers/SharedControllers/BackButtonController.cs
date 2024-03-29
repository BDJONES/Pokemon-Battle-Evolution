using System;
using Unity.Netcode;
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
        uIController.OnHostMenuChange += HandleMenuChange;
        uIController.OnClientMenuChange += HandleMenuChange;
    }


    private void OnDisable()
    {
        uIController.OnHostMenuChange -= HandleMenuChange;
        uIController.OnClientMenuChange -= HandleMenuChange;
    }
    
    private void HandleMenuChange(Menus menu)
    {
        var player = transform.parent.parent.gameObject;
        if (menu == Menus.InBattlePartyMenu)
        {
            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(uIInBattleParty.BackButton, BackButtonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(uIInBattleParty.BackButton, BackButtonClicked, 2);
            }
        }
        else if (menu == Menus.MoveSelectionMenu)
        {
            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(uIMoveSelection.BackButton, BackButtonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(uIMoveSelection.BackButton, BackButtonClicked, 2);
            }
        }
        else if (menu == Menus.PokemonInfoScreen)
        {
            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(pokemonInfoUIElements.BackButton, BackButtonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(pokemonInfoUIElements.BackButton, BackButtonClicked, 2);
            }
        }
        else if (menu == Menus.OpposingPokemonInfoScreen)
        {
            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(pokemonInfoUIElements.BackButton, BackButtonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(pokemonInfoUIElements.BackButton, BackButtonClicked, 2);
            }
        }
        else if (menu == Menus.AttackInfoScreen)
        {
            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(attackInfoUIElements.BackButton, BackButtonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(attackInfoUIElements.BackButton, BackButtonClicked, 2);
            }
        }
    }

    private void BackButtonClicked()
    {
        var player = transform.parent.parent.gameObject;
        if (TrainerController.IsOwnerHost(player) && uIController.GetCurrentTrainer1Menu() == Menus.MoveSelectionMenu)
        {
            uIController.UpdateMenu(Menus.GeneralBattleMenu, 1);
        }
        else if (TrainerController.IsOwnerHost(player) && (uIController.GetCurrentTrainer1Menu() != Menus.MoveSelectionMenu))
        {
            Menus? prevMenu = uIController.GetTrainer1PreviousMenu()!;
            if (prevMenu != null)
            {
                uIController.UpdateMenu((Menus)prevMenu, 1);
            }
        }
        else if (!TrainerController.IsOwnerHost(player) && uIController.GetCurrentTrainer2Menu() == Menus.MoveSelectionMenu)
        {
            uIController.UpdateMenu(Menus.GeneralBattleMenu, 2);
        }
        else
        {
            Menus? prevMenu = uIController.GetTrainer2PreviousMenu()!;
            if (prevMenu != null)
            {
                uIController.UpdateMenu((Menus)prevMenu, 2);
            }
        }
    }
}