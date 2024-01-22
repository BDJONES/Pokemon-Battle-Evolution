using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    BattleStart,
    TurnStart,
    WaitingOnPlayerInput,
    ProcessingInput,
    FirstAttack,
    SecondAttack,
    TurnEnd,
    BattleEnd
}
