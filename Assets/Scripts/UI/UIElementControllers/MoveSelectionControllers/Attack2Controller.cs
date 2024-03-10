using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack2Controller : MoveSelectButton
{
    [SerializeField] TrainerController trainerController;
    private void OnEnable()
    {
        uIController = transform.parent.gameObject.GetComponentInChildren<UIController>();
        trainerController = transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
        uIController.OnMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        uIController.OnMenuChange -= HandleMenuChange;
    }

    protected override void InitializeButton(Button attackButton)
    {
        attack = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[1];
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
