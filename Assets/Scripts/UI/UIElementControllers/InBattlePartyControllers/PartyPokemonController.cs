using Cysharp.Threading.Tasks;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PartyPokemonController : NetworkBehaviour
{
    [SerializeField] protected TrainerController trainerController;
    public static event Action<IPlayerAction> InputReceived;
    public void ReceiveInput(IPlayerAction input)
    {
        // Process the input
        Console.WriteLine("Input received: " + input);

        // Raise the event with the received input
        InputReceived?.Invoke(input);
    }
    protected InBattlePartyUIElements battlePartyUIElements;
    protected InBattlePartyDialogueUIElements battlePartyDialogueUIElements;
    public event EventHandler<OnSwitchEventArgs> SwitchSelected;
    protected UIController uIController;
    protected Switch switchPA;
    protected Menus previousMenu;

    private void Start()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        battlePartyUIElements = uIController.gameObject.GetComponent<InBattlePartyUIElements>();
        battlePartyDialogueUIElements = uIController.gameObject.GetComponent<InBattlePartyDialogueUIElements>();
        if (battlePartyUIElements != null)
        {
            Debug.Log("Battle Party Elements are here");
        }
        if (battlePartyDialogueUIElements != null)
        {
            Debug.Log("Dialogue Elements are here");
        }
        AttachButton();

    }

    protected abstract void AttachButton();
    protected void InitializeSwitch(Pokemon pokemon)
    {
        switchPA = new Switch(trainerController.GetPlayer(), pokemon);
    }


    protected async void PartyPokemonClicked()
    {
        Debug.Log("PartyPokemon Clicked");
        if (!switchPA.GetPokemon().IsDead() && switchPA.GetPokemon() != trainerController.GetPlayer().GetActivePokemon())
        {
            Debug.Log("Valid Switch Occuring");
            var args = new OnSwitchEventArgs
            {
                Switch = this.switchPA
            };
            SwitchSelected?.Invoke(this, args);
            ReceiveInput(switchPA);
        }
        else
        {
            Debug.Log("Invalid Switch Occuring");
            UIController uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("I'm the Host");
                if (uIController != null)
                {
                    Debug.Log("I found the UI Controller");
                    int activeRPCs;
                    GameManager.Instance.SendDialogueToHostRpc($"{switchPA.GetPokemon().GetNickname()} could not be switched in");
                    //while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                    //{
                    //    await UniTask.Yield();
                    //}
                    uIController.UpdateMenuRpc(Menus.InBattlePartyDialogueScreen, 1);
                    activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                    GameManager.Instance.RequestHostReadFirstDialogueRpc();
                    while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                    {
                        await UniTask.Yield();
                    }
                    uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 1);
                }
                else
                {
                    Debug.Log("UI Controller could not be found");
                }
            }
            else
            {
                Debug.Log("I'm the Client");
                if (uIController != null)
                {
                    DialogueBoxController dialogueBox = trainerController.GetDialogueBoxController();
                    dialogueBox.AddDialogueToQueue($"{switchPA.GetPokemon().GetNickname()} could not be switched in");
                    uIController.UpdateMenuRpc(Menus.InBattlePartyDialogueScreen, 2);
                    while (uIController.GetCurrentTrainer2Menu() != Menus.InBattlePartyDialogueScreen)
                    {
                        await UniTask.Yield();
                    }
                   
                    await dialogueBox.ReadOneDialogueTest();
                    uIController.UpdateMenuRpc(Menus.GeneralBattleMenu, 2);
                }
                else
                {
                    Debug.Log("UI Controller could not be found");
                }
            }
        }
    }

    public void SetTrainerController(TrainerController tc)
    {
        trainerController = tc;
    }

    [Rpc(SendTo.Server)] 
    private void SetInvalidSwitchDialogueForClientRpc()
    {
        Debug.Log("Dialogue is Being Set");
        SetClientDialogue();
    }

    private async void SetClientDialogue()
    {
        int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
        GameManager.Instance.SendDialogueToClientRpc($"{switchPA.GetPokemon().GetNickname()} could not be switched in");
        while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
        {
            await UniTask.Yield();
        }
        GameManager.Instance.RequestClientReadFirstDialogueRpc();
        while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
        {
            await UniTask.Yield();
        }
    }
}