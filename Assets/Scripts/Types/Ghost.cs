using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ghost : Type
{
    private void Awake()
    {
        typeName = "Ghost";
        immunities = new List<Type> { StaticTypeObjects.Normal, StaticTypeObjects.Fighting };
        weaknesses = new List<Type> { StaticTypeObjects.Ghost, StaticTypeObjects.Dark };
        resistances = new List<Type> { StaticTypeObjects.Bug, StaticTypeObjects.Poison };
        strengths = new List<Type> { StaticTypeObjects.Psychic, StaticTypeObjects.Ghost };
    }
}