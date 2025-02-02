using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latios : Pokemon
{
    private Latios()
    {
        this.speciesName = "Latios";
        this.nickname = "Latios";
        this.level = 100;
        this.gender = Gender.Male;
        this.baseHP = 80;
        this.baseAttack = 90;
        this.baseDefense = 80;
        this.baseSpecialAttack = 130;
        this.baseSpecialDefense = 110;
        this.baseSpeed = 110;
    }

    protected override void UpdatePokemonInfo(GameState state)
    {
        if (state == GameState.LoadingPokemonInfo)
        {
            // Must assign Scriptable Objects in Start Function
            this.abilityList = new List<Ability>
            {
                ScriptableObject.CreateInstance<Levitate>()
            };
            this.ability = this.abilityList[0];
            // Get the user of this ability to the Ability
            this.ability.abilityUser = this;
            this.ability.InitializeAbility();
            this.Type1 = StaticTypeObjects.Dragon;
            this.Type2 = StaticTypeObjects.Psychic;
            this.heldItem = new Leftovers();
            this.heldItem.SetHolder(this);
            this.moveSet = new List<Attack>
            {
                new DracoMeteor(),
                new Psyshock(),
                new AuraSphere(),
                new CalmMind()
            };
            this.learnSet = new List<Attack>
            {
                new DracoMeteor(),
                new Psyshock(),
                new AuraSphere(),
                new CalmMind()
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
