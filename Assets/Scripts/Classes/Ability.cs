using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public Pokemon abilityUser;
    protected string abilityName = null!;
    protected string description = null!;
    
    protected abstract void TriggerEffect(Pokemon attacker, Pokemon target);
    protected abstract void GameStateOnChangeHandler(GameState state);
}
