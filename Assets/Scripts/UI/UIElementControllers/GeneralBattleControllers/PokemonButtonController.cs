using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonButtonController : MonoBehaviour
{
    [SerializeField] private GeneralBattleUIElements uiElements;
    private UIController uIController;
    
    private void OnEnable()
    {
        uIController = transform.parent.gameObject.GetComponentInChildren<UIController>();
        uIController.OnMenuChange += HandleMenuChange;
        //UIEventSubscriptionManager.Subscribe(uiElements.PokemonButton, PokemonButtonClicked);
    }



    private void OnDisable()
    {
        if (uiElements.PokemonButton == null)
        {
            return;
        }
        uIController.OnMenuChange -= HandleMenuChange;
    }
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            UIEventSubscriptionManager.Subscribe(uiElements.PokemonButton, PokemonButtonClicked);
        }
    }
    private void PokemonButtonClicked()
    {
        uIController.UpdateMenu(Menus.InBattlePartyMenu);
    }
}