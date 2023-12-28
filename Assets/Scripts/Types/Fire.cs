using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Fire : IType
{
    public string typeName
    {
        get
        {
            return "Fire";
        }
    }
    public List<IType> immunities
    {
        get
        {
            return new List<IType> ();
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> weaknesses
    {
        get
        {
            return new List<IType> { StaticTypeObjects.StaticTypeObjects.Water, StaticTypeObjects.Rock, StaticTypeObjects.Ground };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.StaticTypeObjects.Fire, StaticTypeObjects.Fairy, StaticTypeObjects.Steel, StaticTypeObjects.Grass, StaticTypeObjects.Ice };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Steel, StaticTypeObjects.Grass, StaticTypeObjects.Ice, StaticTypeObjects.Bug };
        }
        set => throw new System.NotImplementedException();
    }
}