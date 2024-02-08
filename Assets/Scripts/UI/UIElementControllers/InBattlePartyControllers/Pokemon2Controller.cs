using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon2Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(GameManager.Instance.trainer1.pokemonTeam[1]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon2Button, PartyPokemonClicked);
    }
}