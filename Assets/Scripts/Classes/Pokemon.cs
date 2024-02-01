#nullable enable

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Pokemon: MonoBehaviour
{
    protected string speciesName = null!;
    [SerializeField] protected string nickname = null!;
    protected int level;
    protected List<Ability> abilityList= null!;
    [SerializeField] protected Ability ability = null!;
    [SerializeField] protected Gender gender;
    [SerializeField] protected Type Type1 = null!;
    protected Type? Type2;
    [SerializeField] private StatusConditions status = StatusConditions.Healthy;
    public StatusConditions Status { 
        get
        {
            return status;
        }
        set
        {
            status = value;
            this.StatusEffect();
        }
    }
    [SerializeField] protected List<Attack> moveSet = null!;
    [SerializeField] protected List<Attack> learnSet = null!;
    [SerializeField] protected Item? heldItem;
    protected int baseHP;
    [SerializeField] protected int hpStat;
    protected int baseAttack;
    [SerializeField] protected int attackStat;
    [SerializeField] private int attackStage = 0;
    [SerializeField]
    public int AttackStage
    {
        get
        {
            return attackStage;
        }
        set
        {
            
            if (value > 6 || value < -6)
            {
                Debug.Log("This stat can't be lowered any further");
                return;
            }
            attackStage = value;
            //Debug.Log($"Attack Stage = {attackStage}");
            //Debug.Log($"Base Attack = {baseAttack}");
            //Debug.Log($"Attack IVs = {ivs.attack}");
            //Debug.Log($"Attack Evs = {evs.attack}");
            //Debug.Log($"Level = {level}");
            attackStat = Mathf.FloorToInt(0.01f * (2 * baseAttack + ivs.attack + Mathf.FloorToInt(0.25f * evs.attack)) * level) + 5;
            attackStat = Mathf.FloorToInt(attackStat * stageConversionDictionary[attackStage]);
        }
    }
    protected int baseDefense;
    [SerializeField] protected int defenseStat;
    [SerializeField] private int defenseStage = 0;
    public int DefenseStage
    {
        get
        {
            return defenseStage;
        }
        set
        {

            if (value > 6 || value < -6)
            {
                Debug.Log("This stat can't be lowered any further");
                return;
            }
            defenseStage = value;
            defenseStat = Mathf.FloorToInt(0.01f * (2 * baseDefense + ivs.defense + Mathf.FloorToInt(0.25f * evs.defense)) * level) + 5;
            defenseStat = Mathf.FloorToInt(defenseStat * stageConversionDictionary[defenseStage]);
        }
    }
    protected int baseSpecialAttack;
    [SerializeField] protected int specialAttackStat;
    [SerializeField] private int specialAttackStage = 0;
    public int SpecialAttackStage
    {
        get
        {
            return specialAttackStage;
        }
        set
        {

            if (value > 6 || value < -6)
            {
                Debug.Log("This stat can't be lowered any further");
                return;
            }
            specialAttackStage = value;
            specialAttackStat = Mathf.FloorToInt(0.01f * (2 * baseSpecialAttack + ivs.specialAttack + Mathf.FloorToInt(0.25f * evs.specialAttack)) * level) + 5;
            specialAttackStat = Mathf.FloorToInt(specialAttackStat * stageConversionDictionary[specialAttackStage]);
        }
    }
    protected int baseSpecialDefense;
    [SerializeField] protected int specialDefenseStat;
    [SerializeField] private int specialDefenseStage = 0;
    public int SpecialDefenseStage
    {
        get
        {
            return specialDefenseStage;
        }
        set
        {

            if (value > 6 || value < -6)
            {
                Debug.Log("This stat can't be lowered any further");
                return;
            }
            specialDefenseStage = value;
            specialDefenseStat = Mathf.FloorToInt(0.01f * (2 * baseSpecialDefense + ivs.specialDefense + Mathf.FloorToInt(0.25f * evs.specialDefense)) * level) + 5;
            specialDefenseStat = Mathf.FloorToInt(specialDefenseStat * stageConversionDictionary[specialDefenseStage]);
        }
    }
    protected int baseSpeed;
    [SerializeField] protected int speedStat;
    [SerializeField] private int speedStage = 0;
    public int SpeedStage
    {
        get
        {
            return speedStage;
        }
        set
        {

            if (value > 6 || value < -6)
            {
                Debug.Log("This stat can't be lowered any further");
                return;
            }
            speedStage = value;
            speedStat = Mathf.FloorToInt(0.01f * (2 * baseSpeed + ivs.speed + Mathf.FloorToInt(0.25f * evs.speed)) * level) + 5;
            speedStat = Mathf.FloorToInt(speedStat * stageConversionDictionary[speedStage]);
        }
    }
    [SerializeField] protected Ivs ivs = null!;
    [SerializeField] protected Evs evs = null!;
    
    // Stage Conversions resource: https://bulbapedia.bulbagarden.net/wiki/Stat_modifier
    private readonly Dictionary<int, float> stageConversionDictionary = new()
    {
        { 6, 4f },
        { 5, 3.5f },
        { 4, 3f },
        { 3, 2.5f },
        { 2, 2f },
        { 1, 1.5f },
        { 0, 1f },
        { -1, 0.67f },
        { -2, 0.5f },
        { -3, 0.4f },
        { -4, 0.33f },
        { -5, 0.29f },
        { -6, 0.25f }
    };
    public string GetSpeciesName()
    {
        return speciesName;
    }
    public string GetNickname()
    {
        return nickname;
    }
    public int GetLevel()
    {
        return level;
    }
    public Ability GetAbility()
    {
        return ability;
    }
    public List<Ability> GetAbilityList()
    {
        return abilityList;
    }
    public Gender GetGender()
    {
        return gender;
    }
    public Type GetType1()
    {
        return Type1;
    }
    public Type? GetType2()
    {
        return Type2;
    }
    public bool IsHealthy()
    {
        return this.status == StatusConditions.Healthy;
    }
    public bool IsBurned()
    {
        return this.status == StatusConditions.Burn;
    }
    public bool IsFrozen()
    {
        return this.status == StatusConditions.Freeze;
    }
    public bool IsParalyzed()
    {
        return this.status == StatusConditions.Paralysis;
    }
    public bool IsPoisoned()
    {
        return this.status == StatusConditions.Poison;
    }
    public bool IsBadlyPoisoned()
    {
        return this.status == StatusConditions.BadPoison;
    }
    public bool IsAsleep()
    {
        return this.status == StatusConditions.Asleep;
    }
    public List<Attack> GetMoveset()
    {
        return moveSet;
    }
    public List<Attack> GetLearnset()
    {
        return learnSet;
    }
    public Item? GetItem()
    {
        return heldItem;
    }
    public int GetBaseHP()
    {
        return baseHP;
    }
    public int GetBaseAttack()
    {
        return baseAttack;
    }
    public int GetBaseDefense()
    {
        return baseDefense;
    }
    public int GetBaseSpecialAttack()
    {
        return baseSpecialAttack;
    }
    public int GetBaseSpecialDefense()
    {
        return baseSpecialDefense;
    }
    public int GetBaseSpeed()
    {
        return baseSpeed;
    }
    public int GetMaxHPStat()
    {
        return Mathf.FloorToInt(0.01f * (2 * this.baseHP + this.ivs.hp + Mathf.FloorToInt(0.25f * evs.hp)) * this.level) + this.level + 10;
    }
    public int GetHPStat()
    {
        return hpStat;
    }
    public int GetAttackStat()
    {
        return attackStat;
    }
    public int GetDefenseStat()
    {
        return defenseStat;
    }
    public int GetSpecialAttackStat()
    {
        return specialAttackStat;
    }
    public int GetSpecialDefenseStat()
    {
        return specialDefenseStat;
    }
    public int GetSpeedStat()
    {
        return speedStat;
    }
    public void RemoveItem()
    {
        heldItem = null;
    }
    public void SetNickname(string newNickname)
    {
        nickname = newNickname;
    }
    public void SetHPStat(int newHP)
    {
        hpStat = newHP;
    }
    public void SetAttackStat(int newAttack)
    {
        attackStat = newAttack;
    }
    public void SetDefenseStat(int newDefense)
    {
        defenseStat = newDefense;
    }
    public void SetSpecialAttackStat(int newSpecialAttack)
    {
        specialAttackStat = newSpecialAttack;
    }
    public void SetSpecialDefenseStat(int newSpecialDefense)
    {
        specialDefenseStat = newSpecialDefense;
    }
    public void SetSpeedStat(int newSpeed)
    {
        speedStat = newSpeed;
    }
    public void StatusEffect()
    {
        if (this.status == StatusConditions.Burn)
        {
            // Fix Burn effect based on info here: https://the-episodes-and-movie-yveltal-and-more.fandom.com/wiki/Burn_(Pok%C3%A9mon_Status_Condition)#Generation_7
            // Damage Calculation already takes burn into effect
            return;

        }
        else if (this.status == StatusConditions.Paralysis)
        {
            this.SetSpeedStat(Mathf.FloorToInt(0.01f * (2 * this.baseSpeed + this.ivs.speed + Mathf.FloorToInt(0.25f * this.evs.speed)) * level) + 5);
            this.SetSpeedStat(this.speedStat / 2);
            this.SetSpeedStat(Mathf.FloorToInt(this.speedStat * stageConversionDictionary[speedStage]));
        }
    }
}