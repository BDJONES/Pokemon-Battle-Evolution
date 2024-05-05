using Cysharp.Threading.Tasks;
using UnityEngine;

public class FoulPlay : Attack
{
    public FoulPlay()
    {
        this.attackName = "Foul Play";
        this.description = "The user turns the target’s strength against it. The higher the target’s Attack stat, the greater the damage this move inflicts.";
        this.type = StaticTypeObjects.Dark;
        this.moveCategory = AttackCategory.Physical;
        this.power = 95;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 24;
        this.maxPowerPoints = 24;
        this.isContact = true;
    }

    protected override async UniTask<int> CalculateDamage(Attack attack, Pokemon attacker, Pokemon target)
    {
        float stab = IsStab(attacker); // Same Type Attack Bonus
        float typeMatchup = await Effectiveness(attacker, target);
        int damageRange = Random.Range(217, 255);
        int critChance = Random.Range(1, 16);
        int damage;

        // Damage Formula comes from this attack https://www.math.miami.edu/~jam/azure/compendium/battdam.htm
        // Visual for Formula https://gamerant.com/pokemon-damage-calculation-help-guide/
        
        int step1 = (2 * attacker.GetLevel() / 5 + 2);
        //Debug.Log($"Step 1 = {step1}");
        int step2 = step1 * target.GetAttackStat() * attack.GetAttackPower();
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
            int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()} landed a critical hit");
            GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()} landed a critical hit");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }
        }

        return damage;
    }
}