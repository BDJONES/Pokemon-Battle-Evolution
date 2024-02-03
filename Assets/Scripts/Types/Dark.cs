using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dark : Type
{
    public override void InitializeValues()
    {
        typeName = "Dark";
        immunities = new List<Type> { StaticTypeObjects.Psychic };
        weaknesses = new List<Type> { StaticTypeObjects.Fighting, StaticTypeObjects.Bug, StaticTypeObjects.Fairy };
        resistances = new List<Type> { StaticTypeObjects.Ghost, StaticTypeObjects.Dark };
        strengths = new List<Type> { StaticTypeObjects.Psychic, StaticTypeObjects.Ghost };
    }
}