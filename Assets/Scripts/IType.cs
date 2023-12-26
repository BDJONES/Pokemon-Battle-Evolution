using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class IType : MonoBehaviour
{
    public string typeName;
    public List<IType> immunities;
    public List<IType> weaknesses;
    public List<IType> resistances;
    public List<IType> strengths;
    public static IType Normal = new ()
    {
        typeName = "Normal",
        immunities = { Ghost },
        weaknesses = { Fighting },
        resistances = { },
        strengths = { }
    };
    public static IType Fire = new ()
    {
        typeName = "Fire",
        immunities = { },
        weaknesses = { Water, Rock, Ground },
        resistances = { Fire, Fairy, Steel, Grass, Ice },
        strengths = { Steel, Grass, Ice, Bug }
    };
    public static IType Water = new ()
    {
        typeName = "Water",
        immunities = { },
        weaknesses = { Electric, Grass },
        resistances = { Water, Ice, Fire, Steel },
        strengths = { Rock, Ground, Fire }
    };
    public static IType Electric = new () 
    {
        typeName = "Electric",
        immunities = { },
        weaknesses = { Ground },
        resistances = { Steel, Flying, Electric },
        strengths = { Water, Flying }
    };
    public static IType Grass = new ()
    {
        typeName = "Grass",
        immunities = { },
        weaknesses = { Fire, Flying, Ice, Poison, Bug },
        resistances = { Electric, Water, Ground, Grass },
        strengths = { Rock, Ground, Water }
    };
    public static IType Ice = new ()
    {
        typeName = "Ice",
        immunities = { },
        weaknesses = { Fire, Fighting, Rock, Steel },
        resistances = { Ice },
        strengths = { Dragon, Grass, Flying, Ground }
    };
    public static IType Fighting = new ()
    {
        typeName = "Fighting",
        immunities = { },
        weaknesses = { Psychic, Fairy, Flying },
        resistances = { Dark, Rock, Bug },
        strengths = { Normal, Rock, Steel, Dark }
    };
    public static IType Poison = new ()
    {
        typeName = "Poison",
        immunities = { },
        weaknesses = { Ground, Psychic },
        resistances = { Grass, Fighting, Poison, Bug },
        strengths = { Grass, Fairy }
    };
    public static IType Ground = new ()
    {
        typeName = "Ground",
        immunities = { Electric },
        weaknesses = { Water, Ice, Grass },
        resistances = { Rock, Poison },
        strengths = { Electric, Rock, Fire, Steel, Poison }
    };
    public static IType Flying = new ()
    {
        typeName = "Flying",
        immunities = { Ground },
        weaknesses = { Ice, Electric },
        resistances = { Grass, Fighting, Bug },
        strengths = { Fighting, Grass, Bug }
    };
    public static IType Psychic = new ()
    {
        typeName = "Psychic",
        immunities = { },
        weaknesses = { Bug, Dark, Ghost },
        resistances = { Psychic, Fighting },
        strengths = { Fighting, Poison }
    };
    public static IType Bug = new ()
    {
        typeName = "Bug",
        immunities = { },
        weaknesses = { Fire, Flying },
        resistances = { Grass, Fighting, Ground },
        strengths = { Grass, Psychic, Dark }
    };
    public static IType Rock = new ()
    {
        typeName = "Rock",
        immunities = { },
        weaknesses = { Ground, Grass, Water, Fighting },
        resistances = { Flying, Fire, Normal, Poison },
        strengths = { Fire, Flying, Ice, Bug }
    };
    public static IType Ghost = new ()
    {
        typeName = "Ghost",
        immunities = { Normal, Fighting },
        weaknesses = { Ghost, Dark },
        resistances = { Bug, Poison },
        strengths = { Psychic, Ghost }
    };
    public static IType Dragon = new ()
    {
        typeName = "Dragon",
        immunities = { },
        weaknesses = { Dragon, Ice, Fairy },
        resistances = { Fire, Water, Grass, Electric },
        strengths = { Dragon }
    };
    public static IType Dark = new ()
    {
        typeName = "Dark",
        immunities = { Psychic },
        weaknesses = { Fighting, Bug, Fairy },
        resistances = { Ghost, Dark },
        strengths = { Psychic, Ghost }
    };
    public static IType Steel = new ()
    {
        typeName = "Steel",
        immunities = { Poison },
        weaknesses = { Ground, Fighting, Fire },
        resistances = { Steel, Flying, Grass, Dragon, Psychic, Normal, Rock, Fairy, Bug, Ice },
        strengths = { Water, Flying }
    };
    public static IType Fairy = new ()
    {
        typeName = "Fairy",
        immunities = { Dragon },
        weaknesses = { Steel, Poison },
        resistances = { Fighting, Bug, Dark },
        strengths = { Fighting, Dark, Dragon }
    };
}