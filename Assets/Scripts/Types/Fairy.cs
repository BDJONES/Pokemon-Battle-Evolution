using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fairy : Type
{
    private void Awake()
    {
        typeName = "Fairy";
        immunities = new List<Type> { StaticTypeObjects.Dragon };
        weaknesses = new List<Type> { StaticTypeObjects.Steel, StaticTypeObjects.Poison };
        resistances = new List<Type> { StaticTypeObjects.Fighting, StaticTypeObjects.Bug, StaticTypeObjects.Dark };
        strengths = new List<Type> { StaticTypeObjects.Fighting, StaticTypeObjects.Dark, StaticTypeObjects.Dragon };
    }

}