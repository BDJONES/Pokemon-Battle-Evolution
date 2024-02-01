using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Flying : Type
{
    public override void InitializeValues()
    {
        typeName = "Flying";
        immunities = new List<Type> { StaticTypeObjects.Ground };
        weaknesses = new List<Type> { StaticTypeObjects.Ice, StaticTypeObjects.Electric };
        resistances = new List<Type> { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Bug };
        strengths = new List<Type> { StaticTypeObjects.Fighting, StaticTypeObjects.Grass, StaticTypeObjects.Bug };
    }
}