using Cysharp.Threading.Tasks;
using UnityEngine;

public class Liquidation : Attack
{
    public Liquidation()
    {
        this.attackName = "Liquidation";
        this.description = "The user slams into the target using a full-force blast of water. This may also lower the target’s Defense stat.";
        this.type = StaticTypeObjects.Water;
        this.moveCategory = AttackCategory.Physical;
        this.power = 85;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 16;
        this.maxPowerPoints = 16;
        this.isContact = true;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        int genereatedValue = Random.Range(0, 99);
        if (genereatedValue < 20)
        {
            if (target.GetHPStat() > 0)
            {
                target.DefenseStage -= 1;
                int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                GameManager.Instance.SendDialogueToClientRpc($"{target.GetNickname()}'s defense fell");
                GameManager.Instance.SendDialogueToHostRpc($"{target.GetNickname()}'s defense fell");
                while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                {
                    await UniTask.Yield();
                }
            }
        }
    }
}