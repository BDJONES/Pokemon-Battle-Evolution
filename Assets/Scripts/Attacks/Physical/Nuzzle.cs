using Cysharp.Threading.Tasks;
using UnityEngine;

public class Nuzzle : Attack
{
    public Nuzzle()
    {
        this.attackName = "Nuzzle";
        this.description = "The user attacks by nuzzling its electrified cheeks against the target. This also leaves the target with paralysis.";
        this.type = StaticTypeObjects.Electric;
        this.moveCategory = AttackCategory.Physical;
        this.power = 20;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 32;
        this.maxPowerPoints = 32;
        this.isContact = true;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        if (target.Status == StatusConditions.Healthy)
        {
            if (target.GetHPStat() > 0)
            {
                target.Status = StatusConditions.Paralysis;
                int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()} was paralyzed");
                GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()} was paralyzed");
                while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                {
                    await UniTask.Yield();
                }
            }
        }
    }
}