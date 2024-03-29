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

            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement forfietPromptElement = screen.Query<VisualElement>("Forfiet_Prompt");
                if (forfietPromptElement != null )
                {
                    VisualElement forfietPromptScreen = forfietPromptElement.Query<VisualElement>("Screen");
                    if (forfietPromptScreen != null )
                    {
                        VisualElement forfietPromptContent = forfietPromptScreen.Query<VisualElement>("Content");
                        if (forfietPromptContent != null )
                        {
                            VisualElement forfietPromptBody = forfietPromptContent.Query<VisualElement>("Body");
                            if (forfietPromptBody != null )
                            {
                                VisualElement buttons = forfietPromptBody.Query<VisualElement>("Buttons");
                                if (buttons != null )
                                {
                                    VisualElement yesButtonElement = buttons.Query<VisualElement>("Yes_Button");
                                    if (yesButtonElement != null )
                                    {
                                        VisualElement yesButtonContent = yesButtonElement.Query<VisualElement>("Content");
                                        if (yesButtonContent != null )
                                        {
                                            Button yesButton = yesButtonContent.Query<Button>("YesButton");
                                            if(yesButton != null )
                                            {
                                                return yesButton;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
    }

    public Button NoButton
    {
        get
        {
            //uIDocument.asset
            //Debug.Log(gameObject.name);
            uIDocument = GameObject.Find("UI Controller").GetComponent<UIDocument>();
            if (uIDocument == null)
            {
                Debug.Log("The UI document is null");
                Debug.Log(uIDocument.visualTreeAsset);
                return null;
            }
            if (uIDocument.rootVisualElement == null)
            {
                
                Debug.Log("The root is null");
                //return null;
            }
            
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                TemplateContainer forfietPromptElement = screen.Query<TemplateContainer>("Forfiet_Prompt");
                if (forfietPromptElement != null)
                {
                    VisualElement forfietPromptScreen = forfietPromptElement.Query<VisualElement>("Screen");
                    if (forfietPromptScreen != null)
                    {
                        VisualElement forfietPromptContent = forfietPromptScreen.Query<VisualElement>("Content");
                        if (forfietPromptContent != null)
                        {
                            VisualElement forfietPromptBody = forfietPromptContent.Query<VisualElement>("Body");
                            if (forfietPromptBody != null)
                            {
                                VisualElement buttons = forfietPromptBody.Query<VisualElement>("Buttons");
                                if (buttons != null)
                                {
                                    VisualElement noButtonElement = buttons.Query<VisualElement>("No_Button");
                                    if (noButtonElement != null)
                                    {
                                        VisualElement noButtonContent = noButtonElement.Query<VisualElement>("Content");
                                        if (noButtonContent != null)
                                        {
                                            Button noButton = noButtonContent.Query<Button>("NoButton");
                                            if (noButton != null)
                                            {
                                                return noButton;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }    
            }
            return null;
        }
    }
}
