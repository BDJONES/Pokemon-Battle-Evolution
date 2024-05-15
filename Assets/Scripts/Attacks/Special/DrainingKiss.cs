using Cysharp.Threading.Tasks;
using UnityEngine;

public class DrainingKiss : Attack
{
    public DrainingKiss()
    {
        this.attackName = "Draining Kiss";
        this.description = "The user steals the target’s HP with a kiss. The user’s HP is restored by over half the damage taken by the target.";
        this.type = StaticTypeObjects.Fairy;
        this.moveCategory = AttackCategory.Special;
        this.power = 50;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 16;
        this.maxPowerPoints = 16;
    }

    protected override async UniTask<bool> UseAttack(Pokemon attacker, Pokemon target)
    {
        bool canPokemonMove = await CanPokemonMove(attacker);
        if (!canPokemonMove)
        {
            return false;
        }
        attacker.SendLastAttackFromThisPokemonRpc(this.GetType().Name);
        //GameManager.Instance.AddRPCTaskRpc();
        int damage = await CalculateDamage(this, attacker, target);
        DealDamage(damage, attacker, target);
        if (target.GetHPStat() == 0)
        {
            return true;
        }
        // Will want to look into another way to trigger a hpRegained Effect
        TriggerEffect(attacker, target);
        int hpRegained = Mathf.FloorToInt(damage * .75f);
        if (hpRegained + attacker.GetHPStat() > attacker.GetMaxHPStat())
        {
            attacker.SetHPStat(attacker.GetMaxHPStat());
        }
        else if (hpRegained > 0)
        {
            attacker.SetHPStat(hpRegained + attacker.GetHPStat());
        }
        GameManager.Instance.FinishRPCTaskRpc();
        return true;
    }
}