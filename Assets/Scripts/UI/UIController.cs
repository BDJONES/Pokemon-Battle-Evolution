using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] public UIDocument currentUI; 
    [SerializeField] private VisualTreeAsset titleScreenUI;
    [SerializeField] private VisualTreeAsset loadingScreenUI;
    [SerializeField] private VisualTreeAsset generalBattleUI; // Your HP, Opponent HP, Fight Button, Pokemon Button, Forfiet Button
    [SerializeField] private VisualTreeAsset moveSelectUI;
    [SerializeField] private VisualTreeAsset teamUI;
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
    

    [SerializeField] private Menus? menu;
    public event Action<Menus> OnMenuChange;
    private Menus? prevMenu;

    private void OnEnable()
    {
        UpdateMenu(Menus.LoadingScreen);
        GameManager.OnStateChange += HandleStateChange;
        YourPokemonDeathEventManager.OnDeath += HandlePokemonDeath;
        LobbyManager.TwoPlayersConnected += HandleLobbyConnection;
    }

    private void HandleLobbyConnection()
    {
        
    }

    private void HandlePokemonDeath()
    {
        //Play Animation
        Debug.Log("Detected that Pokemon Died");
        //UpdateMenu(Menus.InBattlePartyMenu);
        //print($"Menu = {menu}");
    }

    private void HandleStateChange(GameState state)
    {
        if (state == GameState.BattleStart)
        {
            UpdateMenu(Menus.GeneralBattleMenu);
        }
    }

    private void Start()
    {
        UpdateMenu(Menus.TitleScreen);
    }

    //private void HandleUIInput()
    //{
    //    string clickedButton = EventSystem.current.currentInputModule.name;
    //    Debug.Log(clickedButton);
    //}

    public void UpdateMenu(Menus newMenu)
    {
        if (menu == null || menu != newMenu)
        {        
            UIEventSubscriptionManager.UnsubscribeAll();
            //Debug.Log($"Just Updated Menu to {newMenu}.\nOld menu was {menu}");
            prevMenu = menu;
            menu = newMenu;
            switch (newMenu)
            {
                case Menus.TitleScreen:
                    currentUI.rootVisualElement.style.display = DisplayStyle.None;
                    currentUI.visualTreeAsset = titleScreenUI;
                    currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                    break;
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
            }
            if (OnMenuChange != null && menu != null)
            {
                //Debug.Log("Invoked Change");
                OnMenuChange.Invoke(newMenu);
            }
        }
        else
        {
            //Debug.Log($"There was an error with {newMenu} being loaded. Most likely already on the same.");
        }
    }

    public Menus? GetCurrentMenu()
    {
        return menu;
    }
    public Menus? GetPreviousMenu()
    {
        return prevMenu;
    }
}