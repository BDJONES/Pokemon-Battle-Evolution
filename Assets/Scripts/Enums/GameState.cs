using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    BattleStart,
    TurnStart,
    WaitingOnPlayerInput,
    ProcessingInput,
    TurnEnd,
    BattleEnd
}
