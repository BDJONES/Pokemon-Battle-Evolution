using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ForfietUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uIDocument;
    
    public Button YesButton
    {
        get
        {
            if (uIDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            VisualElement forfietPromptElement = screen.Query<VisualElement>("Forfiet_Prompt");
            VisualElement forfietPromptScreen = forfietPromptElement.Query<VisualElement>("Screen");
            VisualElement forfietPromptContent = forfietPromptScreen.Query<VisualElement>("Content");
            VisualElement forfietPromptBody = forfietPromptContent.Query<VisualElement>("Body");
            VisualElement buttons = forfietPromptBody.Query<VisualElement>("Buttons");
            VisualElement yesButtonElement = buttons.Query<VisualElement>("Yes_Button");
            VisualElement yesButtonContent = yesButtonElement.Query<VisualElement>("Content");
            Button yesButton = yesButtonContent.Query<Button>("YesButton");
            return yesButton;
        }
    }

    public Button NoButton
    {
        get
        {
            if (uIDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            //Debug.Log("Hello");
            TemplateContainer forfietPromptElement = screen.Query<TemplateContainer>("Forfiet_Prompt");
            //Debug.Log("It's me");
            VisualElement forfietPromptScreen = forfietPromptElement.Query<VisualElement>("Screen");
            //Debug.Log("I was wondering if after all these years you'd like to meet");
            VisualElement forfietPromptContent = forfietPromptScreen.Query<VisualElement>("Content");
            //Debug.Log("To go over everything");
            VisualElement forfietPromptBody = forfietPromptContent.Query<VisualElement>("Body");
            //Debug.Log("They say that time's supposed to heal ya, but I ain't done much healing");
            VisualElement buttons = forfietPromptBody.Query<VisualElement>("Buttons");
            //Debug.Log("Hello, can you hear me?");
            VisualElement noButtonElement = buttons.Query<VisualElement>("No_Button");
            //Debug.Log("I'm in California dreaming about who we used to be");
            VisualElement noButtonContent = noButtonElement.Query<VisualElement>("Content");
            //Debug.Log("When we were younger and free");
            Button noButton = noButtonContent.Query<Button>("NoButton");
            //Debug.Log("I've forgotten how it felt before the world fell at our feet");
            return noButton;
        }
    }
}
