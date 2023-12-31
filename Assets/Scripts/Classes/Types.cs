using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Type : MonoBehaviour
{
    public string typeName = null!;
    public List<Type> immunities = null!;
    public List<Type> weaknesses = null!;
    public List<Type> resistances = null!;
    public List<Type> strengths = null!;
    //static Type StaticTypeObjects.Normal = new()
    //{
    //    typeName = "StaticTypeObjects.Normal",
    //    immunities = { StaticTypeObjects.Ghost },
    //    weaknesses = { StaticTypeObjects.Fighting },
    //    resistances = { },
    //    strengths = { }
    //};
    //    public static Type StaticTypeObjects.Fire = new ()
    //    {
    //        typeName = "StaticTypeObjects.Fire",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Water, StaticTypeObjects.Rock, StaticTypeObjects.Ground },
    //        resistances = { StaticTypeObjects.Fire, StaticTypeObjects.Fairy, StaticTypeObjects.Steel, StaticTypeObjects.Grass, StaticTypeObjects.Ice },
    //        strengths = { StaticTypeObjects.Steel, StaticTypeObjects.Grass, StaticTypeObjects.Ice, StaticTypeObjects.Bug }
    //    };
    //    public static Type StaticTypeObjects.Water = new ()
    //    {
    //        typeName = "StaticTypeObjects.Water",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Electric, StaticTypeObjects.Grass },
    //        resistances = { StaticTypeObjects.Water, StaticTypeObjects.Ice, StaticTypeObjects.Fire, StaticTypeObjects.Steel },
    //        strengths = { StaticTypeObjects.Rock, StaticTypeObjects.Ground, StaticTypeObjects.Fire }
    //    };
    //    public static Type StaticTypeObjects.Electric = new () 
    //    {
    //        typeName = "StaticTypeObjects.Electric",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Ground },
    //        resistances = { StaticTypeObjects.Steel, StaticTypeObjects.Flying, StaticTypeObjects.Electric },
    //        strengths = { StaticTypeObjects.Water, StaticTypeObjects.Flying }
    //    };
    //    public static Type StaticTypeObjects.Grass = new ()
    //    {
    //        typeName = "StaticTypeObjects.Grass",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Fire, StaticTypeObjects.Flying, StaticTypeObjects.Ice, StaticTypeObjects.Poison, StaticTypeObjects.Bug },
    //        resistances = { StaticTypeObjects.Electric, StaticTypeObjects.Water, StaticTypeObjects.Ground, StaticTypeObjects.Grass },
    //        strengths = { StaticTypeObjects.Rock, StaticTypeObjects.Ground, StaticTypeObjects.Water }
    //    };
    //    public static Type StaticTypeObjects.Ice = new ()
    //    {
    //        typeName = "StaticTypeObjects.Ice",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Fire, StaticTypeObjects.Fighting, StaticTypeObjects.Rock, StaticTypeObjects.Steel },
    //        resistances = { StaticTypeObjects.Ice },
    //        strengths = { StaticTypeObjects.Dragon, StaticTypeObjects.Grass, StaticTypeObjects.Flying, StaticTypeObjects.Ground }
    //    };
    //    public static Type StaticTypeObjects.Fighting = new ()
    //    {
    //        typeName = "StaticTypeObjects.Fighting",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Psychic, StaticTypeObjects.Fairy, StaticTypeObjects.Flying },
    //        resistances = { StaticTypeObjects.Dark, StaticTypeObjects.Rock, StaticTypeObjects.Bug },
    //        strengths = { StaticTypeObjects.Normal, StaticTypeObjects.Rock, StaticTypeObjects.Steel, StaticTypeObjects.Dark }
    //    };
    //    public static Type StaticTypeObjects.Poison = new ()
    //    {
    //        typeName = "StaticTypeObjects.Poison",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Ground, StaticTypeObjects.Psychic },
    //        resistances = { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Poison, StaticTypeObjects.Bug },
    //        strengths = { StaticTypeObjects.Grass, StaticTypeObjects.Fairy }
    //    };
    //    public static Type StaticTypeObjects.Ground = new ()
    //    {
    //        typeName = "StaticTypeObjects.Ground",
    //        immunities = { StaticTypeObjects.Electric },
    //        weaknesses = { StaticTypeObjects.Water, StaticTypeObjects.Ice, StaticTypeObjects.Grass },
    //        resistances = { StaticTypeObjects.Rock, StaticTypeObjects.Poison },
    //        strengths = { StaticTypeObjects.Electric, StaticTypeObjects.Rock, StaticTypeObjects.Fire, StaticTypeObjects.Steel, StaticTypeObjects.Poison }
    //    };
    //    public static Type StaticTypeObjects.Flying = new ()
    //    {
    //        typeName = "StaticTypeObjects.Flying",
    //        immunities = { StaticTypeObjects.Ground },
    //        weaknesses = { StaticTypeObjects.Ice, StaticTypeObjects.Electric },
    //        resistances = { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Bug },
    //        strengths = { StaticTypeObjects.Fighting, StaticTypeObjects.Grass, StaticTypeObjects.Bug }
    //    };
    //    public static Type StaticTypeObjects.Psychic = new ()
    //    {
    //        typeName = "StaticTypeObjects.Psychic",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Bug, StaticTypeObjects.Dark, StaticTypeObjects.Ghost },
    //        resistances = { StaticTypeObjects.Psychic, StaticTypeObjects.Fighting },
    //        strengths = { StaticTypeObjects.Fighting, StaticTypeObjects.Poison }
    //    };
    //    public static Type StaticTypeObjects.Bug = new ()
    //    {
    //        typeName = "StaticTypeObjects.Bug",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Fire, StaticTypeObjects.Flying },
    //        resistances = { StaticTypeObjects.Grass, StaticTypeObjects.Fighting, StaticTypeObjects.Ground },
    //        strengths = { StaticTypeObjects.Grass, StaticTypeObjects.Psychic, StaticTypeObjects.Dark }
    //    };
    //    public static Type StaticTypeObjects.Rock = new ()
    //    {
    //        typeName = "StaticTypeObjects.Rock",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Ground, StaticTypeObjects.Grass, StaticTypeObjects.Water, StaticTypeObjects.Fighting },
    //        resistances = { StaticTypeObjects.Flying, StaticTypeObjects.Fire, StaticTypeObjects.Normal, StaticTypeObjects.Poison },
    //        strengths = { StaticTypeObjects.Fire, StaticTypeObjects.Flying, StaticTypeObjects.Ice, StaticTypeObjects.Bug }
    //    };
    //    public static Type StaticTypeObjects.Ghost = new ()
    //    {
    //        typeName = "StaticTypeObjects.Ghost",
    //        immunities = { StaticTypeObjects.Normal, StaticTypeObjects.Fighting },
    //        weaknesses = { StaticTypeObjects.Ghost, StaticTypeObjects.Dark },
    //        resistances = { StaticTypeObjects.Bug, StaticTypeObjects.Poison },
    //        strengths = { StaticTypeObjects.Psychic, StaticTypeObjects.Ghost }
    //    };
    //    public static Type StaticTypeObjects.Dragon = new ()
    //    {
    //        typeName = "StaticTypeObjects.Dragon",
    //        immunities = { },
    //        weaknesses = { StaticTypeObjects.Dragon, StaticTypeObjects.Ice, StaticTypeObjects.Fairy },
    //        resistances = { StaticTypeObjects.Fire, StaticTypeObjects.Water, StaticTypeObjects.Grass, StaticTypeObjects.Electric },
    //        strengths = { StaticTypeObjects.Dragon }
    //    };
    //    public static Type StaticTypeObjects.Dark = new ()
    //    {
    //        typeName = "StaticTypeObjects.Dark",
    //        immunities = { StaticTypeObjects.Psychic },
    //        weaknesses = { StaticTypeObjects.Fighting, StaticTypeObjects.Bug, StaticTypeObjects.Fairy },
    //        resistances = { StaticTypeObjects.Ghost, StaticTypeObjects.Dark },
    //        strengths = { StaticTypeObjects.Psychic, StaticTypeObjects.Ghost }
    //    };
    //    public static Type StaticTypeObjects.Steel = new ()
    //    {
    //        typeName = "StaticTypeObjects.Steel",
    //        immunities = { StaticTypeObjects.Poison },
    //        weaknesses = { StaticTypeObjects.Ground, StaticTypeObjects.Fighting, StaticTypeObjects.Fire },
    //        resistances = { StaticTypeObjects.Steel, StaticTypeObjects.Flying, StaticTypeObjects.Grass, StaticTypeObjects.Dragon, StaticTypeObjects.Psychic, StaticTypeObjects.Normal, StaticTypeObjects.Rock, StaticTypeObjects.Fairy, StaticTypeObjects.Bug, StaticTypeObjects.Ice },
    //        strengths = { StaticTypeObjects.Ice, StaticTypeObjects.Rock, StaticTypeObjects.Fairy }
    //    };
    //    public static Type StaticTypeObjects.Fairy = new ()
    //    {
    //        typeName = "StaticTypeObjects.Fairy",
    //        immunities = { StaticTypeObjects.Dragon },
    //        weaknesses = { StaticTypeObjects.Steel, StaticTypeObjects.Poison },
    //        resistances = { StaticTypeObjects.Fighting, StaticTypeObjects.Bug, StaticTypeObjects.Dark },
    //        strengths = { StaticTypeObjects.Fighting, StaticTypeObjects.Dark, StaticTypeObjects.Dragon }
    //    };
}