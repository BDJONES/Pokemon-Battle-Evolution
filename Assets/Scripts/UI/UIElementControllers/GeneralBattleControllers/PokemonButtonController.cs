using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonButtonController : MonoBehaviour
{
    [SerializeField] private GeneralBattleUIElements uiElements;
    private UIController uIController;
    
    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uiElements = uIController.GetComponent<GeneralBattleUIElements>();
        uIController.OnHostMenuChange += HandleMenuChange;
        uIController.OnClientMenuChange += HandleMenuChange;
        //UIEventSubscriptionManager.Subscribe(uiElements.PokemonButton, PokemonButtonClicked);
    }



    private void OnDisable()
    {
        if (uiElements.PokemonButton == null)
        {
            return;
        }
        uIController.OnHostMenuChange -= HandleMenuChange;
        uIController.OnClientMenuChange -= HandleMenuChange;
    }
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            var player = transform.parent.parent.gameObject;

            if (TrainerController.IsOwnerHost(player))
            {
                //Debug.Log("I am a host");
                UIEventSubscriptionManager.Subscribe(uiElements.PokemonButton, PokemonButtonClicked, 1);
            }
            else
            {
                //Debug.Log("I am not a host");
                UIEventSubscriptionManager.Subscribe(uiElements.PokemonButton, PokemonButtonClicked, 2);
            }
        }
    }
    private void PokemonButtonClicked()
    {
        var player = transform.parent.parent.gameObject;

        if (TrainerController.IsOwnerHost(player))
        {
            //Debug.Log("I am a host");
            uIController.UpdateMenu(Menus.InBattlePartyMenu, 1);
        }
        else
        {
            //Debug.Log("I am not a host");
            uIController.UpdateMenu(Menus.InBattlePartyMenu, 2);
        }
    }
}