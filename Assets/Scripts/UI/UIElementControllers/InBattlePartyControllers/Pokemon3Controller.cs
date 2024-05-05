using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon3Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        var player = trainerController.gameObject;
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[2]);
        Debug.Log("Attaching the Button");
        if (IsHost)
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon3Button, PartyPokemonClicked, 1);
        }
        else
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon3Button, PartyPokemonClicked, 2);
        }
    }
}