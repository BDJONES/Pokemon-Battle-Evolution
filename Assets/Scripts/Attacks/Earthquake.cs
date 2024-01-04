using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : Attack
{
    private void Awake()
    {
        this.attackName = "Earthquake";
        this.description = "The user sets off an earthquake that strikes every Pokémon around it.";
        this.type = StaticTypeObjects.Ground;
        this.moveCategory = AttackCategory.Physical;
        this.power = 100;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 16;
        this.maxPowerPoints = 16;
    }

    public override bool UseAttack(Pokemon attacker, Pokemon target)
    {
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
        // IF the opponent is underground, double the damage     
    }
}
