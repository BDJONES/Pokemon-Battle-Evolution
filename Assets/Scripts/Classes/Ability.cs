using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public Pokemon abilityUser;
    protected string abilityName = null!;
    protected string description = null!;
    protected TrainerController trainerController;
    protected EventsToTriggerManager eventsToTriggerManager;
    protected virtual void Awake()
    {
        if (abilityUser != null)
        {
            trainerController = this.abilityUser.gameObject.transform.parent.parent.gameObject.GetComponent<TrainerController>();
        }
        GameObject eventsToTriggerGO = GameObject.Find("EventsToTriggerManager");
        eventsToTriggerManager = eventsToTriggerGO.GetComponent<EventsToTriggerManager>();
    }

    public void InitializeAbility()
    {
        trainerController = this.abilityUser.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
    }
    protected virtual void GameStateOnChangeHandler(GameState state)
    {
        return;
    }

    protected abstract void TriggerEffect(Pokemon attacker, Pokemon target);
}
