using UnityEngine;
using UnityEngine.UIElements;

public class DialogueUIElements : MonoBehaviour
{
    [SerializeField] private UIDocument uIDocument;
    public VisualElement DialogueBox
    {
        get
        {
            VisualElement dialogueBox = uIDocument.rootVisualElement.Q<VisualElement>("DialogueBox");
            return dialogueBox;
        }
    }
}
