using Mono.Cecil.Cil;
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
            uIDocument = GameObject.Find("UI Controller").GetComponent<UIDocument>();
            if (uIDocument.rootVisualElement == null)
            {
                Debug.Log("The root is null");
                return null;
            }
            VisualElement dialogueBox = uIDocument.rootVisualElement.Q<TemplateContainer>("DialogueBox");
            if (dialogueBox != null)
            {
                return dialogueBox;
            }
            return null;
        }
    }
}
