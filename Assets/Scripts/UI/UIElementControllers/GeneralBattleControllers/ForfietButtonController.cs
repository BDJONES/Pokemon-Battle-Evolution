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
        uiElements.FightButton.clicked += ForfietButtonClicked;
    }



    private void OnDisable()
	{
		if (uiElements.PokemonButton == null)
		{
			return;
		}
		uiElements.ForfietButton.clicked -= ForfietButtonClicked;
	}
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
		{
            uiElements.ForfietButton.clicked += ForfietButtonClicked;
        }
    }

	private void ForfietButtonClicked()
	{
        UIController.Instance.UpdateMenu(Menus.ForfietMenu);
    }
}