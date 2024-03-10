using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
using UnityEngine.InputSystem.XR;
using System;

public class ClientButtonController : MonoBehaviour
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
            UIEventSubscriptionManager.Subscribe(titleScreenUIElements.ClientButton, HandleClick);
        }
    }

    private void OnDisable()
    {
        uIController.OnMenuChange -= HandleMenuChange;
    }

    private void Start()
    {
        titleScreenUIElements = uIController.GetComponent<TitleScreenUIElements>();
    }
    private void HandleClick()
    {
        Debug.Log("Started as a client");
        NetworkManager.Singleton.StartClient();
        // Wait until a HOST joins
    }
}