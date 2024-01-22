using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAttack : Attack
{
    public QuickAttack()
    {
        this.attackName = "Quick Attack";
        this.description = "The user lunges at the target to inflict damage, moving at blinding speed. This move always goes first.";
        this.type = StaticTypeObjects.Normal;
        this.moveCategory = AttackCategory.Physical;
        this.power = 40;
        this.accuracy = 100;
        this.priority = 1;
        this.currPowerPoints = 48;
        this.maxPowerPoints = 48;
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);  
    }
}