using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    protected string itemName;
    protected string description;
    protected Pokemon holder;
    protected int priority;
    protected UIController uIController;
    public void SetHolder(Pokemon holder)
    {
        this.holder = holder;
    }
    protected abstract void InitializeItem();

    protected virtual void TriggerEffect(int userType)
    {
        return;
    }

    protected virtual void RevertEffect()
    {
        return;
    }
}
