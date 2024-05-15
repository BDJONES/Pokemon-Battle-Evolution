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
        Debug.Log("Attaching the Button");
        if (IsHost)
        {
            if (uIController.GetCurrentTrainer1Menu() == Menus.InBattlePartyMenu)
            {
                UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon1Button, PartyPokemonClicked, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(battlePartyDialogueUIElements.Pokemon1Button, PartyPokemonClicked, 1);
            }
        }
        else
        {
            if (uIController.GetCurrentTrainer2Menu() == Menus.InBattlePartyMenu)
            {
                UIEventSubscriptionManager.Subscribe(battlePartyUIElements.Pokemon1Button, PartyPokemonClicked, 2);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(battlePartyDialogueUIElements.Pokemon1Button, PartyPokemonClicked, 2);
            }
        }        
    }
}