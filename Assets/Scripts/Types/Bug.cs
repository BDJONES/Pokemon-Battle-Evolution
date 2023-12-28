using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bug : IType
{
    public string typeName
    {
        get
        {
            return "Bug";
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
            return new List<IType> { StaticTypeObjects.Fire, StaticTypeObjects.Flying };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Ground };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Grass, StaticTypeObjects.Psychic, StaticTypeObjects.Dark };
        }
        set => throw new System.NotImplementedException();
    }
}