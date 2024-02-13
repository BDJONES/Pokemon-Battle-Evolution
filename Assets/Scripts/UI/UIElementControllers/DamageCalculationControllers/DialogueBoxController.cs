using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueBoxController : MonoBehaviour
{
    [SerializeField] private DialogueUIElements dialogueUIElements;
    private Label dialogueBoxText;

    private void Start()
    {
        dialogueBoxText = dialogueUIElements.DialogueBox.Q<Label>("Text");
    }
}
