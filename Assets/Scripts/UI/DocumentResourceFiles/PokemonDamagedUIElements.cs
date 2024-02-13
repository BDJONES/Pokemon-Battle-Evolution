using UnityEngine;
using UnityEngine.UIElements;

public class PokemonDamagedUIElements: MonoBehaviour
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
}