using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    string abilityName { get; set; }
    string description { get; set; }
    void TriggerEffect();
}
