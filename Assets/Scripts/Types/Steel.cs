using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Steel : Type
{
    public Steel()
    {
        typeName = "Steel";
        immunities = new List<Type> { StaticTypeObjects.Poison };
        weaknesses = new List<Type> { StaticTypeObjects.Ground, StaticTypeObjects.Fighting, StaticTypeObjects.Fire };
        resistances = new List<Type> { StaticTypeObjects.Steel, StaticTypeObjects.Flying, StaticTypeObjects.Grass, StaticTypeObjects.Dragon, StaticTypeObjects.Psychic, StaticTypeObjects.Normal, StaticTypeObjects.Rock, StaticTypeObjects.Fairy, StaticTypeObjects.Bug, StaticTypeObjects.Ice };
        strengths = new List<Type> { StaticTypeObjects.Ice, StaticTypeObjects.Rock, StaticTypeObjects.Fairy };
    }
}