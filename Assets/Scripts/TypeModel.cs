using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TypeModel : MonoBehaviour
{
    //var anon = new
    //{
    //    Id = 1,
    //    Name = "Maclane",
    //};

    public string typeName;
    public List<TypeModel> immunities;
    public List<TypeModel> weaknesses;
    public List<TypeModel> resistances;
    public List<TypeModel> strengths;
    public static TypeModel Normal = new ()
    {
        typeName = "Normal",
        immunities = { Ghost },
        weaknesses = { Fighting },
        resistances = { },
        strengths = { }
    };
    public static TypeModel Fire = new ()
    {
        typeName = "Fire",
        immunities = { },
        weaknesses = { Water, Rock, Ground },
        resistances = { Fire, Fairy, Steel, Grass, Ice },
        strengths = { Steel, Grass, Ice, Bug }
    };
    public static TypeModel Water = new ()
    {
        typeName = "Water",
        immunities = { },
        weaknesses = { Electric, Grass },
        resistances = { Water, Ice, Fire, Steel },
        strengths = { Rock, Ground, Fire }
    };
    public static TypeModel Electric = new () 
    {
        typeName = "Electric",
        immunities = { },
        weaknesses = { Ground },
        resistances = { Steel, Flying, Electric },
        strengths = { Water, Flying }
    };
    public static TypeModel Grass = new ()
    {
        typeName = "Grass",
        immunities = { },
        weaknesses = { Fire, Flying, Ice, Poison, Bug },
        resistances = { Electric, Water, Ground, Grass },
        strengths = { Rock, Ground, Water }
    };
    public static TypeModel Ice = new ()
    {
        typeName = "Ice",
        immunities = { },
        weaknesses = { Fire, Fighting, Rock, Steel },
        resistances = { Ice },
        strengths = { Dragon, Grass, Flying, Ground }
    };
    public static TypeModel Fighting = new ()
    {
        typeName = "Fighting",
        immunities = { },
        weaknesses = { Psychic, Fairy, Flying },
        resistances = { Dark, Rock, Bug },
        strengths = { Normal, Rock, Steel, Dark }
    };
    public static TypeModel Poison = new ()
    {
        typeName = "Poison",
        immunities = { },
        weaknesses = { Ground, Psychic },
        resistances = { Grass, Fighting, Poison, Bug },
        strengths = { Grass, Fairy }
    };
    public static TypeModel Ground = new ()
    {
        typeName = "Ground",
        immunities = { Electric },
        weaknesses = { Water, Ice, Grass },
        resistances = { Rock, Poison },
        strengths = { Electric, Rock, Fire, Steel, Poison }
    };
    public static TypeModel Flying = new ()
    {
        typeName = "Flying",
        immunities = { Ground },
        weaknesses = { Ice, Electric },
        resistances = { Grass, Fighting, Bug },
        strengths = { Fighting, Grass, Bug }
    };
    public static TypeModel Psychic = new ()
    {
        typeName = "Psychic",
        immunities = { },
        weaknesses = { Bug, Dark, Ghost },
        resistances = { Psychic, Fighting },
        strengths = { Fighting, Poison }
    };
    public static TypeModel Bug = new ()
    {
        typeName = "Bug",
        immunities = { },
        weaknesses = { Fire, Flying },
        resistances = { Grass, Fighting, Ground },
        strengths = { Grass, Psychic, Dark }
    };
    public static TypeModel Rock = new ()
    {
        typeName = "Rock",
        immunities = { },
        weaknesses = { Ground, Grass, Water, Fighting },
        resistances = { Flying, Fire, Normal, Poison },
        strengths = { Fire, Flying, Ice, Bug }
    };
    public static TypeModel Ghost = new ()
    {
        typeName = "Ghost",
        immunities = { Normal, Fighting },
        weaknesses = { Ghost, Dark },
        resistances = { Bug, Poison },
        strengths = { Psychic, Ghost }
    };
    public static TypeModel Dragon = new ()
    {
        typeName = "Dragon",
        immunities = { },
        weaknesses = { Dragon, Ice, Fairy },
        resistances = { Fire, Water, Grass, Electric },
        strengths = { Dragon }
    };
    public static TypeModel Dark = new ()
    {
        typeName = "Dark",
        immunities = { Psychic },
        weaknesses = { Fighting, Bug, Fairy },
        resistances = { Ghost, Dark },
        strengths = { Psychic, Ghost }
    };
    public static TypeModel Steel = new ()
    {
        typeName = "Steel",
        immunities = { Poison },
        weaknesses = { Ground, Fighting, Fire },
        resistances = { Steel, Flying, Grass, Dragon, Psychic, Normal, Rock, Fairy, Bug, Ice },
        strengths = { Water, Flying }
    };
    public static TypeModel Fairy = new ()
    {
        typeName = "Fairy",
        immunities = { Dragon },
        weaknesses = { Steel, Poison },
        resistances = { Fighting, Bug, Dark },
        strengths = { Fighting, Dark, Dragon }
    };
}