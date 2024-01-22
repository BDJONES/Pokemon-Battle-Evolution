using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonButtonController : MonoBehaviour
{
    [SerializeField] private UIToolkitElements uiElements;
    private void OnEnable()
    {
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
    
    private void PokemonButtonClicked()
    {
        GameManager.Instance.trainer1.Switch(1);
    }
}