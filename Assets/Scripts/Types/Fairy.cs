using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fairy : IType
{
    public string typeName
    {
        get
        {
            return "Fairy";
        }
    }
    public List<IType> immunities
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Dragon };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> weaknesses
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Steel, StaticTypeObjects.Poison };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Fighting, StaticTypeObjects.Bug, StaticTypeObjects.Dark };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Fighting, StaticTypeObjects.Dark, StaticTypeObjects.Dragon };
        }
        set => throw new System.NotImplementedException();
    }
}