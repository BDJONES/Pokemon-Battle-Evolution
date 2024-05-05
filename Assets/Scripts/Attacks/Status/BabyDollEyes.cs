using Cysharp.Threading.Tasks;
using UnityEngine;

public class BabyDollEyes : Attack
{
    public BabyDollEyes()
    {
        this.attackName = "Baby-Doll Eyes";
        this.description = "The user launches a weak jolt of electricity that paralyzes the target.";
        this.type = StaticTypeObjects.Fairy;
        this.moveCategory = AttackCategory.Status;
        this.power = 0;
        this.accuracy = 100;
        this.priority = 1;
        this.currPowerPoints = 48;
        this.maxPowerPoints = 48;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        target.AttackStage -= 1;
        int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
        GameManager.Instance.SendDialogueToClientRpc($"{target.GetNickname()}'s attack fell.");
        GameManager.Instance.SendDialogueToHostRpc($"{target.GetNickname()}'s attack fell.");
        while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
        {
            await UniTask.Yield();
        }
    }
}