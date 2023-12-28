using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Flying : IType
{
    public string typeName
    {
        get
        {
            return "Flying";
        }
    }
    public List<IType> immunities
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Ground };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> weaknesses
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Ice, StaticTypeObjects.Electric };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Bug };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Fighting, StaticTypeObjects.Grass, StaticTypeObjects.Bug };
        }
        set => throw new System.NotImplementedException();
    }
}