using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Pokemon5Controller : PartyPokemonController
{
    protected override void AttachButton()
    {
        var player = trainerController.gameObject;
        InitializeSwitch(trainerController.GetPlayer().GetPokemonTeam()[4]);
        
        if (TrainerController.IsOwnerHost(player))
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon5Button, PartyPokemonClicked, 1);
        }
        else
        {
            UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon5Button, PartyPokemonClicked, 2);
        }        
    }
}