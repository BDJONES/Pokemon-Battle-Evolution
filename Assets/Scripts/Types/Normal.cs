using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Normal : Type
{
    private void Awake()
    {
        typeName = "Normal";
        immunities = new List<Type> { StaticTypeObjects.Ghost };
        weaknesses = new List<Type> { StaticTypeObjects.Fighting };
        resistances = new List<Type>();
        strengths = new List<Type>();
    }
}