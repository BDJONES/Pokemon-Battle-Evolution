using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FightButtonController : MonoBehaviour
{
    [SerializeField] private UIToolkitElements uiElements;
    Attack attack;
    private void OnEnable()
    {
        
        uiElements.FightButton.clicked += FightButtonClicked;
    }



    private void OnDisable()
    {
        if (uiElements.PokemonButton == null)
        {
            return;
        }
        uiElements.FightButton.clicked -= FightButtonClicked;
    }

    private async void FightButtonClicked()
    {
        attack = GameManager.Instance.trainer1.activePokemon.GetMoveset()[2]; //Earthquake
        attack.PerformAction(GameManager.Instance.trainer1.activePokemon, GameManager.Instance.trainer2.activePokemon);
        OpposingPokemonInfoBarController opposingPokemonInfoBarController = GameObject.Find("UI Controller").GetComponent<OpposingPokemonInfoBarController>();
        await opposingPokemonInfoBarController.UpdateHealthBar();
        Debug.Log("Fight button Clicked");
    }
}