using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Attack
{
    public Flamethrower()
    {
        this.attackName = "Flamethrower";
        this.description = "The target is scorched with an intense blast of fire. This may also leave the target with a burn.";
        this.type = StaticTypeObjects.Fire;
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
                target.Status = StatusConditions.Burn;
                int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                GameManager.Instance.SendDialogueToClientRpc($"{target.GetNickname()} was burned.");
                GameManager.Instance.SendDialogueToHostRpc($"{target.GetNickname()} was burned.");
                while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                {
                    await UniTask.Yield();
                }
                //Debug.Log("Flamethrower burned the target");
            }
        }
    }
}
