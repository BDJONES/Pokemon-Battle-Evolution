using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;
using Unity.Netcode;

public class ForfietButtonController : MonoBehaviour
{
	[SerializeField] private GeneralBattleUIElements uIElements;
	private UIController uIController;
	private void OnEnable()
	{
		uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uIController.OnHostMenuChange += HandleMenuChange;
        uIController.OnClientMenuChange += HandleMenuChange;
        uIElements = uIController.GetComponent<GeneralBattleUIElements>();
        //UIEventSubscriptionManager.Subscribe(uiElements.ForfietButton, ForfietButtonClicked);
    }



    private void OnDisable()
	{
		if (uIElements.PokemonButton == null)
		{
			return;
		}
        uIController.OnHostMenuChange -= HandleMenuChange;
        uIController.OnClientMenuChange -= HandleMenuChange;
    }
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
		{
            var player = transform.parent.parent.gameObject;

            if (TrainerController.IsOwnerHost(player))
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

        if (TrainerController.IsOwnerHost(player))
        {
            uIController.UpdateMenu(Menus.ForfietMenu, 1);
        }
        else
        {
            uIController.UpdateMenu(Menus.ForfietMenu, 2);
        }
    }
}