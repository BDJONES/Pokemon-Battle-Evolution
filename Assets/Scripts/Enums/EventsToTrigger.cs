using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventsToTrigger
{
    YourPokemonDamaged,
    OpposingPokemonDamaged,
    YourPokemonAttackedOpposingPokemon,
    OpposingPokemonAttackedYourPokemon,
    YourPokemonSwitched,
    OpposingPokemonSwitched,
    YourPokemonAbilityTriggered,
    OpposingPokemonAbilityTriggered,
    YourPokemonItemTriggered,
    OpposingPokemonItemTriggered,
    OnSuperEffectiveHit,
    OnResistedHit
}
