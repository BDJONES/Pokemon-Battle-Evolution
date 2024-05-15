using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class BackButtonController : NetworkBehaviour
{
    [SerializeField] private InBattlePartyUIElements uIInBattleParty;
    [SerializeField] private MoveSelectionUIElements uIMoveSelection;
    [SerializeField] private PokemonInfoUIElements pokemonInfoUIElements;
    [SerializeField] private AttackInfoUIElements attackInfoUIElements;
    private UIController uIController;

    private void OnEnable()
    {
        NetworkCommands.UIControllerCreated += () => 
        { 
            uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
            uIInBattleParty = uIController.GetComponent<InBattlePartyUIElements>();
            uIMoveSelection = uIController.GetComponent<MoveSelectionUIElements>();
            pokemonInfoUIElements = uIController.GetComponent<PokemonInfoUIElements>();
            attackInfoUIElements = uIController.GetComponent<AttackInfoUIElements>();
            uIController.OnHostMenuChange += HandleMenuChange;
            uIController.OnClientMenuChange += HandleMenuChange;
        };
    }


    private void OnDisable()
    {
        if (uIController != null)
        {
            uIController.OnHostMenuChange -= HandleMenuChange;
            uIController.OnClientMenuChange -= HandleMenuChange;
        }
    }
    
    private void HandleMenuChange(Menus menu)
    {
        var player = transform.parent.parent.gameObject;
        if (menu == Menus.InBattlePartyMenu)
        {
            if (IsHost)
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
            if (IsHost)
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
            if (IsHost)
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
            if (IsHost)
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
            if (IsHost)
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
        if (IsOwner && (uIController.GetCurrentTrainer1Menu() == Menus.MoveSelectionMenu || uIController.GetCurrentTrainer1Menu() == Menus.InBattlePartyDialogueScreen))
        {
            uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 1);
        }
        else if (IsOwner && (uIController.GetCurrentTrainer1Menu() != Menus.MoveSelectionMenu))
        {
            Menus? prevMenu = uIController.GetTrainer1PreviousMenu()!;
            if (prevMenu != null)
            {
                uIController.UpdateMenuRpc((Menus)prevMenu, 1);
            }
        }
        else if (!IsHost && (uIController.GetCurrentTrainer2Menu() == Menus.MoveSelectionMenu || uIController.GetCurrentTrainer2Menu() == Menus.InBattlePartyDialogueScreen))
        {
            uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 2);
        }
        else
        {
            Menus? prevMenu = uIController.GetTrainer2PreviousMenu()!;
            if (prevMenu != null)
            {
                uIController.UpdateMenuRpc((Menus)prevMenu, 2);
            }
        }
    }
}