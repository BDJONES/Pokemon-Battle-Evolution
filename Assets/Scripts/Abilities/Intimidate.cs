using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Intimidate : Ability
{
    protected override void Awake()
    {
        //trainerController = this.abilityUser.transform.parent.gameObject.GetComponent<TrainerController>();
        base.Awake();
        eventsToTriggerManager.OnTriggerEvent += EventToTriggerHandler;
        this.abilityName = "Intimidate";
        this.description = "When the Pokémon enters a battle, it intimidates opposing Pokémon and makes them cower, lowering their Attack stat.";
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("The host is changing the attack value");
            target.AttackStage--;
        }
        else
        {
            Debug.Log("The client is changing the attack value");
            target.RequestStatChangeRpc(Stats.Attack, target.AttackStage - 1);
        }
        GameManager.Instance.SendDialogueToClientRpc($"{target.GetNickname()} was intimidated");
        GameManager.Instance.SendDialogueToHostRpc($"{target.GetNickname()} was intimidated");
        while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
        {
            await UniTask.Yield();
        }
    }       
    
    private void EventToTriggerHandler(EventsToTrigger trigger)
    {

        if (this.abilityUser == null) { return; }
        if (trigger == EventsToTrigger.YourPokemonSwitched)
        {
            Debug.Log("Your Pokemon Switched");
            if (this.abilityUser.ActiveState == true)
            {
                Debug.Log($"{abilityUser.GetSpeciesName()}, Intimidate is activating");
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
