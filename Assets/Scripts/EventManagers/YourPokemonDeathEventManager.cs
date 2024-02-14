using System;
using UnityEngine;

public class YourPokemonDeathEventManager : Singleton<YourPokemonDeathEventManager>
{
    public static event Action OnDeath;
    
    public static void AlertOfDeath()
    {
        if (OnDeath != null)
        {
            Debug.Log("Your Death Invoked");
            OnDeath.Invoke();
        }
    }
}