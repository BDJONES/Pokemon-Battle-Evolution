using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoButtonController : MonoBehaviour
{
    [SerializeField] private ForfietUIElements forfietUIElements;

    private void OnEnable()
    {
        UIController.OnMenuChange += AssignProperties;
    }

    private void OnDisable()
    {
        if (forfietUIElements.NoButton == null)
        {
            return;
        }
        UIController.OnMenuChange -= AssignProperties;
    }

    private void AssignProperties(Menus menu)
    {
        if (menu == Menus.ForfietMenu)
        {
            forfietUIElements.NoButton.clicked += NoButtonClicked;
        }
    }

    private void NoButtonClicked()
    {
        forfietUIElements.NoButton.clicked -= NoButtonClicked;
        UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
    }
}
