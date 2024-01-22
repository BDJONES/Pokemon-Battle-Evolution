using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Attack
{
    public Flamethrower()
    {
        this.attackName = "Flamethrower";
        this.description = "The target is scorched with an intense blast of fire. This may also leave the target with a burn.";
        this.type = StaticTypeObjects.Fire;
        this.moveCategory = AttackCategory.Special;
        this.power = 90;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 16;
        this.maxPowerPoints = 16;
    }
    
    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        int genereatedValue = Random.Range(0, 99);
        if (genereatedValue < 10)
        {
            if (target.GetHPStat() > 0)
            {
                target.Status = StatusConditions.Burn;
                Debug.Log("Flamethrower burned the target");
            }

        }
    }
}
