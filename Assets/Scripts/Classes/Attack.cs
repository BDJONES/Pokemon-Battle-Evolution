using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    protected string attackName;
    protected string description;
    protected Type type = null!;
    protected AttackCategory moveCategory;
    protected int power;
    protected int accuracy;
    protected int priority;
    protected int currPowerPoints;
    protected int maxPowerPoints;
    public string GetAttackName()
    {
        return attackName;
    }
    public string GetAttackDescription()
    {
        return description;
    }
    public Type GetAttackType()
    {
        return type;
    }
    public AttackCategory GetAttackCategory()
    {
        return moveCategory;
    }
    public int GetAttackPower()
    {
        return power;
    }
    public int GetAttackAccuracy()
    {
        return accuracy;
    }
    public int GetAttackPriority()
    {
        return priority;
    }
    public int GetCurrentPP()
    {
        return currPowerPoints;
    }
    public int GetMaxPP()
    {
        return maxPowerPoints;
    }
    public virtual bool UseAttack(Pokemon attacker, Pokemon target)
    {
        if (this.accuracy == 100)
        {
            int damage = CalculateDamage(this, attacker, target);
            DealDamage(damage, attacker, target);
            if (target.GetHPStat() == 0)
            {
                return true;
            }
            TriggerEffect(attacker, target);
        }
        else
        {
            int generatedValue = Random.Range(0, 99);
            int accurateRange = 100 - this.accuracy;
            if (generatedValue < accurateRange)
            {
                Debug.Log("The Attack Missed");
                return false;
            }
            else
            {
                int damage = CalculateDamage(this, attacker, target);
                DealDamage(damage, attacker, target);
                if (target.GetHPStat() == 0)
                {
                    return true;
                }
                TriggerEffect(attacker, target);
            }
        }
        Debug.Log("The Attack Landed");
        return true;
    }
    public virtual void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        currPowerPoints -= 1;
        return;
    }
    public virtual int CalculateDamage(Attack attack, Pokemon attacker, Pokemon target)
    {
        int stab = 1; // Same Type Attack Bonus
        float typeMatchup = 1f;
        int damageRange = Random.Range(217, 255);
        int damage;
        // Damage Formula comes from this attack https://www.math.miami.edu/~jam/azure/compendium/battdam.htm
        // Visual for Formula https://gamerant.com/pokemon-damage-calculation-help-guide/
        if (attack.GetAttackCategory() == AttackCategory.Physical)
        {
            Debug.Log("Fired a Physical Attack");
            int step1 = (2 * attacker.GetLevel() / 5 + 2);
            //Debug.Log($"Step 1 = {step1}");
            int step2 = step1 * attacker.GetAttackStat() * attack.GetAttackPower();
            //Debug.Log($"Step 2 = {step2}");
            int step3 = step2 / target.GetDefenseStat();
            //Debug.Log($"Step 3 = {step3}");
            int step4 = step3 / 50;
            //Debug.Log($"Step 4 = {step4}");
            int step5 = step4 + 2;
            //Debug.Log($"Step 5 = {step5}");
            int step6 = step5 * stab;
            //Debug.Log($"Step 6 = {step6}");
            float step7 = step6 * typeMatchup;
            //Debug.Log($"Step 7 = {step7}");
            float step8 = step7 * damageRange;
            //Debug.Log($"Step 8 = {step8}");
            int step9 = Mathf.FloorToInt(step8 / 255);
            //Debug.Log($"Step 9 = {step9}");
            damage = step9;
            // Move Power needs to be halved
            if (attacker.Status == StatusConditions.Burn)
            {
                damage /= 2;
            }
        }
        else if (attack.GetAttackCategory() == AttackCategory.Special)
        {
            Debug.Log("Fired a Special Attack");
            //Debug.Log($"Special Attack Stat = {pokemon1.GetSpecialAttackStat()}");
            int step1 = (2 * attacker.GetLevel() / 5 + 2);
            //Debug.Log($"Step 1 = {step1}");
            int step2 = step1 * attacker.GetSpecialAttackStat() * attack.GetAttackPower();
            //Debug.Log($"Step 2 = {step2}");
            int step3 = step2 / target.GetSpecialDefenseStat();
            //Debug.Log($"Step 3 = {step3}");
            int step4 = step3 / 50;
            //Debug.Log($"Step 4 = {step4}");
            int step5 = step4 + 2;
            //Debug.Log($"Step 5 = {step5}");
            int step6 = step5 * stab;
            //Debug.Log($"Step 6 = {step6}");
            float step7 = step6 * typeMatchup;
            //Debug.Log($"Step 7 = {step7}"); 
            float step8 = step7 * damageRange;
            //Debug.Log($"Step 8 = {step8}");
            int step9 = Mathf.FloorToInt(step8 / 255);
            //Debug.Log($"Step 9 = {step9}");
            damage = step9;
        }
        else
        {
            return 0;
        }
        return damage;
    }
    public virtual void DealDamage(int damage, Pokemon attacker, Pokemon target)
    {
        if (target.GetHPStat() - damage <= 0)
        {
            target.SetHPStat(0);
            Debug.Log("Opponent fainted");
            Destroy(target.gameObject);
            return;
        }
        target.SetHPStat(target.GetHPStat() - damage);
    }
}