using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBand : Item
{
    private void Start()
    {
        this.itemName = "Choice Band";
        this.description = "An item to be held by a Pokémon. This curious headband boosts the holder's Attack stat but only allows the use of a single move.";
    }

    public override void TriggerEffect(Pokemon holder)
    {
        holder.SetAttackStat(Mathf.FloorToInt(holder.GetAttackStat() * 1.5f));
    }

    public override void RevertEffect(Pokemon holder)
    {
        holder.SetAttackStat(Mathf.FloorToInt(holder.GetAttackStat() * 0.67f));
    }
}
