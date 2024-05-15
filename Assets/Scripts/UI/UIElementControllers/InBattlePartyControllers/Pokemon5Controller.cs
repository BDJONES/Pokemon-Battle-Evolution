using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
public class Pokemon5Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[4]);
        if (battlePartyUIElements.Pokemon5Button != null)
        {
            Debug.Log("Found The Button");
        }
        if (NetworkManager.Singleton.IsHost)
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon5Button, PartyPokemonClicked, 1);
        }
        else
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon5Button, PartyPokemonClicked, 2);
        }
    }
}