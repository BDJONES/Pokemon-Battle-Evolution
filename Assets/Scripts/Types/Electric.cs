using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Electric : IType
{
    public string typeName
    {
        get
        {
            return "Electric";
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
            return new List<IType> { StaticTypeObjects.Ground };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Steel, StaticTypeObjects.Flying, StaticTypeObjects.StaticTypeObjects.Electric };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Water, StaticTypeObjects.Flying };
        }
        set => throw new System.NotImplementedException();
    }
}