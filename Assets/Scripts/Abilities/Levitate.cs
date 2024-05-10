using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class Levitate : Ability
{

    protected override void Awake()
    {
        //trainerController = this.abilityUser.transform.parent.gameObject.GetComponent<TrainerController>();
        base.Awake();
        eventsToTriggerManager.OnTriggerEvent += EventToTriggerHandler;
        this.abilityName = "Levitate";
        this.description = "By floating in the air, the Pokémon receives full immunity to all Ground-type moves.";
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        // If the attack used was ground type, the attack does no damage
        // May need to add this logic in the damage calculation
        if (NetworkManager.Singleton.IsHost)
        {
            target.IsLevitating = true;
        }
        else
        {
            target.RequestFieldChangeRpc(PokemonFields.isLevitating, true);
        }
    }

    private void EventToTriggerHandler(EventsToTrigger e)
    {
        if (e == EventsToTrigger.YourPokemonSwitched)
        {
            if (this.abilityUser == null) return;
            TriggerEffect(trainerController.GetOpponent().GetActivePokemon(), this.abilityUser);
        }
        return;
    }
}