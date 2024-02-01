using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dragon : Type
{
    public override void InitializeValues()
    {
        typeName = "Dragon";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Dragon, StaticTypeObjects.Ice, StaticTypeObjects.Fairy };
        resistances = new List<Type> { StaticTypeObjects.Fire, StaticTypeObjects.Water, StaticTypeObjects.Grass, StaticTypeObjects.Electric };
        strengths = new List<Type> { StaticTypeObjects.Dragon };
    }
}