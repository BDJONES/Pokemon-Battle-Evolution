using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class MoveSelectionUIElements : MonoBehaviour
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
            return opposingTrainerInfo;
        }
    }

    public Button BackButton
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            TemplateContainer backButtonElement = attackVE.Query<TemplateContainer>("Back_Button");
            VisualElement backButtonContent = backButtonElement.Query<VisualElement>("Content");
            Button backButton = backButtonContent.Query<Button>("BackButton");
            return backButton;
        }
    }
    public Button Attack1Button
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            VisualElement leftAttacks = attackVE.Query<VisualElement>("LeftAttacks");
            TemplateContainer attack1Element = leftAttacks.Query<TemplateContainer>("Attack1");
            VisualElement attack1Content = attack1Element.Query<VisualElement>("Content");
            Button attack1Button = attack1Content.Query<Button>("AttackButton");
            return attack1Button;
        }
    }
    public Button Attack1InfoButton
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            VisualElement leftAttacks = attackVE.Query<VisualElement>("LeftAttacks");
            TemplateContainer attack1Element = leftAttacks.Query<TemplateContainer>("Attack1");
            VisualElement attack1Content = attack1Element.Query<VisualElement>("Content");
            Button attack1InfoButton = attack1Content.Query<Button>("AttackInfoButton");
            return attack1InfoButton;
        }
    }
    public Button Attack2Button
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            VisualElement leftAttacks = attackVE.Query<VisualElement>("RightAttacks");
            TemplateContainer attack2Element = leftAttacks.Query<TemplateContainer>("Attack2");
            VisualElement attack2Content = attack2Element.Query<VisualElement>("Content");
            Button attack2Button = attack2Content.Query<Button>("AttackButton");
            return attack2Button;
        }
    }
    public Button Attack2InfoButton
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            VisualElement leftAttacks = attackVE.Query<VisualElement>("RightAttacks");
            TemplateContainer attack2Element = leftAttacks.Query<TemplateContainer>("Attack2");
            VisualElement attack2Content = attack2Element.Query<VisualElement>("Content");
            Button attack2InfoButton = attack2Content.Query<Button>("AttackInfoButton");
            return attack2InfoButton;
        }
    }
    public Button Attack3Button
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            VisualElement leftAttacks = attackVE.Query<VisualElement>("LeftAttacks");
            TemplateContainer attack3Element = leftAttacks.Query<TemplateContainer>("Attack3");
            VisualElement attack3Content = attack3Element.Query<VisualElement>("Content");
            Button attack3Button = attack3Content.Query<Button>("AttackButton");
            return attack3Button;
        }
    }
    public Button Attack3InfoButton
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            VisualElement leftAttacks = attackVE.Query<VisualElement>("LeftAttacks");
            TemplateContainer attack3Element = leftAttacks.Query<TemplateContainer>("Attack3");
            VisualElement attack3Content = attack3Element.Query<VisualElement>("Content");
            Button attack3InfoButton = attack3Content.Query<Button>("AttackInfoButton");
            return attack3InfoButton;
        }
    }
    public Button Attack4Button
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            VisualElement leftAttacks = attackVE.Query<VisualElement>("RightAttacks");
            TemplateContainer attack4Element = leftAttacks.Query<TemplateContainer>("Attack4");
            VisualElement attack4Content = attack4Element.Query<VisualElement>("Content");
            Button attack4Button = attack4Content.Query<Button>("AttackButton");
            return attack4Button;
        }
    }
    public Button Attack4InfoButton
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
            VisualElement attackVE = content.Query<VisualElement>("Attacks");
            VisualElement leftAttacks = attackVE.Query<VisualElement>("RightAttacks");
            TemplateContainer attack4Element = leftAttacks.Query<TemplateContainer>("Attack4");
            VisualElement attack4Content = attack4Element.Query<VisualElement>("Content");
            Button attack4InfoButton = attack4Content.Query<Button>("AttackInfoButton");
            return attack4InfoButton;
        }
    }
}
