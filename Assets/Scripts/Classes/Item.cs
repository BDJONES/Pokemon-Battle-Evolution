using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    protected string itemName;
    protected string description;
    protected Pokemon holder;
    public void SetHolder(Pokemon holder)
    {
        this.holder = holder;
    }
    protected abstract void InitializeItem();
    public abstract void TriggerEffect(Pokemon holder);
    public virtual void RevertEffect(Pokemon holder)
    {
        return;
    }
}
