using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack3Controller : MoveSelectButton
{
    [SerializeField] TrainerController trainerController;
    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        UIController.OnMenuChange -= HandleMenuChange;
    }

    protected override void InitializeButton(Button attackButton)
    {
        attack = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[2];
        base.InitializeButton(attackButton);
    }

    protected override void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            InitializeButton(moveSelectionUIElements.Attack3Button);
            UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack3Button, OnAttackSelected);
        }
    }
}