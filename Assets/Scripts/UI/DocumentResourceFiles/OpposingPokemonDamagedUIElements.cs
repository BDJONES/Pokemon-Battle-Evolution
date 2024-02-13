using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class OpposingPokemonDamagedUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
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
}