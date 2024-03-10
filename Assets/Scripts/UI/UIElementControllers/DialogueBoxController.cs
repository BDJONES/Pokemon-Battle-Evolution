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
    private Label dialogueBoxText;
    private Queue<string> dialogueQueue;
    private UIController uIController;
    private void OnEnable()
    {
        dialogueQueue = new Queue<string>();
        uIController = transform.parent.gameObject.GetComponentInChildren<UIController>();
        uIController.OnMenuChange += HandleMenuChange;    
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

    public void AddDialogueToQueue(string dialogue)
    {
        dialogueQueue.Enqueue(dialogue);
    }

    public async UniTask ReadFirstQueuedDialogue()
    {
        string dialouge = dialogueQueue.Dequeue();
        dialogueBoxText.text = dialouge;
        await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);
    }

    public async UniTask ReadAllQueuedDialogue()
    {
        while (dialogueQueue.Count > 0)
        {
            string dialouge = dialogueQueue.Dequeue();
            dialogueBoxText.text = dialouge;
            await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);
        }
    }
}
