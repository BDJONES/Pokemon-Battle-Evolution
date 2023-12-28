using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Psychic : IType
{
    public string typeName
    {
        get
        {
            return "Psychic";
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
            return new List<IType> { StaticTypeObjects.Bug, StaticTypeObjects.Dark, StaticTypeObjects.Ghost };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Psychic, StaticTypeObjects.Fighting };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Fighting, StaticTypeObjects.Poison };
        }
        set => throw new System.NotImplementedException();
    }
}