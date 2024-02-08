using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PartyPokemonController : MonoBehaviour
{
    public static event Action<IPlayerAction> InputReceived;
    public void ReceiveInput(IPlayerAction input)
    {
        // Process the input
        Console.WriteLine("Input received: " + input);

        // Raise the event with the received input
        InputReceived?.Invoke(input);
    }
    protected InBattlePartyUIElements battlePartyUIElements;
    public event EventHandler<OnSwitchEventArgs> SwitchSelected;
    protected Switch switchPA;
    protected Menus previousMenu;

    private void Start()
    {
        battlePartyUIElements = GameObject.Find("UI Controller").GetComponent<InBattlePartyUIElements>();
        AttachButton();
    }

    protected abstract void AttachButton();
    protected void InitializeSwitch(Pokemon pokemon)
    {
        switchPA = new Switch(GameManager.Instance.trainer1, pokemon);
    }


    protected void PartyPokemonClicked()
    {
        Debug.Log("PartyPokemon Clicked");
        var args = new OnSwitchEventArgs
        {
            Switch = this.switchPA
        };
        SwitchSelected?.Invoke(this, args);
        ReceiveInput(switchPA);
    }
}