using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueBoxController : MonoBehaviour
{
    [SerializeField] private DialogueUIElements dialogueUIElements;
    private static Label dialogueBoxText;

    private void OnEnable()
    {
        UIController.OnMenuChange += HandleMenuChange;    
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.DialogueScreen)
        {
            InitializeFields();
        }
        
    }

    private void InitializeFields()
    {
        //dialogueBoxText = dialogueUIElements.DialogueBox.Q<Label>("Text");
        if (dialogueUIElements.DialogueBox != null)
        {
            VisualElement element = dialogueUIElements.DialogueBox.Query<VisualElement>();
            if (element != null)
            {
                dialogueBoxText = element.Query<Label>();
            }
        }
    }

    public async static UniTask RequestForTextChange(string text)
    {
        //Debug.Log(dialogueBoxText.text);
        string newString = "";
        foreach (var c in text)
        {
            newString += c;
            //Debug.Log(newString);
            dialogueBoxText.text = newString;
            await UniTask.Delay(TimeSpan.FromMilliseconds(75), ignoreTimeScale: false);
        }
    }
}
