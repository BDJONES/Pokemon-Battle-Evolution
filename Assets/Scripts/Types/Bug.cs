using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bug : Type
{
    public override void InitializeValues()
    {
        typeName = "Bug";
        immunities = new List<Type> { };
        weaknesses = new List<Type> { StaticTypeObjects.Fire, StaticTypeObjects.Flying };
        strengths = new List<Type> { StaticTypeObjects.Grass, StaticTypeObjects.Psychic, StaticTypeObjects.Dark };
    }
}