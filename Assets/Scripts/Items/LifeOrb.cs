using Cysharp.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

public class LifeOrb : Item
{
    private EventsToTriggerManager eventsToTriggerManager;
    private static event Action<int> LifeOrbTriggerEvent;
    private TrainerController trainerContoller;
    public LifeOrb()
    {
        GameObject eventsToTriggerGO = GameObject.Find("EventsToTriggerManager");
        eventsToTriggerManager = eventsToTriggerGO.GetComponent<EventsToTriggerManager>();
        eventsToTriggerManager.OnTriggerEvent += HandlePokemonAttack;
        GameManager.OnStateChange += HandleStateChange;
        LifeOrbTriggerEvent += TriggerEffect;
    }

    ~LifeOrb()
    {
        eventsToTriggerManager.OnTriggerEvent -= HandlePokemonAttack;
    }

    protected override void InitializeItem()
    {
        this.itemName = "Life Orb";
        this.description = "An item to be held by a Pokémon. It boosts the power of the holder’s moves, but the holder also loses a small amount of HP with each attack it lands.";
        this.priority = 0;
        foreach(var attack in holder.GetMoveset())
        {
            if (attack.GetAttackPower() > 0)
            {
                attack.SetAttackPower(Mathf.FloorToInt(1.3f * attack.GetAttackPower()));
            }
        }
        foreach (var attack in holder.GetMoveset())
        {
            Debug.Log($"{attack.GetAttackName()} has {attack.GetAttackPower()} base power");
        }
    }

    private void HandleStateChange(GameState state)
    {
        if (state == GameState.BattleStart)
        {
            InitializeItem();
        }
    }

    private void HandlePokemonAttack(EventsToTrigger e)
    {
        
        if (holder == null)
        {
            return;
        }
        if (NetworkManager.Singleton.IsHost)
        {            
            trainerContoller = holder.transform.parent.parent.gameObject.GetComponent<TrainerController>();
            if ((holder.IsOwner && holder.ActiveState && e == EventsToTrigger.YourPokemonAttackedOpposingPokemon) || (!holder.IsOwner && holder == trainerContoller.GetPlayer().GetActivePokemon() && e == EventsToTrigger.OpposingPokemonAttackedYourPokemon))
            {
                Debug.Log($"Hello from the HandleAttack LifeOrb function, {trainerContoller.gameObject.name}, event = {e}");
                Debug.Log("Activating the LifeOrb");
                EffectRPCTransfer lifeOrbEffect = new EffectRPCTransfer();
                lifeOrbEffect.effectName = GetType().Name;
                lifeOrbEffect.priority = this.priority;
                lifeOrbEffect.userSpeed = this.holder.GetSpeedStat();
                if (holder.transform.parent.parent.gameObject.name == "Me")
                {
                    lifeOrbEffect.userType = 1;
                }
                else
                {
                    lifeOrbEffect.userType = 2;
                }
                EffectQueueController.Instance.AddEffectToQueueRpc(lifeOrbEffect);
            }
        }
    }

    public static void InvokeEffect(int userType)
    {
        LifeOrbTriggerEvent?.Invoke(userType);
    }

    protected async override void TriggerEffect(int userType)
    {
        if (holder != null)
        {
            trainerContoller = holder.transform.parent.parent.gameObject.GetComponent<TrainerController>();
            uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        }
        Debug.Log($"UserType = {userType}");
        if ((holder.IsOwner && userType == 1 && holder == trainerContoller.GetPlayer().GetActivePokemon()) || (!holder.IsOwner && userType == 2 && holder == trainerContoller.GetPlayer().GetActivePokemon())) 
        {
            int activeRPCs;
            int oldHPStat = holder.GetHPStat();
            int lifeOrbCost = Mathf.FloorToInt(holder.GetMaxHPStat() * .1f);
            Debug.Log($"oldHPStat is {oldHPStat}");
            //OpposingPokemonInfoController.ChangeOldHP(oldHPStat);
            //PokemonInfoController.ChangeOldHP(oldHPStat);
            if (holder.GetHPStat() - lifeOrbCost < 0)
            {
                holder.SetHPStat(0);
            }
            else
            {
                holder.SetHPStat(holder.GetHPStat() - lifeOrbCost);
                Debug.Log($"{holder.GetNickname()}'s HP = {holder.GetHPStat()}");
            }
            await UniTask.WaitForSeconds(0.3f);
            activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
            //Debug.Log($"Trainer name is = {holder.transform.parent.parent.gameObject.name}");
            if (holder.transform.parent.parent.gameObject.name == "Me")
            {
                GameManager.Instance.SendDialogueToClientRpc($"Your opponent's {holder.GetNickname()} lost some HP due to their Life Orb.");
                GameManager.Instance.SendDialogueToHostRpc($"Your {holder.GetNickname()} lost some HP due to their Life Orb.");
            }
            else
            {
                GameManager.Instance.SendDialogueToHostRpc($"Your opponent's {holder.GetNickname()} lost some HP due to their Life Orb.");
                GameManager.Instance.SendDialogueToClientRpc($"Your {holder.GetNickname()} lost some HP due to their Life Orb.");
            }
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }

            if (userType == 1)
            {
                activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 1);
                uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 2);
                GameManager.Instance.UpdateHealthBarRpc(Attacker.Trainer2, oldHPStat);
            }
            else if (userType == 2)
            {
                activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                uIController.UpdateMenuRpc(Menus.PokemonDamagedScreen, 2);
                uIController.UpdateMenuRpc(Menus.OpposingPokemonDamagedScreen, 1);
                GameManager.Instance.UpdateHealthBarRpc(Attacker.Trainer1, oldHPStat);
            }
            while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
            {
                await UniTask.Yield();
            }

            uIController.UpdateMenuRpc(Menus.DialogueScreen, 1);
            uIController.UpdateMenuRpc(Menus.DialogueScreen, 2);
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
}