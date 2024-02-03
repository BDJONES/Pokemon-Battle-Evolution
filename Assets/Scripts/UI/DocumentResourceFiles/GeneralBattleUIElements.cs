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
            if (screen != null)
            {
                VisualElement content = screen.Query<VisualElement>("Content");
                if (content != null)
                {
                    VisualElement trainerInfo = content.Query<VisualElement>("TrainerInfo");
                    if (trainerInfo != null)
                    {
                        TemplateContainer pokemonInfoBarWidget = trainerInfo.Query<TemplateContainer>("PokemonInfoBar");
                        if (pokemonInfoBarWidget != null)
                        {
                            VisualElement pokemonInfoBar = pokemonInfoBarWidget.Query<VisualElement>("PokemonInfo");
                            if (pokemonInfoBar != null)
                            {
                                return pokemonInfoBar;
                            }
                        }
                    }
                }
            }
            return null;
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
            if (screen != null)
            {
                VisualElement content = screen.Query<VisualElement>("Content");
                if (content != null)
                {
                    VisualElement opposingTrainerInfo = content.Query<VisualElement>("OpposingTrainerInfo");
                    if (opposingTrainerInfo != null)
                    {
                        TemplateContainer opposingPokemonInfoBarWidget = opposingTrainerInfo.Query<TemplateContainer>("OpposingPokemonInfoBar");
                        if (opposingPokemonInfoBarWidget != null)
                        {
                            VisualElement opposingPokemonInfoBar = opposingPokemonInfoBarWidget.Query<VisualElement>("OpposingPokemonInfo");
                            if (opposingPokemonInfoBar != null)
                            {
                                return opposingPokemonInfoBar;
                            }
                        }
                    }
                }
            }
            return null;
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
            if (screen != null)
            {
                VisualElement content = screen.Query<VisualElement>("Content");
                if (content != null)
                {
                    VisualElement userActions = content.Query<VisualElement>("UserActions");
                    if (userActions != null)
                    {
                        VisualElement pfButtons = userActions.Query<VisualElement>("Pokemon_And_Forfiet_Buttons");
                        if (pfButtons != null)
                        {
                            Button pokemonButton = pfButtons.Query<Button>("PokemonButton");
                            if (pokemonButton != null)
                            {
                                return pokemonButton;
                            }
                        }
                    }
                }
            }
            return null;
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
