using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : IPlayerAction
{
    private Trainer trainer;
    private Pokemon pokemon;

    public Switch(Trainer trainer, Pokemon pokemon)
    {
        this.trainer = trainer;
        this.pokemon = pokemon;
    }
    public void SwitchPokemon(Trainer trainer, Pokemon newPokemon)
    {
        //if (newPokemon == trainer.GetActivePokemon())
        //{
        //    Debug.Log("You can't swap with the Pokemon that is already out.");
        //    return;
        //}
        for (int i = 1; i < trainer.GetPokemonTeam().Length; i++)
        {
            //Debug.Log($"Name = {trainer.trainerName}, Number of Pokemon = {trainer.GetPokemonTeam().Length}");
            if (trainer.GetPokemonTeam()[i] == null)
            {
                break;
            }
            if (trainer.GetPokemonTeam()[i].GetSpeciesName() == newPokemon.GetSpeciesName() && newPokemon.GetHPStat() != 0)
            {
                trainer.Switch(i);
                EventsToTriggerManager.AlertEventTriggered(EventsToTrigger.YourPokemonSwitched);
                return;
            }

        }
        Debug.Log("Failed to find something to switch with");
    }

    public Trainer GetTrainer()
    {
        return trainer;
    }

    public Pokemon GetPokemon()
    {
        return pokemon;
    }

    public IPlayerAction PerformAction()
    {
        
        return this;
    }

    public IPlayerAction PerformAction(Pokemon pokemon1, Pokemon pokemon2)
    {
        throw new System.NotImplementedException();
    }

    public IPlayerAction PerformAction(Trainer trainer, Pokemon newPokemon)
    {
        SwitchPokemon(trainer, newPokemon);
        return this;
    }
}