using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;

public class Pokemon1Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        var player = trainerController.gameObject;
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[0]);
        Debug.Log("Attaching the Button");
        if (battlePartyUIElements.Pokemon1Button != null)
        {
            Debug.Log("Found The Button");
        }
        if (NetworkManager.Singleton.IsHost)
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon1Button, PartyPokemonClicked, 1);
        }
        else
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon1Button, PartyPokemonClicked, 2);
        }
    }
}
