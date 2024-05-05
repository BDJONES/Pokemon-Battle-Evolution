using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderWave : Attack
{
    public ThunderWave()
    {
        this.attackName = "Thunder Wave";
        this.description = "The user launches a weak jolt of electricity that paralyzes the target.";
        this.type = StaticTypeObjects.Electric;
        this.moveCategory = AttackCategory.Status;
        this.power = 0;
        this.accuracy = 90;
        this.priority = 0;
        this.currPowerPoints = 32;
        this.maxPowerPoints = 32;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        int activeRPCs;
        base.TriggerEffect(attacker, target);
        if (target.Status == StatusConditions.Healthy && (target.GetType1() != StaticTypeObjects.Electric || (target.GetType2() != null && target.GetType2() != StaticTypeObjects.Electric)))
        {
            
            target.Status = StatusConditions.Paralysis;
            activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            GameManager.Instance.SendDialogueToClientRpc($"{target.GetNickname()} was paralyzed");
            GameManager.Instance.SendDialogueToHostRpc($"{target.GetNickname()} was paralyzed");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }
        }
    }
}
