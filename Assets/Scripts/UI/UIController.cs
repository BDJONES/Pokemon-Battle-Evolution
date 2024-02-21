using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIController : Singleton<UIController>
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] public UIDocument currentUI;
    [SerializeField] public VisualTreeAsset generalBattleUI; // Your HP, Opponent HP, Fight Button, Pokemon Button, Forfiet Button
    [SerializeField] public VisualTreeAsset moveSelectUI;
    [SerializeField] public VisualTreeAsset teamUI;
    [SerializeField] private UIDocument pokemonInfoUI;
    [SerializeField] private UIDocument opposingPokemonInfoUI;
    [SerializeField] private UIDocument moveInfoUI;
    [SerializeField] public VisualTreeAsset forfietUI;
    [SerializeField] public VisualTreeAsset pokemonDamagedUI;
    [SerializeField] public VisualTreeAsset dialogueUI;
    [SerializeField] public VisualTreeAsset opposingPokemonDamagedUI;
    [SerializeField] public VisualTreeAsset pokemonFaintedUI;
    [SerializeField] private Menus? menu;
    public static event Action<Menus> OnMenuChange;

    private void OnEnable()
    {
        GameManager.OnStateChange += HandleStateChange;
        YourPokemonDeathEventManager.OnDeath += HandlePokemonDeath;
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
            menu = newMenu;
            switch (newMenu)
            {
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
}