using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

public class FightButtonController : MonoBehaviour
{
    [SerializeField] private GeneralBattleUIElements uiElements;
    Attack attack;
    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
    }



    private void OnDisable()
    {
        if (uiElements.PokemonButton == null)
        {
            return;
        }
        
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            UIEventSubscriptionManager.Subscribe(uiElements.FightButton, FightButtonClicked);
        }
    }

    private void FightButtonClicked()
    {
        //attack = GameManager.Instance.trainer1.activePokemon.GetMoveset()[2]; //Earthquake
        //attack.PerformAction(GameManager.Instance.trainer1.activePokemon, GameManager.Instance.trainer2.activePokemon);
        //OpposingPokemonInfoBarController opposingPokemonInfoBarController = GameObject.Find("UI Controller").GetComponent<OpposingPokemonInfoBarController>();
        //await opposingPokemonInfoBarController.UpdateHealthBar();
        //Debug.Log("Fight button Clicked");
        
        UIController.Instance.UpdateMenu(Menus.MoveSelectionMenu);
    }
}