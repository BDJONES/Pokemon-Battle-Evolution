using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class NoButtonController : MonoBehaviour
{
    [SerializeField] private ForfietUIElements forfietUIElements;
    private UIController uIController;

    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uIController.OnHostMenuChange += AssignProperties;
        uIController.OnClientMenuChange += AssignProperties;
    }

    private void OnDisable()
    {
        if (forfietUIElements.NoButton == null)
        {
            return;
        }
        uIController.OnHostMenuChange -= AssignProperties;
        uIController.OnClientMenuChange -= AssignProperties;
    }

    private void AssignProperties(Menus menu)
    {
        if (menu == Menus.ForfietMenu)
        {
            var player = transform.parent.parent.gameObject;
            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(forfietUIElements.NoButton, NoButtonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(forfietUIElements.NoButton, NoButtonClicked, 2);
            }
        }
    }

    private void NoButtonClicked()
    {
        Debug.Log("Clicked the No Button");
        var player = transform.parent.parent.gameObject;
        if (TrainerController.IsOwnerHost(player))
        {
            uIController.UpdateMenu(Menus.GeneralBattleMenu, 1);
        }
        else
        {
            uIController.UpdateMenu(Menus.GeneralBattleMenu, 2);
        }
        
    }
}
