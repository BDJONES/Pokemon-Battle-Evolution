using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIController : NetworkBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] public UIDocument currentUI; 
    //[SerializeField] private VisualTreeAsset titleScreenUI;
    [SerializeField] private VisualTreeAsset loadingScreenUI;
    [SerializeField] private VisualTreeAsset generalBattleUI; // Your HP, Opponent HP, Fight Button, Pokemon Button, Forfiet Button
    [SerializeField] private VisualTreeAsset moveSelectUI;
    [SerializeField] private VisualTreeAsset teamUI;
    [SerializeField] private VisualTreeAsset teamDialogueUI;
    [SerializeField] private VisualTreeAsset pokemonInfoUI;
    [SerializeField] private VisualTreeAsset opposingPokemonInfoUI;
    [SerializeField] private VisualTreeAsset attackInfoUI;
    [SerializeField] private VisualTreeAsset forfietUI;
    [SerializeField] private VisualTreeAsset pokemonDamagedUI;
    [SerializeField] private VisualTreeAsset dialogueUI;
    [SerializeField] private VisualTreeAsset opposingPokemonDamagedUI;
    [SerializeField] private VisualTreeAsset pokemonFaintedUI;
    [SerializeField] private VisualTreeAsset winScreenUI;
    [SerializeField] private VisualTreeAsset loseScreenUI;
    [SerializeField] private VisualTreeAsset blankScreenUI;
    [SerializeField] private Menus? trainer1Menu;
    [SerializeField] private Menus? trainer2Menu;
    public event Action<Menus> OnHostMenuChange;
    public event Action<Menus> OnClientMenuChange;
    private Menus? trainer1PrevMenu;
    private Menus? trainer2PrevMenu;

    private void OnEnable()
    {
        //UpdateMenuRpc(Menus.LoadingScreen);
        GameManager.OnStateChange += HandleStateChange;
        YourPokemonDeathEventManager.OnDeath += HandlePokemonDeath;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= HandleStateChange;
        YourPokemonDeathEventManager.OnDeath -= HandlePokemonDeath;
    }

    public override void OnDestroy()
    {
        UIEventSubscriptionManager.UnsubscribeAll(1);
        UIEventSubscriptionManager.UnsubscribeAll(2);
        base.OnDestroy();
    }

    private void HandlePokemonDeath()
    {
        //Play Animation
        Debug.Log("Detected that Pokemon Died");
    }

    private void HandleStateChange(GameState state)
    {
        if (state == GameState.BattleStart)
        {

        }
    }

    //private void Start()
    //{
    //    UpdateMenuRpc(Menus.LoadingScreen);
    //}

    //private void HandleUIInput()
    //{
    //    string clickedButton = EventSystem.current.currentInputModule.name;
    //    Debug.Log(clickedButton);
    //}

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateMenuClientRpc(Menus newMenu)
    {
        trainer2PrevMenu = trainer2Menu;
        trainer2Menu = newMenu;
        if (IsHost) return;
        UIEventSubscriptionManager.UnsubscribeAll(2);
        Debug.Log($"Just Updated Menu to {newMenu}.\nOld menu was {trainer2Menu}");

        switch (newMenu)
        {
            //case Menus.TitleScreen:
            //    currentUI.rootVisualElement.style.display = DisplayStyle.None;
            //    currentUI.visualTreeAsset = titleScreenUI;
            //    currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
            //    break;
            case Menus.LoadingScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = loadingScreenUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.GeneralBattleMenu:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = generalBattleUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.InBattlePartyMenu:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = teamUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.InBattlePartyDialogueScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = teamDialogueUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.MoveSelectionMenu:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = moveSelectUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.AttackInfoScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = attackInfoUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.ForfietMenu:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                //currentUI.visualTreeAsset = null;
                currentUI.visualTreeAsset = forfietUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.PokemonDamagedScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = pokemonDamagedUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.OpposingPokemonDamagedScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = opposingPokemonDamagedUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.DialogueScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = dialogueUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.PokemonFaintedScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = pokemonFaintedUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.PokemonInfoScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = pokemonInfoUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.OpposingPokemonInfoScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = opposingPokemonInfoUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.WinScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = winScreenUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.LoseScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = loseScreenUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.BlankScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = blankScreenUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
        }
        if (OnClientMenuChange != null && trainer2Menu != null)
        {
            //Debug.Log("Invoked Change");
            OnClientMenuChange.Invoke(newMenu);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateMenuHostRpc(Menus newMenu)
    {
        if (!IsHost) return;
        //Debug.Log("Updating the Host's menu");
        UIEventSubscriptionManager.UnsubscribeAll(1);
        Debug.Log($"Just Updated Menu to {newMenu}.\nOld menu was {trainer1Menu}");
        trainer1PrevMenu = trainer1Menu;
        trainer1Menu = newMenu;
        switch (newMenu)
        {
            //case Menus.TitleScreen:
            //    currentUI.rootVisualElement.style.display = DisplayStyle.None;
            //    currentUI.visualTreeAsset = titleScreenUI;
            //    currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
            //    break;
            case Menus.LoadingScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = loadingScreenUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.GeneralBattleMenu:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = generalBattleUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.InBattlePartyMenu:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = teamUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.InBattlePartyDialogueScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = teamDialogueUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.MoveSelectionMenu:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = moveSelectUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.AttackInfoScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = attackInfoUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.ForfietMenu:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = forfietUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                //Debug.Log("The Menu changed to the forfiet menu");
                break;
            case Menus.PokemonDamagedScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = pokemonDamagedUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.OpposingPokemonDamagedScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = opposingPokemonDamagedUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.DialogueScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = dialogueUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.PokemonFaintedScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = pokemonFaintedUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.PokemonInfoScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = pokemonInfoUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.OpposingPokemonInfoScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = opposingPokemonInfoUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.WinScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = winScreenUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.LoseScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = loseScreenUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.BlankScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = blankScreenUI;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
        }
        if (OnHostMenuChange != null && trainer1Menu != null)
        {
            //Debug.Log("Invoked Change");
            OnHostMenuChange.Invoke(newMenu);
        }
    }

    [Rpc(SendTo.Server)]
    public void UpdateMenuRpc(Menus newMenu, int type) // If 1 is a then we are calling the server. If it is 2, then we are calling the client
    {
        if ((trainer1Menu == null || trainer1Menu != newMenu) && type == 1)
        {
            UpdateMenuHostRpc(newMenu);
        }
        else if ((trainer2Menu == null || trainer2Menu != newMenu) && type == 2)
        {
            UpdateMenuClientRpc(newMenu);
        }
    }   

    public Menus? GetCurrentTrainer1Menu()
    {
        return trainer1Menu;
    }
    public Menus? GetCurrentTrainer2Menu()
    {
        return trainer2Menu;
    }
    public Menus? GetTrainer1PreviousMenu()
    {
        return trainer1PrevMenu;
    }
    public Menus? GetTrainer2PreviousMenu()
    {
        return trainer2PrevMenu;
    }
}