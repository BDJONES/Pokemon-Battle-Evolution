using Cysharp.Threading.Tasks;
using System;
using System.Reflection;
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
    protected bool isContact;
    public static event Action<Attack> LastAttack;
    protected bool finishedAttack = false;
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
    public void SetCurrentPP(int newPPAmount)
    {
        currPowerPoints = newPPAmount;
    }
    public int GetMaxPP()
    {
        return maxPowerPoints;
    }
    public bool IsContact()
    {
        return isContact;
    }
    protected async virtual UniTask<bool> UseAttack(Pokemon attacker, Pokemon target)
    {
        if (this.accuracy == 101)
        {
            // Ignore evasion
            int damage = await CalculateDamage(this, attacker, target);
            DealDamage(damage, attacker, target);
            if (target.GetHPStat() == 0)
            {
                return true;
            }
            // Some effects trigger after KO
            TriggerEffect(attacker, target);
        }
        else if (this.accuracy == 100)
        {
            // Have to factor evasion into equation
            int damage = await CalculateDamage(this, attacker, target);
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
            // Have to factor evasion into equation
            int generatedValue = UnityEngine.Random.Range(0, 99);
            int accurateRange = 100 - this.accuracy;
            if (generatedValue < accurateRange)
            {
                Debug.Log("The Attack Missed");
                return false;
            }
            else
            {
                int damage = await CalculateDamage(this, attacker, target);
                DealDamage(damage, attacker, target);
                if (target.GetHPStat() == 0)
                {
                    return true;
                }
                TriggerEffect(attacker, target);
            }
        }
        //Debug.Log("The Attack Landed");
        LastAttack.Invoke(this);
        //finishedAttack = true;
        GameManager.Instance.FinishRPCTaskRpc();
        return true;
    }
    protected virtual void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        currPowerPoints -= 1;
        return;
    }
    protected async virtual UniTask<int> CalculateDamage(Attack attack, Pokemon attacker, Pokemon target)
    {
        if (attacker.GetAbility().GetType().Name == StaticAbilityObjects.Levitate.GetType().Name && attack.GetAttackType() == StaticTypeObjects.Ground)
        {
            return 0;
        }
        float stab = IsStab(attacker); // Same Type Attack Bonus
        float typeMatchup = 1;
        if (attack.GetAttackCategory() != AttackCategory.Status)
        {
            typeMatchup = await Effectiveness(attacker, target);
        }
        int damageRange = UnityEngine.Random.Range(217, 255);
        int critChance = UnityEngine.Random.Range(1, 16);
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
                int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()} landed a critical hit");
                GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()} landed a critical hit");
                while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                {
                    await UniTask.Yield();
                }
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
        GameManager.Instance.AddRPCTaskRpc();
        UseAttack(attacker, target);
        
        //while (!finishedAttack) ;
        //finishedAttack = false;
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

    protected virtual async UniTask<float> Effectiveness(Pokemon attacker, Pokemon target)
    {
        float effectiveness = 1f;
        if (target.GetType1().immunities.Contains(this.type) || (target.GetType2() != null && target.GetType2().immunities.Contains(this.type)))
        {
            return 0;
        }

        if (target.GetType1().weaknesses.Contains(this.type))
        {
            effectiveness *= 2;
        }

        if (target.GetType2() != null && target.GetType2().weaknesses.Contains(this.type))
        {
            Debug.Log($"{target.GetType2().GetType().Name} is weak to {this.type.GetType().Name}");
            effectiveness *= 2;
        }

        if (target.GetType1().resistances.Contains(this.type))
        {
            effectiveness /= 2;
        }

        if (target.GetType2() != null && target.GetType2().resistances.Contains(this.type))
        {
            effectiveness /= 2;
        }

        if (effectiveness > 1f)
        {
            int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            GameManager.Instance.SendDialogueToClientRpc($"{this.GetAttackName()} is super effective.");
            GameManager.Instance.SendDialogueToHostRpc($"{this.GetAttackName()} is super effective.");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }
        }
        else if (effectiveness < 1f)
        {
            int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            GameManager.Instance.SendDialogueToClientRpc($"{this.GetAttackName()} is not very effective.");
            GameManager.Instance.SendDialogueToHostRpc($"{this.GetAttackName()} is not very effective.");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }
        }
        return effectiveness;
    }
}