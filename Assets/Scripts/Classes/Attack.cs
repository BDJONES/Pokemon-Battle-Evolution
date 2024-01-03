using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public string attackName;
    public string description;
    public Type type { get; set; }
    public AttackCategory moveCategory { get; set; }
    public int power { get; set; }


    public int accuracy { get; set; }


    public int currPowerPoints { get; set; }
    public int maxPowerPoints { get; set; }
    public abstract bool UseAttack(Pokemon target);
    public virtual void TriggerEffect(Pokemon target)
    {
        currPowerPoints -= 1;
        return;
    }
}