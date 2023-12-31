using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Water : Type
{
    public Water()
    {
        typeName = "Water";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Electric, StaticTypeObjects.Grass };
        resistances = new List<Type> { StaticTypeObjects.Water, StaticTypeObjects.Ice, StaticTypeObjects.Fire, StaticTypeObjects.Steel };
        strengths = new List<Type> { StaticTypeObjects.Rock, StaticTypeObjects.Ground, StaticTypeObjects.Fire };
    }
}