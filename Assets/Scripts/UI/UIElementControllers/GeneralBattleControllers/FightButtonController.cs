using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

public class FightButtonController : MonoBehaviour
{
    [SerializeField] private GeneralBattleUIElements uIElements;
    private UIController uIController;
    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uIElements = uIController.GetComponent<GeneralBattleUIElements>();
        uIController.OnMenuChange += HandleMenuChange;
    }



    private void OnDisable()
    {
        if (uIElements.PokemonButton == null)
        {
            return;
        }
        uIController.OnMenuChange -= HandleMenuChange;
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            UIEventSubscriptionManager.Subscribe(uIElements.FightButton, FightButtonClicked);
        }
    }

    private void FightButtonClicked()
    {
        //attack = GameManager.Instance.trainer1.GetActivePokemon().GetMoveset()[2]; //Earthquake
        //attack.PerformAction(GameManager.Instance.trainer1.GetActivePokemon(), GameManager.Instance.trainer2.GetActivePokemon());
        //OpposingPokemonInfoBarController opposingPokemonInfoBarController = GameObject.Find("UI Controller").GetComponent<OpposingPokemonInfoBarController>();
        //await opposingPokemonInfoBarController.UpdateHealthBar();
        //Debug.Log("Fight button Clicked");
        
        uIController.UpdateMenu(Menus.MoveSelectionMenu);
    }
}