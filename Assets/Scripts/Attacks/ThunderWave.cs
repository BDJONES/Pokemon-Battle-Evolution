using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWave : Attack
{
    private void Awake()
    {
        this.attackName = "Thunder Wave";
    }

    public override bool UseAttack(Pokemon target)
    {
        throw new System.NotImplementedException();
    }
}
