using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIController : Singleton<UIController>
{
    [SerializeField] public UIDocument currentUI;
    [SerializeField] public VisualTreeAsset generalBattleUI; // Your HP, Opponent HP, Fight Button, Pokemon Button, Forfiet Button
    [SerializeField] public VisualTreeAsset moveSelectUI;
    [SerializeField] public VisualTreeAsset teamUI;
    [SerializeField] private UIDocument pokemonInfoUI;
    [SerializeField] private UIDocument opposingPokemonInfoUI;
    [SerializeField] private UIDocument moveInfoUI;
    [SerializeField] public VisualTreeAsset forfietUI;
    [SerializeField] public VisualTreeAsset pokemonDamagedScreen;
    [SerializeField] public VisualTreeAsset dialogueScreen;
    [SerializeField] public VisualTreeAsset opposingPokemonDamagedScreen;
    [SerializeField] private Menus menu;
    public static event Action<Menus> OnMenuChange;
    private void OnEnable()
    {
        GameManager.OnStateChange += HandleStateChange;
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
        UIEventSubscriptionManager.UnsubscribeAll();
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
                currentUI.visualTreeAsset = pokemonDamagedScreen;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.OpposingPokemonDamagedScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = opposingPokemonDamagedScreen;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
            case Menus.DialogueScreen:
                currentUI.rootVisualElement.style.display = DisplayStyle.None;
                currentUI.visualTreeAsset = dialogueScreen;
                currentUI.rootVisualElement.style.display = DisplayStyle.Flex;
                break;
        }
        if (OnMenuChange != null)
        {
            OnMenuChange.Invoke(menu);
        }
    }
}