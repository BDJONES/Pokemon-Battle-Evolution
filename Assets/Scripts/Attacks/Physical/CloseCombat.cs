using Cysharp.Threading.Tasks;
using UnityEngine;

public class CloseCombat : Attack
{
    public CloseCombat()
    {
        this.attackName = "Close Combat";
        this.description = "The user fights the target up close, inflicting damage without guarding itself. This also lowers the user’s Defense and Sp. Def stats.";
        this.type = StaticTypeObjects.Fighting;
        this.moveCategory = AttackCategory.Physical;
        this.power = 120;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 8;
        this.maxPowerPoints = 8;
        this.isContact = true;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        attacker.DefenseStage -= 1;
        attacker.SpecialDefenseStage -= 1;
        int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
        GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()}'s defense fell");
        GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()}'s defense fell");
        GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()}'s special defense fell");
        GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()}'s special defense fell");
        while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
        {
            await UniTask.Yield();
        }
    }
}