using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pokemon1Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[0]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon1Button, PartyPokemonClicked);
    }
}
