#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Pokemon: MonoBehaviour
{
    protected string speciesName = null!;
    protected int level;
    protected List<Ability>? abilities;
    protected Ability ability;
    protected Gender gender;
    protected Type Type1 = null!;
    protected Type? Type2 { get; set; }
    [SerializeField]protected StatusConditions status;
    protected List<Attack> moveSet = null!;
    protected List<Attack> learnSet = null!;
    protected Item? heldItem;
    protected int baseHP;
    [SerializeField] protected int hpStat;
    protected int baseAttack;
    [SerializeField] protected int attackStat;
    protected int baseDefense;
    [SerializeField] protected int defenseStat;
    protected int baseSpecialAttack;
    [SerializeField] protected int specialAttackStat;
    protected int baseSpecialDefense;
    [SerializeField] protected int specialDefenseStat;
    protected int baseSpeed;
    [SerializeField] protected int speedStat;
    [SerializeField] protected Ivs ivs = null!;
    [SerializeField] protected Evs evs = null!;
    public int GetLevel()
    {
        return level;
    }
    public StatusConditions GetStatus()
    {
        return status;
    }
    public void SetStatus(StatusConditions newStatus)
    {
        status = newStatus;
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
}