using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class NoButtonController : NetworkBehaviour
{
    [SerializeField] private ForfietUIElements forfietUIElements;
    private UIController uIController;

    private void OnEnable()
    {
        //if (IsOwner)
        //{
            NetworkCommands.UIControllerCreated += () =>
            {
                uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
                uIController.OnHostMenuChange += AssignProperties;
                uIController.OnClientMenuChange += AssignProperties;
            };
        //}

    }

    private void OnDisable()
    {
        if (uIController != null)
        {
            if (forfietUIElements.NoButton == null)
            {
                return;
            }
            uIController.OnHostMenuChange -= AssignProperties;
            uIController.OnClientMenuChange -= AssignProperties;
        }
    }

    private void AssignProperties(Menus menu)
    {
        if (menu == Menus.ForfietMenu)
        {
            var player = transform.parent.parent.gameObject;
            if (IsHost)
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
        if (IsHost)
        {
            uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 1);
        }
        else
        {
            uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 2);
        }
        
    }
}
