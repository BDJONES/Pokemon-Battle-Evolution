using Cysharp.Threading.Tasks;
using UnityEngine;

public class SwordsDance : Attack
{
    public SwordsDance()
    {
        this.attackName = "Swords Dance";
        this.description = "A frenetic dance to uplift the fighting spirit. This sharply boosts the user’s Attack stat.";
        this.type = StaticTypeObjects.Normal;
        this.moveCategory = AttackCategory.Status;
        this.power = 0;
        this.accuracy = 101;
        this.priority = 0;
        this.currPowerPoints = 32;
        this.maxPowerPoints = 32;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        int activeRPCs;
        base.TriggerEffect(attacker, target);
        if (attacker.AttackStage == 5)
        {
            attacker.AttackStage += 1;
        }
        else if (attacker.AttackStage < 5)
        {
            attacker.AttackStage += 2;
        }
        else
        {
            activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()}'s attack could not go any higher");
            GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()}'s attack could not go any higher");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }
            return;
        }
        // May not work
        activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
        GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()}'s attack sharply increased");
        GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()}'s attack sharply increased");
        while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
        {
            await UniTask.Yield();
        }
    }
}