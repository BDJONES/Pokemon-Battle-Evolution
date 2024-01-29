using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack1Controller : MoveSelectButton
{
    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
    }

    protected override void InitializeButton(Button attackButton)
    {
        attack = GameManager.Instance.trainer1.activePokemon.GetMoveset()[0];
        base.InitializeButton(attackButton);
    }

    protected override void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            InitializeButton(moveSelectionUIElements.Attack1Button);
            moveSelectionUIElements.Attack1Button.clicked += OnAttackSelected;
        }
    }

}
