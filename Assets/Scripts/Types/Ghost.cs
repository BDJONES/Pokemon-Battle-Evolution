using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ghost : IType
{
    public string typeName
    {
        get
        {
            return "Ghost";
        }
    }
    public List<IType> immunities
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Normal, StaticTypeObjects.Fighting };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> weaknesses
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Ghost, StaticTypeObjects.Dark };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Bug, StaticTypeObjects.Poison };
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