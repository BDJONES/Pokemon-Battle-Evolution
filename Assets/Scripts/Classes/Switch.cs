using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : IPlayerAction
{
    public Trainer trainer;
    public Pokemon pokemon;

    public Switch(Trainer trainer, Pokemon pokemon)
    {
        this.trainer = trainer;
        this.pokemon = pokemon;
    }
    public void SwitchPokemon(Trainer trainer, Pokemon newPokemon)
    {
        //if (newPokemon == trainer.activePokemon)
        //{
        //    Debug.Log("You can't swap with the Pokemon that is already out.");
        //    return;
        //}
        trainer.Switch(1);
        return;
        //for (int i = 0; i < trainer.team.Length; i++)
        //{
        //    if (trainer.team[i].GetComponent<Pokemon>().GetNickname() == newPokemon.GetNickname())
        //    {
        //        trainer.Switch(i);
        //    }
        //}
        

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