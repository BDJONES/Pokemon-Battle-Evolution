using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class Static : Ability
{
    protected override void Awake()
    {
        //trainerController = this.abilityUser.transform.parent.gameObject.GetComponent<TrainerController>();
        base.Awake();
        eventsToTriggerManager.OnTriggerEvent += EventToTriggerHandler;
        this.abilityName = "Static";
        this.description = "The Pokémon is charged with static electricity and may paralyze attackers that make direct contact with it.";
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
        eventsToTriggerManager.OnTriggerEvent -= EventToTriggerHandler;
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {

        if (attacker.GetLastAttack().IsContact())
        {
            int paralyzeChance = Random.Range(0, 99);
            Debug.Log($"Para-Chance is {paralyzeChance}");
            if (paralyzeChance < 30 && attacker.Status == StatusConditions.Healthy)
            {
                attacker.Status = StatusConditions.Paralysis;
                if (NetworkManager.Singleton.IsHost)
                {
                    GameManager.Instance.SendDialogueToHostRpc($"Your {target.GetNickname()}'s ability Static, paralyzed your opponent's {attacker.GetNickname()}");
                    GameManager.Instance.SendDialogueToClientRpc($"Your {attacker.GetNickname()} was paralyzed, by your opponent {target.GetNickname()}'s Static");
                }
                else
                {
                    GameManager.Instance.SendDialogueToClientRpc($"Your {target.GetNickname()}'s ability Static, paralyzed your opponent's {attacker.GetNickname()}");
                    GameManager.Instance.SendDialogueToHostRpc($"Your {attacker.GetNickname()} was paralyzed, by your opponent {target.GetNickname()}'s Static");
                }
            }
        }
    }

    private void EventToTriggerHandler(EventsToTrigger trigger)
    {
        if (trigger == EventsToTrigger.OpposingPokemonAttackedYourPokemon)
        {
            if (this.abilityUser.ActiveState == true)
            {
                TriggerEffect(trainerController.GetOpponent().GetActivePokemon(), this.abilityUser);
            }
        }
    }
}