using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ice : Type
{
    private void Awake()
    {
        typeName = "Ice";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Fire, StaticTypeObjects.Fighting, StaticTypeObjects.Rock, StaticTypeObjects.Steel };
        resistances = new List<Type> { StaticTypeObjects.Ice };
        strengths = new List<Type> { StaticTypeObjects.Dragon, StaticTypeObjects.Grass, StaticTypeObjects.Flying, StaticTypeObjects.Ground };
    }
}
