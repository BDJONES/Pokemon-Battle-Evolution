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
        titleScreenUIElements = gameObject.GetComponent<TitleScreenUIElements>();
        lobbyManager = GameObject.Find("Lobby Manager").GetComponent<LobbyManager>();
        titleScreenUIElements.JoinButton.clicked += HandleClick;
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.TitleScreen)
        {

        }
    }

    private void OnDisable()
    {
        //titleScreenUIElements.JoinButton.clicked -= HandleClick;
    }

    private void HandleClick()
    {
        titleScreenUIElements.JoinButton.clicked -= HandleClick;
        lobbyManager.CreateOrJoinLobby();
    }
}
