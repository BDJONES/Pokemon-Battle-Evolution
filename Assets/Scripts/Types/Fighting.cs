using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fighting : Type
{
    private void Awake()
    {
        typeName = "Fighting";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Psychic, StaticTypeObjects.Fairy, StaticTypeObjects.Flying };
        resistances = new List<Type> { StaticTypeObjects.Dark, StaticTypeObjects.Rock, StaticTypeObjects.Bug };
        strengths = new List<Type> { StaticTypeObjects.Normal, StaticTypeObjects.Rock, StaticTypeObjects.Steel, StaticTypeObjects.Dark };
    }
}
