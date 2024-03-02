using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonInfoUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uIDocument;

    public VisualElement PokemonInfo
    {
        get
        {
            if (uIDocument.rootVisualElement == null)
            {
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement topElements = screen.Query<VisualElement>("TopElements");
                if (topElements != null)
                {
                    //Debug.Log("Found the top elements");
                    VisualElement pokemonInfo = topElements.Query<VisualElement>("PokemonInfo");
                    if (pokemonInfo != null)
                    {
                        //Debug.Log("Found the PokemonInfo");
                        return pokemonInfo;
                    }
                }
            }
            Debug.Log("Unable to find PokemonInfo VisualElement");
            return null;
        }
    }
    public VisualElement StatStages
    {
        get
        {
            if (uIDocument.rootVisualElement == null)
            {
                Debug.Log("The root was null");
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement topElements = screen.Query<VisualElement>("TopElements");
                if (topElements != null)
                {
                    VisualElement statStages = topElements.Query<VisualElement>("StatStages");
                    if (statStages != null)
                    {
                        return statStages;
                    }
                }
            }
            Debug.Log("Unable to find the StatStages VisualElements");
            return null;
        }
    }
    public TemplateContainer PokemonImage
    {
        get
        {
            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement topElements = screen.Query<VisualElement>("BottomElements");
                if (topElements != null)
                {
                    TemplateContainer statStages = topElements.Query<TemplateContainer>("PokemonImageContainer");
                    if (statStages != null)
                    {
                        return statStages;
                    }
                }
            }
            return null;
        }
    }

    public Button BackButton
    {
        get
        {
            if (uIDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            TemplateContainer backButtonElement = screen.Query<TemplateContainer>("Back_Button");
            VisualElement backButtonContent = backButtonElement.Query<VisualElement>("Content");
            Button backButton = backButtonContent.Query<Button>("BackButton");
            return backButton;
        }
    }

    public VisualElement StatusButtons
    {
        get
        {
            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement topElements = screen.Query<VisualElement>("BottomElements");
                if (topElements != null)
                {
                    VisualElement statStages = topElements.Query<VisualElement>("StatusButtons");
                    if (statStages != null)
                    {
                        return statStages;
                    }
                }
            }
            return null;
        }
    }
}
