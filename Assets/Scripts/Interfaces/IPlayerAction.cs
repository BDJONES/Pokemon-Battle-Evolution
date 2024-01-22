using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction
{
    IPlayerAction PerformAction(Pokemon pokemon1, Pokemon pokemon2);
    IPlayerAction PerformAction(Trainer trainer, Pokemon pokemon2);
}