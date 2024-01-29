using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonButtonController : MonoBehaviour
{
    [SerializeField] private GeneralBattleUIElements uiElements;
    
    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;
        uiElements.PokemonButton.clicked += PokemonButtonClicked;
    }



    private void OnDisable()
    {
        if (uiElements.PokemonButton == null)
        {
            return;
        }
        uiElements.PokemonButton.clicked -= PokemonButtonClicked;
    }
    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.GeneralBattleMenu)
        {
            uiElements.PokemonButton.clicked += PokemonButtonClicked;
        }
    }
    private void PokemonButtonClicked()
    {
        UIController.Instance.UpdateMenu(Menus.InBattlePartyMenu);
        //GameManager.Instance.trainer1.Switch(1);
    }
}