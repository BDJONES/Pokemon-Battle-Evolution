using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JoinButtonController : MonoBehaviour
{
    private TitleScreenUIElements titleScreenUIElements;
    //private UIController uIController;
    private LobbyManager lobbyManager;

    private void OnEnable()
    {
        //uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        //uIController.OnMenuChange += HandleMenuChange;
        titleScreenUIElements = gameObject.GetComponent<TitleScreenUIElements>();
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        titleScreenUIElements.JoinButton.clicked += HandleClick;
        //titleScreenUIElements.JoinButton += HandleClick;
        //if (IsHost)
        //{
        //    UIEventSubscriptionManager.Subscribe(, , 1);
        //}
        //else
        //{
        //    UIEventSubscriptionManager.Subscribe(titleScreenUIElements.JoinButton, HandleClick, 2);
        //}
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
        titleScreenUIElements.JoinButton.clicked -= HandleClick;
        lobbyManager.CreateOrJoinLobby();
        //GameObject.Find("TitleScreenUI").SetActive(false);
        // Wait until a CLIENT joins
    }
}
