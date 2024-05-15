using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
public class Pokemon2Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[1]);
        if (battlePartyUIElements.Pokemon2Button != null)
        {
            Debug.Log("Found The Button");
        }
        if (NetworkManager.Singleton.IsHost)
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon2Button, PartyPokemonClicked, 1);
        }
        else
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon2Button, PartyPokemonClicked, 2);
        }
    }
}