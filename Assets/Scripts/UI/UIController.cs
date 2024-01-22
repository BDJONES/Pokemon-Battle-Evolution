using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIToolkitElements uiElements;
    [SerializeField] private UIDocument mainUI;
    [SerializeField] private UIDocument attackingUI;
    [SerializeField] private UIDocument teamUI;
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