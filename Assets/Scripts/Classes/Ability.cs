using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public Pokemon abilityUser;
    protected string abilityName = null!;
    protected string description = null!;
    protected TrainerController trainerController;
    protected virtual void Awake()
    {
        //trainerController = this.abilityUser.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
    }

    public void InitializeAbility()
    {
        trainerController = this.abilityUser.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<TrainerController>();
    }

    protected abstract void TriggerEffect(Pokemon attacker, Pokemon target);
    protected abstract void GameStateOnChangeHandler(GameState state);
}
