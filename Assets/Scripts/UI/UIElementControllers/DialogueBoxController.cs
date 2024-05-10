using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueBoxController : NetworkBehaviour
{
    [SerializeField] private DialogueUIElements dialogueUIElements;
    [SerializeField] private InBattlePartyDialogueUIElements inBattlePartyDialogueUIElements;
    private Label dialogueBoxText;
    private Queue<FixedString128Bytes> dialogueQueue;
    private UIController uIController;
    private void OnEnable()
    {
        Debug.Log("Dialogue Box Enabled");
        NetworkCommands.UIControllerCreated += () =>
        {
            Debug.Log("Initializing Dialogue Queue");
            dialogueQueue = new Queue<FixedString128Bytes>();
            uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
            uIController.OnHostMenuChange += HandleMenuChange;
            uIController.OnClientMenuChange += HandleMenuChange;
        };
    }

    private void OnDisable()
    {
        if (uIController != null)
        {
            uIController.OnHostMenuChange -= HandleMenuChange;
            uIController.OnClientMenuChange -= HandleMenuChange;
        }
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.DialogueScreen || menu == Menus.InBattlePartyDialogueScreen)
        {
            InitializeFields();
        }

    }

    public int QueueSize()
    {
        return dialogueQueue.Count;
    }

    private void InitializeFields()
    {
        Debug.Log("Initializing the Fields");
        //dialogueBoxText = dialogueUIElements.DialogueBox.Q<Label>("Text");
        if (IsHost)
        {
            if (uIController.GetCurrentTrainer1Menu() == Menus.DialogueScreen)
            {
                if (dialogueUIElements.DialogueBox != null)
                {
                    VisualElement element = dialogueUIElements.DialogueBox.Query<VisualElement>();
                    if (element != null)
                    {
                        Debug.Log("Element was found");
                        dialogueBoxText = element.Query<Label>();
                    }
                }
                else
                {
                    Debug.Log("Unable to find the Dialogue Box");
                }
            }
            else if (uIController.GetCurrentTrainer1Menu() == Menus.InBattlePartyDialogueScreen)
            {
                if (inBattlePartyDialogueUIElements.DialogueBox != null)
                {
                    VisualElement element = inBattlePartyDialogueUIElements.DialogueBox.Query<VisualElement>();
                    if (element != null)
                    {
                        Debug.Log("Element was found");
                        dialogueBoxText = element.Query<Label>();
                    }
                }
                else
                {
                    Debug.Log("Unable to find the Dialogue Box");
                }
            }
        }
        else
        {
            Debug.Log("Looking for the client's dialogue box");
            if (uIController.GetCurrentTrainer2Menu() == Menus.DialogueScreen)
            {
                if (dialogueUIElements.DialogueBox != null)
                {
                    VisualElement element = dialogueUIElements.DialogueBox.Query<VisualElement>();
                    if (element != null)
                    {
                        Debug.Log("Element was found");
                        dialogueBoxText = element.Query<Label>();
                    }
                }
                else
                {
                    Debug.Log("Unable to find the Dialogue Box");
                }
            }
            else if (uIController.GetCurrentTrainer2Menu() == Menus.InBattlePartyDialogueScreen)
            {
                if (inBattlePartyDialogueUIElements.DialogueBox != null)
                {
                    VisualElement element = inBattlePartyDialogueUIElements.DialogueBox.Query<VisualElement>();
                    if (element != null)
                    {
                        Debug.Log("Element was found");
                        dialogueBoxText = element.Query<Label>();
                    }
                }
                else
                {
                    Debug.Log("Unable to find the Dialogue Box");
                }
            }
        }
    }

    public void AddDialogueToQueue(string dialogue)
    {
        FixedString128Bytes convertedDialogue = new FixedString128Bytes(dialogue);
        dialogueQueue.Enqueue(convertedDialogue);
        Debug.Log($"Dialogue Added was {dialogue}");
    }

    public async UniTask ReadOneDialogueTest()
    {
        FixedString128Bytes dialouge = dialogueQueue.Dequeue();
        if (dialogueBoxText == null)
        {
            Debug.Log("This Label was not found");
            return;
        }
        Debug.Log($"Dialogue being added: {dialouge}");
        dialogueBoxText.text = dialouge.ToString();
        Debug.Log($"Dialogue Box Text: {dialogueBoxText.text}");
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    public IEnumerator ReadFirstQueuedDialogue()
    {
        FixedString128Bytes dialouge = dialogueQueue.Dequeue();
        if (dialogueBoxText == null)
        {
            Debug.Log("This Label was not found");
            yield return null;
        }
        dialogueBoxText.text = dialouge.ToString();
        //Debug.Log(dialogueBoxText.text);
        yield return new WaitForSecondsRealtime(1);
        GameManager.Instance.FinishRPCTaskRpc();
        //await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);
    }    

    public IEnumerator ReadFirstQueuedDialogue(RPCManager rpcManager)
    {
        FixedString128Bytes dialouge = dialogueQueue.Dequeue();
        if (dialogueBoxText == null)
        {
            Debug.Log("This Label was not found");
            yield return null;
        }
        dialogueBoxText.text = dialouge.ToString();
        Debug.Log(dialogueBoxText.text);
        Debug.Log("Yielding for 1 second");
        yield return new WaitForSecondsRealtime(1);
       
        //await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);
    }

    public IEnumerator ReadAllQueuedDialogue()
    {
        Debug.Log($"How many things are in the dialogue queue {dialogueQueue.Count}");
        while (dialogueQueue.Count > 0)
        {
            FixedString128Bytes dialouge = dialogueQueue.Dequeue();
            Debug.Log($"This current dialogue is {dialouge}");
            dialogueBoxText.text = dialouge.ToString();
            yield return new WaitForSecondsRealtime(1);
            //await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: false);
        }
        GameManager.Instance.FinishRPCTaskRpc();
    }
}
