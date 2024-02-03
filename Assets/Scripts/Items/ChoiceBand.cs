using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceBand : Item
{
    private void OnEnable()
    {
        GameManager.OnStateChange += GameStateOnChangeHandler;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
    }

    private void GameStateOnChangeHandler(GameState state)
    {
        if (state == GameState.LoadingPokemonInfo)
        {
            InitializeItem();
            TriggerEffect(this.holder);
        }
    }

    protected override void InitializeItem()
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
