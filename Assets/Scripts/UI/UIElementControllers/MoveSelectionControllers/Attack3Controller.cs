using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack3Controller : MoveSelectButton
{
    [SerializeField] private TrainerController trainerController;
    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        moveSelectionUIElements = uIController.gameObject.GetComponent<MoveSelectionUIElements>();
        uIController.OnHostMenuChange += HandleMenuChange;
        uIController.OnClientMenuChange += HandleMenuChange;
        //trainerController = transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
    }

    private void OnDisable()
    {
        uIController.OnHostMenuChange -= HandleMenuChange;
        uIController.OnClientMenuChange -= HandleMenuChange;
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
            var player = transform.parent.parent.gameObject;
            InitializeButton(moveSelectionUIElements.Attack3Button);
            
            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack3Button, OnAttackSelected, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack3Button, OnAttackSelected, 2);
            }
        }
    }
}