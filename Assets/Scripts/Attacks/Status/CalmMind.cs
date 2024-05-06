using Cysharp.Threading.Tasks;
using UnityEngine;

public class CalmMind : Attack
{
    public CalmMind()
    {
        this.attackName = "Calm Mind";
        this.description = "The user quietly focuses its mind and calms its spirit to boost its Sp. Atk and Sp. Def stats.";
        this.type = StaticTypeObjects.Psychic;
        this.moveCategory = AttackCategory.Status;
        this.power = 0;
        this.accuracy = 101;
        this.priority = 0;
        this.currPowerPoints = 32;
        this.maxPowerPoints = 32;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        Debug.Log("Activating Calm Mind's effect");
        base.TriggerEffect(attacker, target);
        if (attacker.SpecialAttackStage < 6)
        {
            attacker.SpecialAttackStage += 1;
            int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()}'s Special Attack increased");
            GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()}'s Special Attack increased");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }
        }
        else
        {
            attacker.SpecialAttackStage = 6;
        }

        if (attacker.SpecialDefenseStage < 6)
        {
            attacker.SpecialDefenseStage += 1;
            int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()}'s Special Defense increased");
            GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()}'s Special Defense increased");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }
        }
        else
        {
            attacker.SpecialDefenseStage = 6;
        }
    }
}