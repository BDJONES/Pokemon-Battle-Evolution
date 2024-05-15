using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon6Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[5]);
        if (battlePartyUIElements.Pokemon6Button != null)
        {
            Debug.Log("Found The Button");
        }
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Hello There Host");
            
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon6Button, PartyPokemonClicked, 1);
        }
        else
        {
            Debug.Log("Hello There Client");
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon6Button, PartyPokemonClicked, 2);
        }
    }
}