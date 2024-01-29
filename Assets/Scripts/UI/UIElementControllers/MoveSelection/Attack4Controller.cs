using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack4Controller : MoveSelectButton
{
    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
    }

    protected override void InitializeButton(Button attackButton)
    {
        attack = GameManager.Instance.trainer1.activePokemon.GetMoveset()[3];
        base.InitializeButton(attackButton);
    }

    protected override void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            InitializeButton(moveSelectionUIElements.Attack4Button);
            moveSelectionUIElements.Attack4Button.clicked += OnAttackSelected;
        }
    }
}