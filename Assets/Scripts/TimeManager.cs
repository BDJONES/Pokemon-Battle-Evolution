using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private float matchTimer;
    private float turnTimer;
    private bool matchTimerActive = false;
    private bool turnTimerActive = false;

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

    public void StartMatchTimer()
    {
        matchTimerActive = true;
        matchTimer = 60f;
    }
    
    public void StartTurnTimer()
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
        }

        if (turnTimer <= 0)
        {
            turnTimerActive = false;
        }
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
