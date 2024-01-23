using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIController : Singleton<UIController>
{
    [SerializeField] private UIToolkitElements uiElements;
    [SerializeField] public UIDocument currentUI;
    [SerializeField] public VisualTreeAsset generalBattleUI; // Your HP, Opponent HP, Fight Button, Pokemon Button, Forfiet Button
    [SerializeField] private UIDocument attackingUI;
    [SerializeField] public VisualTreeAsset teamUI;
    [SerializeField] private UIDocument pokemonInfoUI;
    [SerializeField] private UIDocument opposingPokemonInfoUI;
    [SerializeField] private UIDocument moveInfoUI;
    [SerializeField] private UIDocument forfietUI;

    private void OnEnable()
    {
        uiElements.FightButton.clicked += HandleUIInput;   
    }

    private void HandleUIInput()
    {
        string clickedButton = EventSystem.current.currentInputModule.name;
        Debug.Log(clickedButton);
    }
}