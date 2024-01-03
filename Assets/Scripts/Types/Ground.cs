using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Ground : Type
{
    private void Awake()
    {
        typeName = "Ground";
        immunities = new List<Type> { StaticTypeObjects.Electric };
        weaknesses = new List<Type> { StaticTypeObjects.Water, StaticTypeObjects.Ice, StaticTypeObjects.Grass };
        resistances = new List<Type> { StaticTypeObjects.Rock, StaticTypeObjects.Poison };
        strengths = new List<Type> { StaticTypeObjects.Electric, StaticTypeObjects.Rock, StaticTypeObjects.Fire, StaticTypeObjects.Steel, StaticTypeObjects.Poison };
    }
}