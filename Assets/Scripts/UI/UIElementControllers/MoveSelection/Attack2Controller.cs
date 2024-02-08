using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack2Controller : MoveSelectButton
{
    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
    }

    protected override void InitializeButton(Button attackButton)
    {
        attack = GameManager.Instance.trainer1.activePokemon.GetMoveset()[1];
        base.InitializeButton(attackButton);
    }

    protected override void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            InitializeButton(moveSelectionUIElements.Attack2Button);
            UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack2Button, OnAttackSelected);
        }
    }
}
