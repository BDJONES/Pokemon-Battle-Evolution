using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fighting : IType
{
    public string typeName
    {
        get
        {
            return "Fighting";
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
            return new List<IType> { StaticTypeObjects.Psychic, StaticTypeObjects.Fairy, StaticTypeObjects.Flying };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Dark, StaticTypeObjects.Rock, StaticTypeObjects.Bug };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Normal, StaticTypeObjects.Rock, StaticTypeObjects.Steel, StaticTypeObjects.Dark };
        }
        set => throw new System.NotImplementedException();
    }
}
