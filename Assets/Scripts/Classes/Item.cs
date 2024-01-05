using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected string itemName { get; set; }
    protected string description { get; set; }
    public abstract void TriggerEffect(Pokemon holder);
    public virtual void RevertEffect(Pokemon holder)
    {
        return;
    }
}
