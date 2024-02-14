using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uIDocument;
    public VisualElement DialogueBox
    {
        get
        {
            if (uIDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            VisualElement screen = uIDocument.rootVisualElement.Query<VisualElement>("Screen");
            if (screen != null)
            {
                VisualElement content = screen.Query<VisualElement>("Content");
                if (content != null)
                {
                    VisualElement dialogueBox = content.Q<TemplateContainer>("DialogueBox");
                    if (dialogueBox != null)
                    {
                        return dialogueBox;
                    }
                }
            }
            return null;
        }
    }
}
