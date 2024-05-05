using Cysharp.Threading.Tasks;
using UnityEngine;

public class DracoMeteor : Attack
{
    public DracoMeteor()
    {
        this.attackName = "Draco Meteor";
        this.description = "Comets are summoned down from the sky onto the target. The recoil from this move harshly lowers the user’s Sp. Atk stat.";
        this.type = StaticTypeObjects.Dragon;
        this.moveCategory = AttackCategory.Special;
        this.power = 90;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 8;
        this.maxPowerPoints = 8;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        if (attacker.SpecialAttackStage > -6)
        {
            if (attacker.SpecialAttackStage > -5)
            {
                attacker.SpecialAttackStage -= 2;
                int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                GameManager.Instance.SendDialogueToClientRpc($"{target.GetNickname()}'s special attack fell harshly.");
                GameManager.Instance.SendDialogueToHostRpc($"{target.GetNickname()}'s special attack fell harshly.");
                while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                attacker.SpecialAttackStage -= 1;
                int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                GameManager.Instance.SendDialogueToClientRpc($"{target.GetNickname()}'s special attack fell.");
                GameManager.Instance.SendDialogueToHostRpc($"{target.GetNickname()}'s special attack fell.");
                while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                {
                    await UniTask.Yield();
                }
            }
        }
    }
}