using Cysharp.Threading.Tasks;
using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Leftovers : Item
{
    private static Action<int> LeftoverTriggerEvent;
    TrainerController trainerContoller;
    public Leftovers()
    {
        GameManager.OnStateChange += GameStateOnChangeHandler;
        LeftoverTriggerEvent += TriggerEffect;
        this.uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
    }

    ~Leftovers()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
    }

    public static void InvokeEffect(int userType)
    {
        LeftoverTriggerEvent?.Invoke(userType);
    }

    private void GameStateOnChangeHandler(GameState state)
    {
        if (holder == null) return;
        trainerContoller = holder.transform.parent.parent.gameObject.GetComponent<TrainerController>();
        if (state == GameState.LoadingPokemonInfo)
        {
            InitializeItem();
        }
        else if (state == GameState.TurnEnd && (holder.ActiveState || holder == trainerContoller.GetPlayer().GetActivePokemon()))
        {
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("The Leftovers are activating");
                EffectRPCTransfer leftoversEffect = new EffectRPCTransfer();
                leftoversEffect.effectName = GetType().Name;
                leftoversEffect.priority = this.priority;
                leftoversEffect.userSpeed = this.holder.GetSpeedStat();
                if (holder.transform.parent.parent.gameObject.name == "Me")
                {
                    leftoversEffect.userType = 1;
                }
                else
                {
                    leftoversEffect.userType = 2;
                }
                EffectQueueController.Instance.AddEffectToQueueRpc(leftoversEffect);
            }
        }
    }

    protected override void InitializeItem()
    {
        this.itemName = "Leftovers";
        this.description = "An item to be held by a Pokémon. The holder’s HP is slowly but steadily restored throughout a battle.";
        this.priority = 0;

    }

    protected override async void TriggerEffect(int userType)
    {
        if (holder != null)
        {
            trainerContoller = holder.transform.parent.parent.gameObject.GetComponent<TrainerController>();
        }
        else
        {
            return;
        }
        
        if ((holder.IsOwner && userType == 1 && holder == trainerContoller.GetPlayer().GetActivePokemon()) || (!holder.IsOwner && userType == 2 && holder == trainerContoller.GetPlayer().GetActivePokemon()))
        {
            Debug.Log($"UserType = {userType}");
            int activeRPCs;
            int oldHPStat = holder.GetHPStat();
            int hpRegained = Mathf.FloorToInt(0.0625f * holder.GetMaxHPStat());
            
            if (hpRegained < 1)
            {
                hpRegained = 1;
            }

            if (holder.GetHPStat() == holder.GetMaxHPStat())
            {
                GameManager.Instance.FinishRPCTaskRpc();
                return;
            }
            else if (holder.GetHPStat() + hpRegained > holder.GetMaxHPStat())
            {
                this.holder.SetHPStat(holder.GetMaxHPStat());
            }
            else
            {
                this.holder.SetHPStat(holder.GetHPStat() + hpRegained);
            }
            await UniTask.WaitForSeconds(1f);
            activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            // Need to add Dialogue and other Screen Info, which should be easy because I'm the host
            if (userType == 1)
            {
                GameManager.Instance.SendDialogueToClientRpc($"Your opponent's {holder.GetNickname()} restored some HP with their Leftovers.");
                GameManager.Instance.SendDialogueToHostRpc($"Your {holder.GetNickname()} restored some HP with their Leftovers.");
            }
            else if (userType == 2)
            {
                GameManager.Instance.SendDialogueToHostRpc($"Your opponent's {holder.GetNickname()} restored some HP with their Leftovers.");
                GameManager.Instance.SendDialogueToClientRpc($"Your {holder.GetNickname()} restored some HP with their Leftovers.");
            }
            Debug.Log("Sending Dialogue");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }

            if (userType == 1)
            {
                activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 1);
                uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 2);
                //await UniTask.WaitForSeconds(5);
                GameManager.Instance.UpdateHealthBarRpc(Attacker.Trainer2, oldHPStat);
            }
            else if (userType == 2)
            {
                activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 2);
                uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 1);
                //await UniTask.WaitForSeconds(5);
                GameManager.Instance.UpdateHealthBarRpc(Attacker.Trainer1, oldHPStat);
                
            }
            Debug.Log("Updating the Health Bar");
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }

            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
            Debug.Log("Changing Screen to Dialogue");
            activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            GameManager.Instance.RequestClientReadFirstDialogueRpc();
            GameManager.Instance.RequestHostReadFirstDialogueRpc();
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }
            GameManager.Instance.FinishRPCTaskRpc();
        }
        
    }

    protected override void RevertEffect()
    {
        // Revert AttackStat to Base
        this.holder.SetAttackStat(Mathf.FloorToInt(0.01f * (2 * holder.GetBaseAttack() + holder.GetIvs().attack + Mathf.FloorToInt(0.25f * holder.GetEvs().attack)) * holder.GetLevel()) + 5);
        this.holder.SetSpecialAttackStat(Mathf.FloorToInt(0.01f * (2 * holder.GetBaseSpecialAttack() + holder.GetIvs().specialAttack + Mathf.FloorToInt(0.25f * holder.GetEvs().specialAttack)) * holder.GetLevel()) + 5);
        this.holder.AttackStage = holder.AttackStage;
        this.holder.SpecialAttackStage = holder.SpecialAttackStage;
    }
}