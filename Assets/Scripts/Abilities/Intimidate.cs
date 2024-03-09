using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : Ability
{
    //TrainerController trainerController;
    protected override void Awake()
    {
        //trainerController = this.abilityUser.transform.parent.gameObject.GetComponent<TrainerController>();
        base.Awake();
        EventsToTriggerManager.OnTriggerEvent += EventToTriggerHandler;
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
    
    private void EventToTriggerHandler(EventsToTrigger trigger)
    {
        if (trigger == EventsToTrigger.YourPokemonSwitched)
        {
            if (this.abilityUser.ActiveState == true)
            {
                TriggerEffect(this.abilityUser, trainerController.GetOpponent().GetActivePokemon());
            }
        }
    }
    protected override void GameStateOnChangeHandler(GameState state)
    {
        return;
        //if (state == GameState.BattleStart)
        //{
        //    if (this.abilityUser == GameManager.Instance.GetTrainer1Controller().GetPlayer().GetActivePokemon())
        //    {
        //        TriggerEffect(this.abilityUser, GameManager.Instance.GetTrainer1Controller().GetOpponent().GetActivePokemon());
        //    }
        //    else
        //    {
        //        TriggerEffect(this.abilityUser, GameManager.Instance.GetTrainer1Controller().GetPlayer().GetActivePokemon());
        //    }
        //}
    }
}
