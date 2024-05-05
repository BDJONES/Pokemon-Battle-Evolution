using UnityEngine;
using UnityEngine.UIElements;

public class WinLoseUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uIDocument;

    public Button QuitButton
    {
        get
        {
            if (uIDocument == null || uIDocument.rootVisualElement == null)
            {
                Debug.Log("UI Document is null");
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement content = screen.Query<VisualElement>("Content");
                if (content != null)
                {
                    Button quitButton = content.Query<Button>();
                    if (quitButton != null)
                    {
                        return quitButton;
                    }
                }
            }
            return null;
        }
    }
}