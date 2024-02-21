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
    int inactiveTurnCount;
    public static event Action playerTooInactive;

    private void Start()
    {
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

    public async UniTask<IPlayerAction> SelectMove()
    {
        Debug.Log("You can now choose something to do.");
        IPlayerAction selection = null;
        if (this.gameObject != null && this.gameObject.name != "Trainer # 2")
        {
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
            UIInputGrabber.MoveSelected -= anonFunc;
        } 
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
                playerTooInactive.Invoke();
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
