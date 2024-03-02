using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack1Controller : MoveSelectButton
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
        attack = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[0];
        base.InitializeButton(attackButton);
    }

    protected override void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            
            InitializeButton(moveSelectionUIElements.Attack1Button);
            UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack1Button, OnAttackSelected);
        }
    }

}
