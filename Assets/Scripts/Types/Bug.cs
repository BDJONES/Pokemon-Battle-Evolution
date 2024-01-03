using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bug : Type
{
    private void Awake()
    {
        typeName = "Bug";
        immunities = new List<Type> { };
        weaknesses = new List<Type> { StaticTypeObjects.Fire, StaticTypeObjects.Flying };
        strengths = new List<Type> { StaticTypeObjects.Grass, StaticTypeObjects.Psychic, StaticTypeObjects.Dark };
    }
    //public List<Type> immunities
    //{
    //    get
    //    {
    //        return new List<Type>();
    //    }
    //    set => throw new System.NotImplementedException();
    //}
    //public List<Type> weaknesses
    //{
    //    get
    //    {
    //        return new List<Type> { StaticTypeObjects.Fire, StaticTypeObjects.Flying };
    //    }
    //    set => throw new System.NotImplementedException();
    //}
    //public List<Type> resistances
    //{
    //    get
    //    {
    //        return new List<Type> { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Ground };
    //    }
    //    set => throw new System.NotImplementedException();
    //}
    //public List<Type> strengths
    //{
    //    get
    //    {
    //        return new List<Type> { StaticTypeObjects.Grass, StaticTypeObjects.Psychic, StaticTypeObjects.Dark };
    //    }
    //    set => throw new System.NotImplementedException();
    //}
}