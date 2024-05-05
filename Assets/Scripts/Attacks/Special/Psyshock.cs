using Cysharp.Threading.Tasks;
using UnityEngine;

public class Psyshock : Attack
{
    public Psyshock()
    {
        this.attackName = "Psyshock";
        this.description = "The user materializes an odd psychic wave to attack the target. This move deals physical damage.";
        this.type = StaticTypeObjects.Psychic;
        this.moveCategory = AttackCategory.Special;
        this.power = 80;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 16;
        this.maxPowerPoints = 16;
    }

    protected override async UniTask<int> CalculateDamage(Attack attack, Pokemon attacker, Pokemon target)
    {
        float stab = IsStab(attacker); // Same Type Attack Bonus
        float typeMatchup = await Effectiveness(attacker, target);
        int damageRange = Random.Range(217, 255);
        int critChance = Random.Range(1, 16);
        int damage;

        Debug.Log($"Fired a Special Attack = {attack.GetAttackName()}");
        //Debug.Log($"Special Attack Stat = {pokemon1.GetSpecialAttackStat()}");
        int step1 = (2 * attacker.GetLevel() / 5 + 2);
        //Debug.Log($"Step 1 = {step1}");
        int step2 = step1 * attacker.GetSpecialAttackStat() * attack.GetAttackPower();
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
        return damage;
    }
}