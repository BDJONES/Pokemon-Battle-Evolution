using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack1Controller : MoveSelectButton
{
    [SerializeField] TrainerController trainerController;
    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        moveSelectionUIElements = uIController.gameObject.GetComponent<MoveSelectionUIElements>();
        //trainerController = transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
        uIController.OnHostMenuChange += HandleMenuChange;
        uIController.OnClientMenuChange += HandleMenuChange;
    }

    private void OnDisable()
    {
        uIController.OnHostMenuChange -= HandleMenuChange;
        uIController.OnClientMenuChange -= HandleMenuChange;
    }

    protected override void InitializeButton(Button attackButton)
    {
        //Debug.Log(trainerController.GetPlayer().GetActivePokemon().GetMoveset()[0].GetAttackName());
        attack = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[0];
        base.InitializeButton(attackButton);
    }

    protected override void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            var player = transform.parent.parent.gameObject;
            InitializeButton(moveSelectionUIElements.Attack1Button);
            
            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack1Button, OnAttackSelected, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack1Button, OnAttackSelected, 2);
            }
        }
    }
}
