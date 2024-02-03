using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rock : Type
{
    public override void InitializeValues()
    {
        typeName = "Rock";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Ground, StaticTypeObjects.Grass, StaticTypeObjects.Water, StaticTypeObjects.Fighting };
        resistances = new List<Type> { StaticTypeObjects.Flying, StaticTypeObjects.Fire, StaticTypeObjects.Normal, StaticTypeObjects.Poison };
        strengths = new List<Type> { StaticTypeObjects.Fire, StaticTypeObjects.Flying, StaticTypeObjects.Ice, StaticTypeObjects.Bug };
    }
}