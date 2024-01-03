using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : Attack
{
    private void Awake()
    {
        this.attackName = "Earthquake";
    }

    public override bool UseAttack(Pokemon target)
    {
        throw new System.NotImplementedException();
    }
}
