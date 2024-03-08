using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
using System;

public class HostButtonController : MonoBehaviour
{
    private TitleScreenUIElements titleScreenUIElements;
    private UIController uIController;

    private void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uIController.OnMenuChange += HandleMenuChange;
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.TitleScreen)
        {
            UIEventSubscriptionManager.Subscribe(titleScreenUIElements.HostButton, HandleClick);
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
        Debug.Log("Started as a Host");
        NetworkManager.Singleton.StartHost();
        // Wait until a CLIENT joins
    }
}