using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Fire : Type
{
    public Fire()
    {
        typeName = "Fire";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Water, StaticTypeObjects.Rock, StaticTypeObjects.Ground };
        resistances = new List<Type> { StaticTypeObjects.Fire, StaticTypeObjects.Fairy, StaticTypeObjects.Steel, StaticTypeObjects.Grass, StaticTypeObjects.Ice };
        strengths = new List<Type> { StaticTypeObjects.Steel, StaticTypeObjects.Grass, StaticTypeObjects.Ice, StaticTypeObjects.Bug };
    }
}