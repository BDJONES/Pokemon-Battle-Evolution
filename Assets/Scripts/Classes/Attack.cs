using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Attack : IPlayerAction
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

    public Attack() 
    {
        this.attackName = "Generic Attack";
    }

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
    protected virtual bool UseAttack(Pokemon attacker, Pokemon target)
    {
        if (this.accuracy == 100)
        {
            int damage = CalculateDamage(this, attacker, target);
            DealDamage(damage, attacker, target);
            if (target.GetHPStat() == 0)
            {
                return true;
            }
            // Some effects trigger after KO
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
        //Debug.Log("The Attack Landed");
        return true;
    }
    protected virtual void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        currPowerPoints -= 1;
        return;
    }
    protected virtual int CalculateDamage(Attack attack, Pokemon attacker, Pokemon target)
    {
        float stab = IsStab(attacker); // Same Type Attack Bonus
        float typeMatchup = Effectiveness(attacker, target);
        int damageRange = Random.Range(217, 255);
        int critChance = Random.Range(1, 16);
        int damage;
        // Damage Formula comes from this attack https://www.math.miami.edu/~jam/azure/compendium/battdam.htm
        // Visual for Formula https://gamerant.com/pokemon-damage-calculation-help-guide/
        if (attack.GetAttackCategory() == AttackCategory.Physical)
        {
            //Debug.Log($"Fired a Physical Attack = {attack.attackName}");
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
            float step6 = step5 * stab;
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
            if (critChance == 1)
            {
                damage = Mathf.FloorToInt(damage * 1.5f);
            }
        }
        else if (attack.GetAttackCategory() == AttackCategory.Special)
        {
            Debug.Log($"Fired a Special Attack = {attack.attackName}");
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
            float step6 = step5 * stab;
            //Debug.Log($"Step 6 = {step6}");
            float step7 = step6 * typeMatchup;
            //Debug.Log($"Step 7 = {step7}"); 
            float step8 = step7 * damageRange;
            //Debug.Log($"Step 8 = {step8}");
            int step9 = Mathf.FloorToInt(step8 / 255);
            //Debug.Log($"Step 9 = {step9}");
            damage = step9;
            if (critChance == 1)
            {
                Debug.Log("You landed a critical hit");
                damage = Mathf.FloorToInt(damage * 1.5f);
            }
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
            //Object.Destroy(target.gameObject);
            return;
        }
        target.SetHPStat(target.GetHPStat() - damage);
    }
    public IPlayerAction PerformAction()
    {
        return this;

    }

    public IPlayerAction PerformAction(Pokemon attacker, Pokemon target)
    {
        UseAttack(attacker, target);
        return this;
    }

    public IPlayerAction PerformAction(Trainer trainer, Pokemon pokemon2)
    {
        throw new System.NotImplementedException();
    }

    protected virtual float IsStab(Pokemon pokemon)
    {
        if (this.type == pokemon.GetType1() || (pokemon.GetType2() != null && this.type == pokemon.GetType2())) {
            //Debug.Log($"{this.GetAttackName()} is STAB");
            return 1.5f;
        }
        else
        {
            return 1;
        }
    }

    protected virtual float Effectiveness(Pokemon attacker, Pokemon target)
    {
        float effectiveness = 1f;

        //for (int i = 0; i < target.GetType1().weaknesses.Count; i++)
        //{
        //    Debug.Log(target.GetType1().weaknesses[i].GetType().Name);
        //    if (this.type)
        //    {
        //        Debug.Log("He Weak to this right here");
        //    }
        //    else
        //    {
        //        Debug.Log("That's aight");
        //    }
        //}
        //Debug.Log("Checking for move effectiveness");
        if (target.GetType1().immunities.Contains(this.type) || (target.GetType2() != null && target.GetType2().immunities.Contains(this.type)))
        {
            //Debug.Log($"{target.GetSpeciesName()} is unaffected.");
            return 0;
        }

        if (target.GetType1().weaknesses.Contains(this.type))
        {
            //Debug.Log($"{target.GetType1().GetType().Name} is weak to {this.type.GetType().Name}");
            effectiveness *= 2;
        }

        if (target.GetType2() != null && target.GetType2().weaknesses.Contains(this.type))
        {
            Debug.Log($"{target.GetType2().GetType().Name} is weak to {this.type.GetType().Name}");
            effectiveness *= 2;
        }

        if (target.GetType1().resistances.Contains(this.type))
        {
            //Debug.Log($"{target.GetType1().GetType().Name} resists {this.type.GetType().Name}");
            effectiveness /= 2;
        }

        if (target.GetType2() != null && target.GetType2().resistances.Contains(this.type))
        {
            //Debug.Log($"{target.GetType2().GetType().Name} resists {this.type.GetType().Name}");
            effectiveness /= 2;
        }

        if (effectiveness > 1f)
        {
            Debug.Log($"{this.GetAttackName()} is super effective. Effectiveness = {effectiveness}.");
        }
        else if (effectiveness == 1f)
        {
            Debug.Log($"{this.GetAttackName()} is effective");
        }
        else
        {
            Debug.Log($"{this.GetAttackName()} is not very effective. Effectiveness = {effectiveness}.");
        }

        return effectiveness;
    }
}