using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikachu : Pokemon
{
    public Pikachu()
    {
        this.speciesName = "Pikachu";
        this.nickname = "Pikachu";
        this.level = 100;
        this.gender = Gender.Male;
        this.baseHP = 45;
        this.baseAttack = 80;
        this.baseDefense = 50;
        this.baseSpecialAttack = 75;
        this.baseSpecialDefense = 60;
        this.baseSpeed = 120;
    }

    protected override void UpdatePokemonInfo(GameState state)
    {
        if (state == GameState.LoadingPokemonInfo)
        {
            // Must assign Scriptable Objects in Start Function
            this.abilityList = new List<Ability>
            {
                ScriptableObject.CreateInstance<Static>()
            };
            this.ability = this.abilityList[0];
            // Get the user of this ability to the Ability
            this.ability.abilityUser = this;
            this.ability.InitializeAbility();
            this.Type1 = StaticTypeObjects.Electric;
            this.Type2 = null;
            this.heldItem = new LightBall();
            this.heldItem.SetHolder(this);
            this.moveSet = new List<Attack>
            {
                new Thunderbolt(),
                new BrickBreak(),
                new Nuzzle(),
                new DrainingKiss()
            };
            this.learnSet = new List<Attack>
            {
                new Thunderbolt(),
                new BrickBreak(),
                new Nuzzle(),
                new DrainingKiss()
            };
            this.ivs = new Ivs();
            this.evs = new Evs();
            // Assigning the actual values to the stats as opposed to the Base Stats
            // Math comes from this website: https://pokemon.fandom.com/wiki/Statistics
            this.attackStat = Mathf.FloorToInt(0.01f * (2 * this.baseAttack + this.ivs.attack + Mathf.FloorToInt(0.25f * evs.attack)) * this.level) + 5;
            this.defenseStat = Mathf.FloorToInt(0.01f * (2 * this.baseDefense + this.ivs.defense + Mathf.FloorToInt(0.25f * evs.defense)) * this.level) + 5;
            this.specialAttackStat = Mathf.FloorToInt(0.01f * (2 * this.baseSpecialAttack + this.ivs.specialAttack + Mathf.FloorToInt(0.25f * evs.specialAttack)) * this.level) + 5;
            this.specialDefenseStat = Mathf.FloorToInt(0.01f * (2 * this.baseSpecialDefense + this.ivs.specialDefense + Mathf.FloorToInt(0.25f * evs.specialDefense)) * this.level) + 5;
            this.speedStat = Mathf.FloorToInt(0.01f * (2 * this.baseSpeed + this.ivs.speed + Mathf.FloorToInt(0.25f * evs.speed)) * this.level) + 5;
            if (!IsServer) return;
            this.hpStat.Value = GetMaxHPStat();
        }
    }
}
