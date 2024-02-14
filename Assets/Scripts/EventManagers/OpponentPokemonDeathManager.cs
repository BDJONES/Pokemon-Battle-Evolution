using System;
using UnityEngine;

public class OpponentPokemonDeathEventManager : Singleton<OpponentPokemonDeathEventManager>
{
    public static event Action OnDeath;

    public static void AlertOfDeath()
    {
        if (OnDeath != null)
        {
            Debug.Log("Opponent Death Invoked");
            OnDeath.Invoke();
        }
    }
}