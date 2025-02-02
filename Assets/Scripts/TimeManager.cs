using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class TimeManager : Singleton<TimeManager>
{
    private float matchTimer = 100f;
    private float turnTimer = 100f;
    private bool matchTimerActive = false;
    private bool turnTimerActive = false;
    public static event Action MatchTimerEnd;
    //public static event Action TurnTimerEnd;
    public float MatchTimer 
    {  
        get { 
            return matchTimer; 
        } 
    }
    public float TurnTimer
    {
        get
        {
            return turnTimer;
        }
    }

    private void OnEnable()
    {
        GameManager.OnStateChange += HandleStateChange;
        UIInputGrabber.MoveSelected += ChangeTimerToInactive;
    }

    private void ChangeTimerToInactive()
    {
        turnTimerActive = false;
    }

    private void HandleStateChange(GameState state)
    {
        if (state == GameState.BattleStart)
        {
            StartMatchTimer();
        }
        else if (state == GameState.TurnStart)
        {
            StartTurnTimer();
        }
        else if (state == GameState.ProcessingInput)
        {
            turnTimerActive = false;
        }
        else if (state == GameState.WaitingOnPlayerInput)
        {
            StartTurnTimer();
        }
    }

    private void StartMatchTimer()
    {
        matchTimerActive = true;
        matchTimer = 60f;
    }
    
    private void StartTurnTimer()
    {
        turnTimerActive = true;
        turnTimer = 10f;
    }

    private void Update()
    {
        if (matchTimerActive && matchTimer > 0)
        {
            matchTimer -= Time.deltaTime;
        }

        if (turnTimerActive && turnTimer > 0)
        {
            turnTimer -= Time.deltaTime;
        }

        if (matchTimer <= 0)
        {
            matchTimerActive = false;
            UpdateMatchState();
        }

        if (turnTimer <= 0)
        {
            turnTimerActive = false;
            UpdateTurnState();
        }
    }

    private void UpdateMatchState()
    {

        if (matchTimer == 0)
        {
            //Debug.Log("Match timer Ending");
            MatchTimerEnd.Invoke();
        }
    }

    private void UpdateTurnState()
    {
        //if (turnTimer == 0)
        //{
        //    TurnTimerEnd.Invoke();
        //}
    }

    public bool IsMatchTimerActive()
    {
        return matchTimerActive;
    }

    public bool IsTurnTimerActive()
    {
        return turnTimerActive;
    }
}
