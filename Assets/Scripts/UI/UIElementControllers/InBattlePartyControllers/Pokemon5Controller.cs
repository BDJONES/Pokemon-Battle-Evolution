using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon5Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(GameManager.Instance.trainer1.pokemonTeam[4]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon5Button, PartyPokemonClicked);
    }
}