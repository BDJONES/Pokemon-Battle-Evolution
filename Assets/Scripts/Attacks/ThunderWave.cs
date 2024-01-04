using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWave : Attack
{
    private void Awake()
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

    public override bool UseAttack(Pokemon attacker, Pokemon target)
    {
        if (target.GetType1().immunities.Contains(this.type)) {
            Debug.Log("The target is immune to the move");
            return false;
        }
        if (this.accuracy == 100)
        {
            TriggerEffect(attacker, target);
        }
        else
        {
            int generatedValue = Random.Range(0, 99);
            int accurateRange = 100 - this.accuracy;
            if (generatedValue < accurateRange)
            {
                Debug.Log("The Attack Missed");
                return false;
            }
            else
            {
                TriggerEffect(attacker, target);
            }
        }
        Debug.Log("The Attack Landed");
        return true;
    }
    public override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        if (target.GetStatus() == StatusConditions.Healthy && (target.GetType1() != StaticTypeObjects.Electric || (target.GetType2() && target.GetType2() != StaticTypeObjects.Electric)))
        {
            Debug.Log("The target was paralyzed");
            target.SetStatus(StatusConditions.Paralysis);
            target.StatusEffect();
        }
    }
}
