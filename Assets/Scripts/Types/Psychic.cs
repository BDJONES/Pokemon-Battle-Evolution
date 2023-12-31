using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Psychic : Type
{
    public Psychic()
    {
        typeName = "Psychic";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Bug, StaticTypeObjects.Dark, StaticTypeObjects.Ghost };
        resistances = new List<Type> { StaticTypeObjects.Psychic, StaticTypeObjects.Fighting };
        strengths = new List<Type> { StaticTypeObjects.Fighting, StaticTypeObjects.Poison };
    }
}