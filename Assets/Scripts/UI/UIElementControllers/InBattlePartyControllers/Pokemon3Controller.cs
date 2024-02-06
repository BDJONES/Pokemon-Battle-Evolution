using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon3Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(GameManager.Instance.trainer1.pokemonTeam[2]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon3Button, PartyPokemonClicked);
    }
}