using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TrainerController : NetworkBehaviour
{
    [SerializeField] private Trainer player;
    [SerializeField] private Trainer opponent;
    private UIInputGrabber uIGrabber;
    private DialogueBoxController dialogueBoxController;
    private int inactiveTurnCount;
    public event Action playerTooInactive;
    [SerializeField] private GameObject myCamera;
    [SerializeField] private GameObject myUI;
    private AudioListener myAudioListener;
    private bool isUIActive = false;
    private RPCManager moveSelectionRPCManager;

    //public static event Action<GameObject> NewPlayerConnected;
    //public override void OnNetworkSpawn()
    //{
    //    //NetworkObjectId
    //    GameObject player = NetworkManager.SpawnManager.GetLocalPlayerObject().gameObject;
    //    if (player == null)
    //    {
    //        Debug.Log("The player is null");
    //    }
    //    if (NewPlayerConnected != null)
    //    {
    //        NewPlayerConnected.Invoke(player);
    //    }
    //    else
    //    {
    //        Debug.Log("There is an issue with the event");
    //    }
    //    return;
    //}
    private void OnEnable()
    {
        //Debug.Log("In TrainerController OnEnable");
        dialogueBoxController = transform.GetChild(0).gameObject.GetComponentInChildren<DialogueBoxController>();
        NetworkCommands.UIControllerCreated += SetUIActive;
        GameManager.GameManagerSpawned += SetMoveSelectionRPCManager;
        //myAudioListener = gameObject.GetComponentInChildren<AudioListener>();
        inactiveTurnCount = 0;
        
    }

    private void SetMoveSelectionRPCManager()
    {
        moveSelectionRPCManager = GameObject.Find("Game Manager").GetComponent<GameManager>().RPCManager;
    }

    public Trainer GetPlayer()
    {
        return player;
    }

    public Trainer GetOpponent() { 
        return opponent;
    }

    public DialogueBoxController GetDialogueBoxController()
    {
        return dialogueBoxController;
    }

    public void SetOpponent(Trainer Opponent)
    {
        opponent = Opponent;
    }

    public void SetCameraActive()
    {
        if (!IsOwner) return;
        //Debug.Log("Enabling the Camera");
        myCamera.SetActive(true);
    }

    public void SetUIActive()
    {
        if (!IsOwner || isUIActive) return;
        ////Debug.Log("Enabling UI");
        myUI.SetActive(true);
        //myUI.gameObject.GetComponent<NetworkObject>().Spawn(true);
        uIGrabber = new UIInputGrabber(myUI);
        isUIActive = true;
    }

    public async UniTask<IPlayerAction> SelectMove()
    {
        IPlayerAction selection = null;
        Action anonFunc = () =>
        {
            // Invoke the handler and set value of selection to returned Action
            selection = uIGrabber.GetSelectedAction();
            Debug.Log("Selection was made");
        };

        UIInputGrabber.MoveSelected += anonFunc;
        while (selection == null)//&& TimeManager.Instance.IsTurnTimerActive()
        {
            await UniTask.Yield();
        }
        //await dialogueBoxController.ReadAllQueuedDialogue();
        UIInputGrabber.MoveSelected -= anonFunc;
        // If a move was selected return that value
        if (selection != null)
        {
            return selection;
        }
        else
        {
            Debug.Log("Time out of the turn. Will select a random move.");
            inactiveTurnCount++;
            if (inactiveTurnCount >= 3)
            {
                playerTooInactive?.Invoke();
            }
            var attacks = player.GetActivePokemon().GetMoveset();
            int randomMove = UnityEngine.Random.Range(0, 3);
            return attacks[randomMove];
        }
    }

    public async UniTask<Switch> SwitchOutFaintedPokemon()
    {
        Debug.Log("You can now choose something to do.");
        Switch selection = null;
        if (this.gameObject != null && this.gameObject.name != "Trainer # 2")
        {
            Action anonFunc = () =>
            {
                // Invoke the handler and set value of selection to returned Action
                selection = (Switch) uIGrabber.GetSelectedAction();
                Debug.Log("Selection was made");
            };

            UIInputGrabber.MoveSelected += anonFunc;
            while (selection == null && TimeManager.Instance.IsTurnTimerActive())
            {
                await UniTask.Yield();
            }
            UIInputGrabber.MoveSelected -= anonFunc;
        }
        // If a move was selected return that value
        if (selection != null)
        {
            return selection;
        }
        else
        {
            inactiveTurnCount++;
            if (inactiveTurnCount >= 3)
            {
                playerTooInactive.Invoke();
            }
            List<int> availablePokemon = new List<int>();
            var team = player.GetPokemonTeam();
            for (int i = 0; i < team.Length; i++)
            {
                if (team[i] != null && !team[i].IsDead())
                {
                    availablePokemon.Add(i);
                }
            }
            int randomIndex = UnityEngine.Random.Range(0, availablePokemon.Count);
            return new Switch(player, team[availablePokemon[randomIndex]]);
        }
    }

    private void Update()
    {
        //Debug.Log("Enabling the Camera");
        //if (myCamera.activeInHierarchy == false)
        //{
        //myCamera.SetActive(true);
        //}
        //if (myUI.activeInHierarchy == false)
        //{
        //Debug.Log("Enabling UI");
        //myUI.SetActive(true);
        //}

        //myAudioListener.enabled = true;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            NetworkObject.gameObject.name = "Me";
            if (!IsHost)
            {
                var trainers = FindObjectsOfType<Trainer>();
                foreach (Trainer trainer in trainers)
                {
                    if (gameObject != trainer.gameObject)
                    {
                        Debug.Log("Found the opponent");
                        SetOpponent(trainer);
                        break;
                    }
                }
            }
            //if (IsHost)
            //{
            //    NetworkObject.Spawn(true);

            //}
            //NetworkObject.gameObject.transform.Find("YourUI").gameObject.SetActive(false);
            SetCameraActive();
            //if (!IsHost)
            //{
            //    SetUIActive();
            //}
        }
    }

    public static bool IsOwnerHost(GameObject gameObject)
    {
        if (gameObject.GetComponent<NetworkObject>().IsOwnedByServer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async void SendMoveSelect(int type)
    {
        if (IsOwner)
        {
            GameManager gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
            IPlayerAction selection = await SelectMove();
            if (selection.GetType().IsSubclassOf(typeof(Attack)))
            {
                Debug.Log("The selection was an Attack");
                var convertedSelection = (Attack) selection;
                AttackRPCTransfer attackRPCTrasfer = new AttackRPCTransfer();
                attackRPCTrasfer.attackName = convertedSelection.GetAttackName();
                attackRPCTrasfer.remainingPP = convertedSelection.GetCurrentPP();
                gm.RecieveAttackSelectionRpc(type, attackRPCTrasfer);
            }
            else
            {
                Debug.Log("The selection was a Switch");
                var convertedSelection = (Switch) selection;
                SwitchRPCTransfer switchRPCTransfer = new SwitchRPCTransfer();
                GameObject player = GameObject.Find("Me");
                switchRPCTransfer.trainerClientID = player.GetComponent<NetworkObject>().OwnerClientId;
                switchRPCTransfer.pokemonIndex = FindPokemonIndex(convertedSelection.GetPokemon().GetSpeciesName());
                gm.RecieveSwitchSelectionRpc(type, switchRPCTransfer);
            }
            return;
        }
    }

    private int FindPokemonIndex (string species)
    {
        Trainer trainer = GetPlayer();
        for (int i = 1; i < 6; i++)
        {
            if (trainer.GetPokemonTeam()[i].GetSpeciesName().Equals(species))
            {
                return i;
            }
        }
        Debug.Log("Unable to find the Pokemon");
        return -1;
    }
}
