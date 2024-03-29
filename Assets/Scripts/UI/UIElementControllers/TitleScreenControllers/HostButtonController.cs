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
        uIController = transform.parent.gameObject.GetComponentInChildren<UIController>();
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
                UIEventSubscriptionManager.Subscribe(titleScreenUIElements.HostButton, HandleClick, 1);
            }    
            else
            {
                UIEventSubscriptionManager.Subscribe(titleScreenUIElements.HostButton, HandleClick, 2);
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
        Debug.Log("Started as a Host");
        NetworkManager.Singleton.StartHost();
        // Wait until a CLIENT joins
    }
}