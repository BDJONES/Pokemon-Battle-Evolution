using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : Ability
{


    private void Awake()
    {
        GameManager.OnStateChange += GameStateOnChangeHandler;
        this.abilityName = "Intimidate";
        this.description = "When the Pokémon enters a battle, it intimidates opposing Pokémon and makes them cower, lowering their Attack stats.";
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        Debug.Log("You intimidated the target");
        target.AttackStage--;
    }    
    protected override void GameStateOnChangeHandler(GameState state)
    {
        if (state == GameState.BattleStart)
        {
            if (this.abilityUser == GameManager.Instance.trainer1.activePokemon)
            {
                TriggerEffect(this.abilityUser, GameManager.Instance.trainer2.activePokemon);
            }
            else
            {
                TriggerEffect(this.abilityUser, GameManager.Instance.trainer1.activePokemon);
            }
        }
    }
}
