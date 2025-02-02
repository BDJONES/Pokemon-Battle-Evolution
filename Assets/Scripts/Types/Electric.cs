﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Electric : Type
{
    public override void InitializeValues()
    {
        typeName = "Electric";
        immunities = new List<Type>();
        weaknesses = new List<Type> { StaticTypeObjects.Ground };
        resistances = new List<Type> { StaticTypeObjects.Steel, StaticTypeObjects.Flying, StaticTypeObjects.Electric };
        strengths = new List<Type> { StaticTypeObjects.Water, StaticTypeObjects.Flying };
    }
}