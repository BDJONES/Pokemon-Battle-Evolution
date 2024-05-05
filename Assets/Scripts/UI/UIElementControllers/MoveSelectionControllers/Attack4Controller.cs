using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack4Controller : MoveSelectButton
{
    [SerializeField] TrainerController trainerController;
    private void OnEnable()
    {
        //if (IsOwner)
        //{
            NetworkCommands.UIControllerCreated += () =>
            {
                uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
                moveSelectionUIElements = uIController.gameObject.GetComponent<MoveSelectionUIElements>();
                uIController.OnHostMenuChange += HandleMenuChange;
                uIController.OnClientMenuChange += HandleMenuChange;
            };
        //}
    }

    private void OnDisable()
    {
        if (uIController != null)
        {
            uIController.OnHostMenuChange -= HandleMenuChange;
            uIController.OnClientMenuChange -= HandleMenuChange;
        }
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
            var player = transform.parent.parent.gameObject;
            InitializeButton(moveSelectionUIElements.Attack4Button);

            if (IsHost)
            {
                UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack4Button, OnAttackSelected, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack4Button, OnAttackSelected, 2);
            }
            
        }
    }
}