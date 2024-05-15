using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceBand : Item
{
    public ChoiceBand()
    {
        GameManager.OnStateChange += GameStateOnChangeHandler;
    }

    ~ChoiceBand()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
    }

    private void GameStateOnChangeHandler(GameState state)
    {
        if (state == GameState.LoadingPokemonInfo && holder.ActiveState)
        {
            Debug.Log("Activating the ChoiceBand");
            InitializeItem();
            //TriggerEffect();
        }
    }

    protected override void InitializeItem()
    {
        this.itemName = "Choice Band";
        this.description = "An item to be held by a Pokémon. This curious headband boosts the holder's Attack stat but only allows the use of a single move.";
    }

    protected override void TriggerEffect(int userType)
    {
        holder.SetAttackStat(Mathf.FloorToInt(holder.GetAttackStat() * 1.5f));
    }

    protected override void RevertEffect()
    {
        holder.SetAttackStat(Mathf.FloorToInt(holder.GetAttackStat() * 0.67f));
    }


}
