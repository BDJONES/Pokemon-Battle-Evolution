using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dragon : IType
{
    public string typeName
    {
        get
        {
            return "Dragon";
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
            return new List<IType> { StaticTypeObjects.Dragon, StaticTypeObjects.Ice, StaticTypeObjects.Fairy };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Fire, StaticTypeObjects.Water, StaticTypeObjects.Grass, StaticTypeObjects.Electric };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Dragon };
        }
        set => throw new System.NotImplementedException();
    }
}