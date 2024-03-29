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
        uIController.OnHostMenuChange += HandleMenuChange;
        uIController.OnClientMenuChange += HandleMenuChange;
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.TitleScreen)
        {
            var player = transform.parent.parent.gameObject;

            if (TrainerController.IsOwnerHost(player))
            {
                UIEventSubscriptionManager.Subscribe(titleScreenUIElements.ClientButton, HandleClick, 1);
            }
            else
            {
                UIEventSubscriptionManager.Subscribe(titleScreenUIElements.ClientButton, HandleClick, 2);
            }
        }
    }

    private void OnDisable()
    {
        uIController.OnHostMenuChange -= HandleMenuChange;
        uIController.OnClientMenuChange -= HandleMenuChange;
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