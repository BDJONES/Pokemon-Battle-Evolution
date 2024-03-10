using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

public class ForfietButtonController : MonoBehaviour
{
	[SerializeField] private GeneralBattleUIElements uIElements;
	private UIController uIController;
	private void OnEnable()
	{
		uIController = transform.parent.gameObject.GetComponentInChildren<UIController>();
        uIController.OnMenuChange += HandleMenuChange;
		uIElements = uIController.GetComponent<GeneralBattleUIElements>();
        //UIEventSubscriptionManager.Subscribe(uiElements.ForfietButton, ForfietButtonClicked);
    }



    private void OnDisable()
	{
		if (uIElements.PokemonButton == null)
		{
			return;
		}
		uIController.OnMenuChange -= HandleMenuChange;
	}
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
		{
            UIEventSubscriptionManager.Subscribe(uIElements.ForfietButton, ForfietButtonClicked);
        }
    }

	private void ForfietButtonClicked()
	{
        uIController.UpdateMenu(Menus.ForfietMenu);
    }
}