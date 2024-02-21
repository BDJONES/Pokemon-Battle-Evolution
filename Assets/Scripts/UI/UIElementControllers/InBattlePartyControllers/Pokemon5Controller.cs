using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon5Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[4]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon5Button, PartyPokemonClicked);
    }
}