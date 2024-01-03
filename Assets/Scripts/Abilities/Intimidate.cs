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
        this.description = "When the Pokémon enters a battle, it intimidates opposing Pokémon and makes them cower, lowering their Attack stats.";
    }


}
