using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Ground : IType
{
    public string typeName
    {
        get
        {
            return "Ground";
        }
    }
    public List<IType> immunities
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Electric };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> weaknesses
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Water, StaticTypeObjects.Ice, StaticTypeObjects.Grass };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Rock, StaticTypeObjects.Poison };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Electric, StaticTypeObjects.Rock, StaticTypeObjects.Fire, StaticTypeObjects.Steel, StaticTypeObjects.Poison };
        }
        set => throw new System.NotImplementedException();
    }
}