using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class Synchronize : Ability
{
    protected override void Awake()
    {
        //trainerController = this.abilityUser.transform.parent.gameObject.GetComponent<TrainerController>();
        base.Awake();
        GameManager.OnStateChange += GameStateOnChangeHandler;
        this.abilityName = "Synchronize";
        this.description = "If the Pokémon is burned, paralyzed, or poisoned by another Pokémon, that Pokémon will be inflicted with the same status condition.";
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
        if (abilityUser != null)
        {
            abilityUser.StatusChanged -= HandleStatusConditionChange;
        }
        
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        // Create an event that will notify when the status has changed
        if (attacker.Status != StatusConditions.Healthy || target.Status == StatusConditions.Healthy)
        {
            Debug.Log("The target is healthy");
            return;
        }        
        if (NetworkManager.Singleton.IsHost)
        {
            attacker.Status = target.Status;
        }
        else
        {
            attacker.RequestStatusChangeRpc(target.Status);
        }            
        GameManager.Instance.SendDialogueToClientRpc($"Synchronize caused {attacker.GetNickname()} to feel the same effects as {target.GetNickname()}");
        GameManager.Instance.SendDialogueToHostRpc($"Synchronize caused {attacker.GetNickname()} to feel the same effects as {target.GetNickname()}");
    }

    protected override void GameStateOnChangeHandler(GameState state)
    {
        if (state == GameState.BattleStart)
        {
            if (abilityUser != null)
            {
                abilityUser.StatusChanged += HandleStatusConditionChange;
            }
        }
    }

    private async void HandleStatusConditionChange(StatusConditions statusCondition)
    {
        if (statusCondition == StatusConditions.Burn || statusCondition == StatusConditions.BadPoison || statusCondition == StatusConditions.Poison || statusCondition == StatusConditions.Paralysis)
        {
            Debug.Log($"Detected a StatusCondition Change, {statusCondition}");
            if (this.abilityUser.ActiveState == true)
            {
                Debug.Log("Triggering the Condition Change");
                while (abilityUser.Status != statusCondition)
                {
                    await UniTask.Yield();
                }
                TriggerEffect(trainerController.GetOpponent().GetActivePokemon(), this.abilityUser);
            }
        }
    }
}