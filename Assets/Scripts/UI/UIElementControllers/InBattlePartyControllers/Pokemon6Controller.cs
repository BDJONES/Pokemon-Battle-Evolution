using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon6Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(GameManager.Instance.trainer1.pokemonTeam[5]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon6Button, PartyPokemonClicked);
    }
}