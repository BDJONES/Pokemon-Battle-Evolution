using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Poison : IType
{
    public string typeName
    {
        get
        {
            return "Poison";
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
            return new List<IType> { StaticTypeObjects.Ground, StaticTypeObjects.Psychic };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Poison, StaticTypeObjects.Bug };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Grass, StaticTypeObjects.Fairy };
        }
        set => throw new System.NotImplementedException();
    }
}
