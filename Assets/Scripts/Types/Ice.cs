using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ice : IType
{
    public string typeName
    {
        get
        {
            return "Ice";
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
            return new List<IType> { StaticTypeObjects.Fire, StaticTypeObjects.Fighting, StaticTypeObjects.Rock, StaticTypeObjects.Steel };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> resistances
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Ice };
        }
        set => throw new System.NotImplementedException();
    }
    public List<IType> strengths
    {
        get
        {
            return new List<IType> { StaticTypeObjects.Dragon, StaticTypeObjects.Grass, StaticTypeObjects.Flying, StaticTypeObjects.Ground };
        }
        set => throw new System.NotImplementedException();
    }
}
