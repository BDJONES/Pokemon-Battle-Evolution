using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoketCreature : Pokemon
{
    private PoketCreature()
    {
        this.speciesName = "Poket Creature";
        this.nickname = this.speciesName;
        this.level = 100;
        this.gender = Gender.Male;
        this.Status = StatusConditions.Healthy;
        this.baseHP = 60;
        this.baseAttack = 60;
        this.baseDefense = 60;
        this.baseSpecialAttack = 60;
        this.baseSpecialDefense = 60;
        this.baseSpeed = 60;
    }

    private void Start()
    {
        // Must assign Scriptable Objects in Start Function
        this.abilityList = new List<Ability>
        {
            ScriptableObject.CreateInstance<Intimidate>()
        };
        this.ability = this.abilityList[0];
        // Get the user of this ability to the Ability
        this.ability.abilityUser = this;
        this.Type1 = StaticTypeObjects.Fire;
        this.Type2 = null;
        this.moveSet = new List<Attack>
        { 
            new Tackle(),
            new Flamethrower(),
            new Earthquake(),
            new ThunderWave()
        };
        this.learnSet = new List<Attack>
        {
            new Flamethrower(),
            new Tackle(),
            new Earthquake(),
            new ThunderWave()
        };        
        this.ivs = new Ivs();
        this.evs = new Evs();
        // Assigning the actual values to the stats as opposed to the Base Stats
        // Math comes from this website: https://pokemon.fandom.com/wiki/Statistics
        this.hpStat = GetMaxHPStat();
        this.attackStat = Mathf.FloorToInt(0.01f * (2 * this.baseAttack + this.ivs.attack + Mathf.FloorToInt(0.25f * evs.attack)) * this.level) + 5;
        this.defenseStat = Mathf.FloorToInt(0.01f * (2 * this.baseDefense + this.ivs.defense + Mathf.FloorToInt(0.25f * evs.defense)) * this.level) + 5;
        this.specialAttackStat = Mathf.FloorToInt(0.01f * (2 * this.baseSpecialAttack + this.ivs.specialAttack + Mathf.FloorToInt(0.25f * evs.specialAttack)) * this.level) + 5;
        this.specialDefenseStat = Mathf.FloorToInt(0.01f * (2 * this.baseSpecialDefense + this.ivs.specialDefense + Mathf.FloorToInt(0.25f * evs.specialDefense)) * this.level) + 5;
        this.speedStat = Mathf.FloorToInt(0.01f * (2 * this.baseSpeed + this.ivs.speed + Mathf.FloorToInt(0.25f * evs.speed)) * this.level) + 5;

    }
}
