using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : Ability
{
    private void Awake()
    {
        this.abilityName = "Intimidate";
        this.description = "When the Pokémon enters a battle, it intimidates opposing Pokémon and makes them cower, lowering their Attack stats.";
    }

    public override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        Debug.Log("You intimidated the target");
        target.SetAttackStage(target.GetAttackStage() - 1);
    }
}
