using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon2Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[1]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon2Button, PartyPokemonClicked);
    }
}