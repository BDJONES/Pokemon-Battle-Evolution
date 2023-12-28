using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rock : IType
{
    public string typeName
    {
        get
        {
            return "Rock";
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
            return new List<IType> { StaticTypeObjects.Ground, StaticTypeObjects.Grass, StaticTypeObjects.Water, StaticTypeObjects.Fighting };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Flying, StaticTypeObjects.Fire, StaticTypeObjects.Normal, StaticTypeObjects.Poison };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Fire, StaticTypeObjects.Flying, StaticTypeObjects.Ice, StaticTypeObjects.Bug };
        }
        set => throw new System.NotImplementedException();
    }
}