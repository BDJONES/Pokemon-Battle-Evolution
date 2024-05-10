using UnityEngine.UIElements;
using UnityEngine;

public class InBattlePartyDialogueUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    public Button Pokemon1Button
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            TemplateContainer pokemon1 = uiDocument.rootVisualElement.Q<TemplateContainer>("Pokemon1");
            Button pokemon1Button = pokemon1.Q<Button>("Widget");
            return pokemon1Button ?? null;
        }
    }
    public Button Pokemon2Button
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            TemplateContainer pokemon2 = uiDocument.rootVisualElement.Q<TemplateContainer>("Pokemon2");
            Button pokemon2Button = pokemon2.Q<Button>("Widget");
            return pokemon2Button ?? null;
        }
    }
    public Button Pokemon3Button
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            TemplateContainer pokemon3 = uiDocument.rootVisualElement.Q<TemplateContainer>("Pokemon3");
            Button pokemon3Button = pokemon3.Q<Button>("Widget");
            return pokemon3Button ?? null;
        }
    }
    public Button Pokemon4Button
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            TemplateContainer pokemon4 = uiDocument.rootVisualElement.Q<TemplateContainer>("Pokemon4");
            Button pokemon4Button = pokemon4.Q<Button>("Widget");
            return pokemon4Button ?? null;
        }
    }
    public Button Pokemon5Button
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            TemplateContainer pokemon5 = uiDocument.rootVisualElement.Q<TemplateContainer>("Pokemon5");
            Button pokemon5Button = pokemon5.Q<Button>("Widget");
            return pokemon5Button ?? null;
        }
    }
    public Button Pokemon6Button
    {
        get
        {
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            TemplateContainer pokemon6 = uiDocument.rootVisualElement.Q<TemplateContainer>("Pokemon6");
            Button pokemon6Button = pokemon6.Q<Button>("Widget");
            return pokemon6Button ?? null;
        }
    }
    public VisualElement DialogueBox
    {
        get
        {
            uiDocument = GameObject.Find("UI Controller").GetComponent<UIDocument>();
            if (uiDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            VisualElement dialogueBox = uiDocument.rootVisualElement.Q<TemplateContainer>("DialogueBox");
            if (dialogueBox != null)
            {
                return dialogueBox;
            }
            return null;
        }
    }
}