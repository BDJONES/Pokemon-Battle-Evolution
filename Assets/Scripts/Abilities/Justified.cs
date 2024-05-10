using Cysharp.Threading.Tasks;
using System;
using Unity.Netcode;

public class Justified : Ability
{
    protected override void Awake()
    {
        //trainerController = this.abilityUser.transform.parent.gameObject.GetComponent<TrainerController>();
        base.Awake();
        GameManager.LastAttack += DarkAttackHandler;
        this.abilityName = "Justified";
        this.description = "When the Pokémon is hit by a Dark-type attack, its Attack stat is boosted by its sense of justice.";
    }

    private void DarkAttackHandler(Attack attack)
    {
        if (this.abilityUser.ActiveState && attack.GetAttackType() == StaticTypeObjects.Dark && (attack.GetAttackCategory() == AttackCategory.Physical || attack.GetAttackCategory() == AttackCategory.Special))
        {
            TriggerEffect(trainerController.GetOpponent().GetActivePokemon(), this.abilityUser);
        }
    }

    private void OnDestroy()
    {
        GameManager.LastAttack -= DarkAttackHandler;
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        // If the attack used was ground type, the attack does no damage
        // May need to add this logic in the damage calculation

        if (target.AttackStage < 6)
        {
            GameManager.Instance.SendDialogueToClientRpc($"Being hit by a Dark-Attack caused {target.GetNickname()}'s attack to raise");
            GameManager.Instance.SendDialogueToHostRpc($"Being hit by a Dark-Attack caused {target.GetNickname()}'s attack to raise");
            if (NetworkManager.Singleton.IsHost)
            {
                target.AttackStage += 1;
            }
            else
            {
                target.RequestStatChangeRpc(Stats.Attack, target.AttackStage + 1);
            }
        }
    }
}