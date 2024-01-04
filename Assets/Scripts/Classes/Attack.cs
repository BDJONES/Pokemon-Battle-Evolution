using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    protected string attackName;
    protected string description;
    protected Type type = null!;
    protected AttackCategory moveCategory;
    protected int power;
    protected int accuracy;
    protected int priority;
    protected int currPowerPoints;
    protected int maxPowerPoints;
    public string GetAttackName()
    {
        return attackName;
    }
    public string GetAttackDescription()
    {
        return description;
    }
    public Type GetAttackType()
    {
        return type;
    }
    public AttackCategory GetAttackCategory()
    {
        return moveCategory;
    }
    public int GetAttackPower()
    {
        return power;
    }
    public int GetAttackAccuracy()
    {
        return accuracy;
    }
    public int GetAttackPriority()
    {
        return priority;
    }
    public int GetCurrentPP()
    {
        return currPowerPoints;
    }
    public int GetMaxPP()
    {
        return maxPowerPoints;
    }
    public abstract bool UseAttack(Pokemon attacker, Pokemon target);
    public virtual void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        currPowerPoints -= 1;
        return;
    }
}