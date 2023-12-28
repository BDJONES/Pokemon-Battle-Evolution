using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Normal : IType
{
    public string typeName {
        get {
            return "Normal";
        } 
    }
    public List<IType> immunities { 
        get {
            return new List<IType> { StaticTypeObjects.Ghost };
        } 
        set => throw new System.NotImplementedException(); 
    }
    public List<IType> weaknesses { 
        get { 
            return new List<IType> { StaticTypeObjects.Fighting };
        } 
        set => throw new System.NotImplementedException(); 
    }
    public List<IType> resistances {
        get { 
            return new List<IType>();
        }
        set => throw new System.NotImplementedException(); 
    }
    public List<IType> strengths {
        get {
            return new List<IType>();
        }
        set => throw new System.NotImplementedException(); 
    }
}