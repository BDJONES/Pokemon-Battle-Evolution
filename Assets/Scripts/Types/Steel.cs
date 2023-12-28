using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Steel : IType
{
    public string typeName
    {
        get
        {
            return "Steel";
        }
    }
    public List<IType> immunities
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Poison };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> weaknesses
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Ground, StaticTypeObjects.Fighting, StaticTypeObjects.Fire };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Steel, StaticTypeObjects.Flying, StaticTypeObjects.Grass, StaticTypeObjects.Dragon, StaticTypeObjects.Psychic, StaticTypeObjects.Normal, StaticTypeObjects.Rock, StaticTypeObjects.Fairy, StaticTypeObjects.Bug, StaticTypeObjects.Ice };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Ice, StaticTypeObjects.Rock, StaticTypeObjects.Fairy };
        }
        set => throw new System.NotImplementedException();
    }
}