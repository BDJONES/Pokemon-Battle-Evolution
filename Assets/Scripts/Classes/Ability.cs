using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    protected string abilityName = null!;
    protected string description = null!;
    public abstract void TriggerEffect(Pokemon target);
}
