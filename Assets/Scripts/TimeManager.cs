using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class TimeManager : Singleton<TimeManager>
{
    private float matchTimer;
    private float turnTimer;
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
    }

    private void StartMatchTimer()
    {
        matchTimerActive = true;
        matchTimer = 60f;
    }
    
    private void StartTurnTimer()
    {
        turnTimerActive = true;
        turnTimer = 8f;
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
