using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIController : Singleton<UIController>
{
    [SerializeField] private GeneralBattleUIElements uiElements;
    [SerializeField] public UIDocument currentUI;
    [SerializeField] public VisualTreeAsset generalBattleUI; // Your HP, Opponent HP, Fight Button, Pokemon Button, Forfiet Button
    [SerializeField] public VisualTreeAsset moveSelectUI;
    [SerializeField] public VisualTreeAsset teamUI;
    [SerializeField] private UIDocument pokemonInfoUI;
    [SerializeField] private UIDocument opposingPokemonInfoUI;
    [SerializeField] private UIDocument moveInfoUI;
    [SerializeField] public VisualTreeAsset forfietUI;
    [SerializeField] private Menus menu;
    public static event Action<Menus> OnMenuChange;
    private void OnEnable()
    {
        //uiElements.FightButton.clicked += HandleUIInput;   
    }

    //private void HandleUIInput()
    //{
    //    string clickedButton = EventSystem.current.currentInputModule.name;
    //    Debug.Log(clickedButton);
    //}

    public void UpdateMenu(Menus newMenu)
    {
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
        }
        if (OnMenuChange != null)
        {
            OnMenuChange.Invoke(menu);
        }
    }
}