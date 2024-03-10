using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] private Trainer player;
    [SerializeField] private Trainer opponent;
    private UIInputGrabber uIGrabber;
    private DialogueBoxController dialogueBoxController;
    private int inactiveTurnCount;
    public event Action playerTooInactive;

    private void OnEnable()
    {
        dialogueBoxController = gameObject.GetComponentInChildren<DialogueBoxController>();
        uIGrabber = new UIInputGrabber();
        inactiveTurnCount = 0;
    }

    public Trainer GetPlayer()
    {
        return player;
    }

    public Trainer GetOpponent() { 
        return opponent;
    }

    public DialogueBoxController GetDialogueBoxController()
    {
        return dialogueBoxController;
    }

    public void SetOpponent(Trainer Opponent)
    {
        opponent = Opponent;
    }

    public async UniTask<IPlayerAction> SelectMove()
    {
        IPlayerAction selection = null;
        Action anonFunc = () =>
        {
            // Invoke the handler and set value of selection to returned Action
            selection = uIGrabber.GetSelectedAction();
            Debug.Log("Selection was made");
        };

        UIInputGrabber.MoveSelected += anonFunc;
        while (selection == null && TimeManager.Instance.IsTurnTimerActive())
        {
            await UniTask.Yield();
        }
        await dialogueBoxController.ReadAllQueuedDialogue();
        UIInputGrabber.MoveSelected -= anonFunc;
        // If a move was selected return that value
        if (selection != null)
        {
            return selection;
        }
        else
        {
            Debug.Log("Time out of the turn. Will select a random move.");
            inactiveTurnCount++;
            if (inactiveTurnCount >= 3)
            {
                playerTooInactive?.Invoke();
            }
            var attacks = player.GetActivePokemon().GetMoveset();
            int randomMove = UnityEngine.Random.Range(0, 3);
            return attacks[randomMove];
        }
    }

    public async UniTask<Switch> SwitchOutFaintedPokemon()
    {
        Debug.Log("You can now choose something to do.");
        Switch selection = null;
        if (this.gameObject != null && this.gameObject.name != "Trainer # 2")
        {
            Action anonFunc = () =>
            {
                // Invoke the handler and set value of selection to returned Action
                selection = (Switch) uIGrabber.GetSelectedAction();
                Debug.Log("Selection was made");
            };

            UIInputGrabber.MoveSelected += anonFunc;
            while (selection == null && TimeManager.Instance.IsTurnTimerActive())
            {
                await UniTask.Yield();
            }
            UIInputGrabber.MoveSelected -= anonFunc;
        }
        // If a move was selected return that value
        if (selection != null)
        {
            return selection;
        }
        else
        {
            inactiveTurnCount++;
            if (inactiveTurnCount >= 3)
            {
                playerTooInactive.Invoke();
            }
            List<int> availablePokemon = new List<int>();
            var team = player.GetPokemonTeam();
            for (int i = 0; i < team.Length; i++)
            {
                if (team[i] != null && !team[i].IsDead())
                {
                    availablePokemon.Add(i);
                }
            }
            int randomIndex = UnityEngine.Random.Range(0, availablePokemon.Count);
            return new Switch(player, team[availablePokemon[randomIndex]]);
        }
    }
}
