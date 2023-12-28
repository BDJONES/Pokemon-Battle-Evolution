using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Water : IType
{
    public string typeName
    {
        get
        {
            return "Water";
        }
    }
    public List<IType> immunities
    {
        get
        {
            return new List<IType>();
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> weaknesses
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Electric, StaticTypeObjects.Grass };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.StaticTypeObjects.Water, StaticTypeObjects.Ice, StaticTypeObjects.StaticTypeObjects.Fire, StaticTypeObjects.Steel };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Rock, StaticTypeObjects.Ground, StaticTypeObjects.StaticTypeObjects.Fire };
        }
        set => throw new System.NotImplementedException();
    }
}