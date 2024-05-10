using Unity.Netcode;
using UnityEngine;

public class Pressure : Ability
{
    protected override void Awake()
    {
        //trainerController = this.abilityUser.transform.parent.gameObject.GetComponent<TrainerController>();
        base.Awake();
        eventsToTriggerManager.OnTriggerEvent += EventToTriggerHandler;
        this.abilityName = "Pressure";
        this.description = "When the Pokémon enters a battle, it intimidates opposing Pokémon and makes them cower, lowering their Attack stats.";
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {


        if (NetworkManager.Singleton.IsHost)
        {
            attacker.IsPressured = true;
            if (target.IsOwner)
            {
                GameManager.Instance.SendDialogueToClientRpc($"Your opponent's {target.GetNickname()} is exerting Pressure");
                GameManager.Instance.SendDialogueToHostRpc($"Your {target.GetNickname()} is exerting Pressure");
            }
        }
        else
        {
            attacker.RequestFieldChangeRpc(PokemonFields.isPressured, true);
            if (target.IsOwner)
            {
                GameManager.Instance.SendDialogueToHostRpc($"Your opponent's {target.GetNickname()} is exerting Pressure");
                GameManager.Instance.SendDialogueToClientRpc($"Your {target.GetNickname()} is exerting Pressure");
            }
        }
    }

    private void EventToTriggerHandler(EventsToTrigger e)
    {
        if (e == EventsToTrigger.OpposingPokemonSwitched || e == EventsToTrigger.YourPokemonSwitched)
        {
            if (this.abilityUser == null) return;
            if (this.abilityUser.ActiveState == true)
            {
                TriggerEffect(trainerController.GetOpponent().GetActivePokemon(), this.abilityUser);
            }
        }
    }
}