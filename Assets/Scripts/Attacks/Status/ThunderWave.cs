using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWave : Attack
{
    public ThunderWave()
    {
        this.attackName = "Thunder Wave";
        this.description = "The user launches a weak jolt of electricity that paralyzes the target.";
        this.type = StaticTypeObjects.Electric;
        this.moveCategory = AttackCategory.Status;
        this.power = 0;
        this.accuracy = 90;
        this.priority = 0;
        this.currPowerPoints = 32;
        this.maxPowerPoints = 32;
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        if (target.Status == StatusConditions.Healthy && (target.GetType1() != StaticTypeObjects.Electric || (target.GetType2() != null && target.GetType2() != StaticTypeObjects.Electric)))
        {
            Debug.Log("The target was paralyzed");
            target.Status = StatusConditions.Paralysis;
        }
    }
}
