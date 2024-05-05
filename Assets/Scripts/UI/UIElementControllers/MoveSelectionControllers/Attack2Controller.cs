using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack2Controller : MoveSelectButton
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
                //trainerController = transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
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
        attack = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[1];
        base.InitializeButton(attackButton);
    }

    protected override void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            var player = transform.parent.parent.gameObject;
            InitializeButton(moveSelectionUIElements.Attack2Button);
            
            if (IsHost)
            {
                UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack2Button, OnAttackSelected, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack2Button, OnAttackSelected, 2);
            }
        }
    }
}
