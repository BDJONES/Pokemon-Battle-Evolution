using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : Ability
{
    public override void TriggerEffect(Pokemon target)
    {
        return;
    }

    private void Awake()
    {
        this.abilityName = "Intimidate";
        this.description = "When the Pok�mon enters a battle, it intimidates opposing Pok�mon and makes them cower, lowering their Attack stats.";
    }


}
