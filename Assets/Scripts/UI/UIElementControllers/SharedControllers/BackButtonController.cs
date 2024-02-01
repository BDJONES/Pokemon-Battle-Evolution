using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BackButtonController : MonoBehaviour
{
    [SerializeField] private InBattlePartyUIElements uIInBattleParty;
    [SerializeField] private MoveSelectionUIElements uIMoveSelection;

    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
    }


    private void OnDisable()
    {

    }
    
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.InBattlePartyMenu)
        {
            UIEventSubscriptionManager.Subscribe(uIInBattleParty.BackButton, BackButtonClicked);
        }
        if (menu == Menus.MoveSelectionMenu)
        {
            UIEventSubscriptionManager.Subscribe(uIMoveSelection.BackButton, BackButtonClicked);
        }
    }

    private void BackButtonClicked()
    {
        UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
    }
}