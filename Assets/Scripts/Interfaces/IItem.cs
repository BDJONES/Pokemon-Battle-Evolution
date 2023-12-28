using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    string itemName { get; set; }
    string description { get; set; }
    void TriggerEffect();
}
