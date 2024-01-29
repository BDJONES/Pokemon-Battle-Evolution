using UnityEngine;
using UnityEngine.UIElements;

public class InBattlePartyUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
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
            TemplateContainer backButtonElement = screen.Query<TemplateContainer>("Back_Button");
            VisualElement backButtonContent = backButtonElement.Query<VisualElement>("Content");
            Button backButton = backButtonContent.Query<Button>("BackButton");
            return backButton;
        }
    }
}