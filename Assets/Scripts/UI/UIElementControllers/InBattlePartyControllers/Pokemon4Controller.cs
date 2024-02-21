using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon4Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[3]);
        UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon4Button, PartyPokemonClicked);
    }
}