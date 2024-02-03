using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    LoadingPokemonInfo,
    BattleStart,
    TurnStart,
    WaitingOnPlayerInput,
    ProcessingInput,
    FirstAttack,
    SecondAttack,
    TurnEnd,
    BattleEnd
}
