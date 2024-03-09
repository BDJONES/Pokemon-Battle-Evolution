using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JoinButtonController : MonoBehaviour
{
    private TitleScreenUIElements titleScreenUIElements;
    private UIController uIController;
    private LobbyManager lobbyManager;

    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uIController.OnMenuChange += HandleMenuChange;
        titleScreenUIElements = uIController.GetComponent<TitleScreenUIElements>();
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.TitleScreen)
        {
            titleScreenUIElements.JoinButton.clicked += HandleClick;
            UIEventSubscriptionManager.Subscribe(titleScreenUIElements.JoinButton, HandleClick);
        }
    }

    private void OnDisable()
    {
        uIController.OnMenuChange += HandleMenuChange;
    }

    private void Start()
    {
        titleScreenUIElements = GameObject.Find("UI Controller").GetComponent<TitleScreenUIElements>();
    }
    private void HandleClick()
    {
        lobbyManager.CreateOrJoinLobby();
        // Wait until a CLIENT joins
    }
}
