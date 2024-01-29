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
            uIInBattleParty.BackButton.clicked += BackButtonClicked;
        }
        if (menu == Menus.MoveSelectionMenu)
        {
            uIMoveSelection.BackButton.clicked += BackButtonClicked;
        }
    }

    private void BackButtonClicked()
    {
        uIInBattleParty.BackButton.clicked -= BackButtonClicked;
        UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
    }
}