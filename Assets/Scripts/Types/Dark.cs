using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dark : IType
{
    public string typeName
    {
        get
        {
            return "Dark";
        }
    }
    public List<IType> immunities
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Psychic };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> weaknesses
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Fighting, StaticTypeObjects.Bug, StaticTypeObjects.Fairy };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Ghost, StaticTypeObjects.Dark };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Psychic, StaticTypeObjects.Ghost };
        }
        set => throw new System.NotImplementedException();
    }
}