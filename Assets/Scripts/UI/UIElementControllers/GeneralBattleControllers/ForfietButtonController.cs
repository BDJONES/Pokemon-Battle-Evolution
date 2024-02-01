using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

public class ForfietButtonController : MonoBehaviour
{
	[SerializeField] private GeneralBattleUIElements uiElements;
	private void OnEnable()
	{
		UIController.OnMenuChange += HandleMenuChange;
        UIEventSubscriptionManager.Subscribe(uiElements.ForfietButton, ForfietButtonClicked);
    }



    private void OnDisable()
	{
		if (uiElements.PokemonButton == null)
		{
			return;
		}
		
	}
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
		{
            UIEventSubscriptionManager.Subscribe(uiElements.ForfietButton, ForfietButtonClicked);
        }
    }

	private void ForfietButtonClicked()
	{
        UIController.Instance.UpdateMenu(Menus.ForfietMenu);
    }
}