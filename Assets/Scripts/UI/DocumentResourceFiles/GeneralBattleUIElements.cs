using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneralBattleUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    public VisualElement PokemonInfoBar
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            VisualElement screen = uiDocument.rootVisualElement.Query<VisualElement>("Screen");
            VisualElement content = screen.Query<VisualElement>("Content");
            VisualElement trainerInfo = content.Query<VisualElement>("TrainerInfo");
            TemplateContainer pokemonInfoBarWidget = trainerInfo.Query<TemplateContainer>("PokemonInfoBar");
            VisualElement pokemonInfoBar = pokemonInfoBarWidget.Query<VisualElement>("PokemonInfo");
            return pokemonInfoBar;
        }
    }
    public VisualElement OpposingPokemonInfoBar
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            VisualElement screen = uiDocument.rootVisualElement.Query<VisualElement>("Screen");
            VisualElement content = screen.Query<VisualElement>("Content");
            VisualElement opposingTrainerInfo = content.Query<VisualElement>("OpposingTrainerInfo");
            TemplateContainer opposingPokemonInfoBarWidget = opposingTrainerInfo.Query<TemplateContainer>("OpposingPokemonInfoBar");
            VisualElement opposingPokemonInfoBar = opposingPokemonInfoBarWidget.Query<VisualElement>("OpposingPokemonInfo");
            return opposingPokemonInfoBar;
        }
    }
    public Button FightButton
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            return uiDocument.rootVisualElement.Query<Button>("FightButton");
        }
    }
    public Button PokemonButton
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                //Debug.Log("The root is null");
                return null;
            }
            VisualElement screen = uiDocument.rootVisualElement.Query<VisualElement>("Screen");
            VisualElement content = screen.Query<VisualElement>("Content");
            VisualElement userActions = content.Query<VisualElement>("UserActions");
            VisualElement pfButtons = userActions.Query<VisualElement>("Pokemon_And_Forfiet_Buttons");
            Button pokemonButton = pfButtons.Query<Button>("PokemonButton");
            return pokemonButton;

            
        }
    }
    public Button ForfietButton
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            return uiDocument.rootVisualElement.Query<Button>("ForfietButton");
        }
    }
    
}
