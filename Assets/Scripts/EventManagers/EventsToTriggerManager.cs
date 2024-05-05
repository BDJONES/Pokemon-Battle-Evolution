using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EventsToTriggerManager : NetworkBehaviour
{
    public event Action <EventsToTrigger> OnTriggerEvent;
    private static readonly Dictionary<EventsToTrigger, EventsToTrigger> eventOpposites = new()
    {
        {EventsToTrigger.YourPokemonDamaged,  EventsToTrigger.OpposingPokemonDamaged},
        {EventsToTrigger.OpposingPokemonDamaged, EventsToTrigger.YourPokemonDamaged},
        {EventsToTrigger.YourPokemonAttackedOpposingPokemon, EventsToTrigger.OpposingPokemonAttackedYourPokemon},
        {EventsToTrigger.OpposingPokemonAttackedYourPokemon, EventsToTrigger.YourPokemonAttackedOpposingPokemon},
        {EventsToTrigger.YourPokemonSwitched, EventsToTrigger.OpposingPokemonSwitched},
        {EventsToTrigger.OpposingPokemonSwitched, EventsToTrigger.YourPokemonSwitched},
        {EventsToTrigger.YourPokemonAbilityTriggered, EventsToTrigger.OpposingPokemonAbilityTriggered},
        {EventsToTrigger.OpposingPokemonAbilityTriggered, EventsToTrigger.YourPokemonAbilityTriggered},
        {EventsToTrigger.YourPokemonItemTriggered, EventsToTrigger.OpposingPokemonItemTriggered},
        {EventsToTrigger.OnSuperEffectiveHit, EventsToTrigger.OnSuperEffectiveHit},
        {EventsToTrigger.OnResistedHit, EventsToTrigger.OnResistedHit}
    };

    private void OnEnable()
    {
        gameObject.name = "EventsToTriggerManager";
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateClientOfEventTriggeredRpc(EventsToTrigger e)
    {
        Debug.Log("In the RPC");
        if (IsHost) return;
        Debug.Log($"{e} event is being invoked on client");
        OnTriggerEvent?.Invoke(e);
    }

    public void AlertEventTriggered(EventsToTrigger e)
    {
        Debug.Log($"{e} Event will be alerted");
        OnTriggerEvent?.Invoke(e);
        EventsToTrigger oppEvent =  eventOpposites[e];
        UpdateClientOfEventTriggeredRpc(oppEvent);
    }
}
