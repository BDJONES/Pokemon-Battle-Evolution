using Cysharp.Threading.Tasks;

public class Moonlight : Attack
{
    public Moonlight()
    {
        this.attackName = "Moonlight";
        this.description = "A frenetic dance to uplift the fighting spirit. This sharply boosts the user’s Attack stat.";
        this.type = StaticTypeObjects.Fairy;
        this.moveCategory = AttackCategory.Status;
        this.power = 0;
        this.accuracy = 101;
        this.priority = 0;
        this.currPowerPoints = 8;
        this.maxPowerPoints = 8;
    }

    protected async override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        attacker.SetHPStat(attacker.GetHPStat() + (attacker.GetMaxHPStat() / 2));
        int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
        GameManager.Instance.SendDialogueToClientRpc($"{attacker.GetNickname()} had it's HP restored");
        GameManager.Instance.SendDialogueToHostRpc($"{attacker.GetNickname()} had it's HP restored");
        while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
        {
            await UniTask.Yield();
        }
    }
}