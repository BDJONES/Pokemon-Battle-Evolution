using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Grass : IType
{
    public string typeName
    {
        get
        {
            return "Grass";
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
            return new List<IType> { StaticTypeObjects.Fire, StaticTypeObjects.Flying, StaticTypeObjects.Ice, StaticTypeObjects.Poison, StaticTypeObjects.Bug };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Electric, StaticTypeObjects.Water, StaticTypeObjects.Ground, StaticTypeObjects.Grass };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Rock, StaticTypeObjects.Ground, StaticTypeObjects.Water };
        }
        set => throw new System.NotImplementedException();
    }
}
