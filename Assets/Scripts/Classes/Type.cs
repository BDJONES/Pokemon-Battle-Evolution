using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;


public abstract class Type
{
    public string typeName = null!;
    public List<Type> immunities = null!;
    public List<Type> weaknesses = null!;
    public List<Type> resistances = null!;
    public List<Type> strengths = null!;
    public abstract void InitializeValues();
}