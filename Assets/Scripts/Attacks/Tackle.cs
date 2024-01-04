using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : Attack
{
    private void Awake()
    {
        this.attackName = "Tackle";
        this.description = "A physical attack in which the user charges and slams into the target with its whole body.";
        this.type = StaticTypeObjects.Normal;
        this.moveCategory = AttackCategory.Physical;
        this.power = 40;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 56;
        this.maxPowerPoints = 56;
    }

    public override bool UseAttack(Pokemon attacker, Pokemon target)
    {
        if (this.accuracy == 100f)
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
}
