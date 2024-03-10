using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack4Controller : MoveSelectButton
{
    [SerializeField] TrainerController trainerController;
    private void OnEnable()
    {
        uIController = transform.parent.gameObject.GetComponentInChildren<UIController>();
        uIController.OnMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        uIController.OnMenuChange -= HandleMenuChange;
    }

    protected override void InitializeButton(Button attackButton)
    {
        attack = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[3];
        base.InitializeButton(attackButton);
    }

    protected override void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            InitializeButton(moveSelectionUIElements.Attack4Button);
            UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack4Button, OnAttackSelected);
        }
    }
}