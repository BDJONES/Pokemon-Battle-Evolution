using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon3Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[2]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon3Button, PartyPokemonClicked);
    }
}