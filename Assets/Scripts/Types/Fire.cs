using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Fire : Type
{
    public override void InitializeValues()
    {
        typeName = "Fire";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Ground, StaticTypeObjects.Rock, StaticTypeObjects.Water };
        resistances = new List<Type> { StaticTypeObjects.Fairy, StaticTypeObjects.Fire, StaticTypeObjects.Grass, StaticTypeObjects.Ice, StaticTypeObjects.Steel };
        strengths = new List<Type> { StaticTypeObjects.Bug, StaticTypeObjects.Grass, StaticTypeObjects.Ice, StaticTypeObjects.Steel };
    }
}