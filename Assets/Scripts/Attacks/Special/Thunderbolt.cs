using Cysharp.Threading.Tasks;
using UnityEngine;

public class Thunderbolt : Attack
{
    public Thunderbolt()
    {
        this.attackName = "Thunderbolt";
        this.description = "The user attacks the target with a strong electric blast. This may also leave the target with paralysis.";
        this.type = StaticTypeObjects.Electric;
        this.moveCategory = AttackCategory.Special;
        this.power = 90;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 16;
        this.maxPowerPoints = 16;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        int genereatedValue = Random.Range(0, 99);
        if (genereatedValue < 10 && target.Status == StatusConditions.Healthy)
        {
            if (target.GetHPStat() > 0)
            {
                target.Status = StatusConditions.Paralysis;
                int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                GameManager.Instance.SendDialogueToClientRpc($"{target.GetNickname()} was paralyzed.");
                GameManager.Instance.SendDialogueToHostRpc($"{target.GetNickname()} was paralyzed.");
                while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                {
                    await UniTask.Yield();
                }
            }
        }
    }
}