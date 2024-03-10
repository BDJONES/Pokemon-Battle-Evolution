using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JoinButtonController : MonoBehaviour
{
    private TitleScreenUIElements titleScreenUIElements;
    private LobbyManager lobbyManager;

    private void OnEnable()
    {
        //uIController = .GetComponentInChildren<UIController>();
        //uIController.OnMenuChange += HandleMenuChange;
        titleScreenUIElements = gameObject.GetComponent<TitleScreenUIElements>();
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();            
        titleScreenUIElements.JoinButton.clicked += HandleClick;
        UIEventSubscriptionManager.Subscribe(titleScreenUIElements.JoinButton, HandleClick);
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.TitleScreen)
        {

        }
    }

    private void OnDisable()
    {
    }

    private void HandleClick()
    {
        lobbyManager.CreateOrJoinLobby();

        // Wait until a CLIENT joins
    }
}
