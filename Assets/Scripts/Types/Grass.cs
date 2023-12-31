using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Grass : Type
{
    public Grass()
    {
        typeName = "Grass";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Fire, StaticTypeObjects.Flying, StaticTypeObjects.Ice, StaticTypeObjects.Poison, StaticTypeObjects.Bug };
        resistances = new List<Type> { StaticTypeObjects.Electric, StaticTypeObjects.Water, StaticTypeObjects.Ground, StaticTypeObjects.Grass };
        strengths = new List<Type> { StaticTypeObjects.Rock, StaticTypeObjects.Ground, StaticTypeObjects.Water };
    }
}
