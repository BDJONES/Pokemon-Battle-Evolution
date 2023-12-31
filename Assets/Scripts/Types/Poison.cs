using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Poison : Type
{
    public Poison()
    {
        typeName = "Poison";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Ground, StaticTypeObjects.Psychic };
        resistances = new List<Type> { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Poison, StaticTypeObjects.Bug };
        strengths = new List<Type> { StaticTypeObjects.Grass, StaticTypeObjects.Fairy };
    }
}
