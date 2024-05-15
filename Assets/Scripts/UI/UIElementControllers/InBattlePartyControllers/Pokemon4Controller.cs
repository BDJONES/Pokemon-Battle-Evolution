using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
public class Pokemon4Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[3]);
        if (battlePartyUIElements.Pokemon4Button != null)
        {
            Debug.Log("Found The Button");
        }
        if (NetworkManager.Singleton.IsHost)
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon4Button, PartyPokemonClicked, 1);
        }
        else
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon4Button, PartyPokemonClicked, 2);
        }
    }
}