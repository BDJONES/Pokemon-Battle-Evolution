using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoButtonController : MonoBehaviour
{
    [SerializeField] private ForfietUIElements forfietUIElements;
    private UIController uIController;

    private void OnEnable()
    {
        uIController = transform.parent.gameObject.GetComponentInChildren<UIController>();
        uIController.OnMenuChange += AssignProperties;
    }

    private void OnDisable()
    {
        //if (forfietUIElements.NoButton == null)
        //{
        //    return;
        //}
        uIController.OnMenuChange -= AssignProperties;
    }

    private void AssignProperties(Menus menu)
    {
        if (menu == Menus.ForfietMenu)
        {
            UIEventSubscriptionManager.Subscribe(forfietUIElements.NoButton, NoButtonClicked);
        }
    }

    private void NoButtonClicked()
    {
        forfietUIElements.NoButton.clicked -= NoButtonClicked;
        uIController.UpdateMenu(Menus.GeneralBattleMenu);
    }
}
