using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using Unity.Netcode;

public class ForfietButtonController : NetworkBehaviour
{
	[SerializeField] private GeneralBattleUIElements uIElements;
	private UIController uIController;
	private void OnEnable()
	{
        NetworkCommands.UIControllerCreated += () =>
        {
            uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
            uIController.OnHostMenuChange += HandleMenuChange;
            uIController.OnClientMenuChange += HandleMenuChange;
            uIElements = uIController.GetComponent<GeneralBattleUIElements>();
        };

        //UIEventSubscriptionManager.Subscribe(uiElements.ForfietButton, ForfietButtonClicked);
    }



    private void OnDisable()
	{
        if (uIController != null)
        {
		    if (uIElements.PokemonButton == null)
		    {
			    return;
		    }
            uIController.OnHostMenuChange -= HandleMenuChange;
            uIController.OnClientMenuChange -= HandleMenuChange;
        }

    }
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
		{
            var player = transform.parent.parent.gameObject;

            if (IsHost)
            {
                UIEventSubscriptionManager.Subscribe(uIElements.ForfietButton, ForfietButtonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(uIElements.ForfietButton, ForfietButtonClicked, 2);
            }           
        }
    }

	private void ForfietButtonClicked()
	{
        var player = transform.parent.parent.gameObject;

        if (IsHost)
        {
            uIController.UpdateMenuRpc(Menus.ForfietMenu, 1);
        }
        else
        {
            uIController.UpdateMenuRpc(Menus.ForfietMenu, 2);
        }
    }
}