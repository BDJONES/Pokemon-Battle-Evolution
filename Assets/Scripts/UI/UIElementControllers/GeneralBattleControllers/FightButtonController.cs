using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
using Unity.Netcode;

public class FightButtonController : MonoBehaviour
{
    [SerializeField] private GeneralBattleUIElements uIElements;
    private UIController uIController;
    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uIElements = uIController.GetComponent<GeneralBattleUIElements>();
        uIController.OnHostMenuChange += HandleMenuChange;
        uIController.OnClientMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        if (uIElements.PokemonButton == null)
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
                UIEventSubscriptionManager.Subscribe(uIElements.FightButton, FightButtonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(uIElements.FightButton, FightButtonClicked, 2);
            }
        }
    }

    private void FightButtonClicked()
    {
        //attack = GameManager.Instance.trainer1.GetActivePokemon().GetMoveset()[2]; //Earthquake
        //attack.PerformAction(GameManager.Instance.trainer1.GetActivePokemon(), GameManager.Instance.trainer2.GetActivePokemon());
        //OpposingPokemonInfoBarController opposingPokemonInfoBarController = GameObject.Find("UI Controller").GetComponent<OpposingPokemonInfoBarController>();
        //await opposingPokemonInfoBarController.UpdateHealthBar();
        //Debug.Log("Fight button Clicked");
        Debug.Log("Clicked the Fight Button");
        UpdateUIForPlayer();
    }

    private void UpdateUIForPlayer()
    {
        var player = transform.parent.parent.gameObject;

        if (TrainerController.IsOwnerHost(player))
        {
            Debug.Log("I am a host");
            uIController.UpdateMenu(Menus.MoveSelectionMenu, 1);
        }
        else
        {
            Debug.Log("I am not a host");
            uIController.UpdateMenu(Menus.MoveSelectionMenu, 2);
        }
    }
}