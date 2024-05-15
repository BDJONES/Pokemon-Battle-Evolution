using System;
using Unity.Netcode;
using UnityEngine;

public class LightBall : Item
{
    private static event Action<int> LightBallTriggerEvent;

    public LightBall()
    {
        GameManager.OnStateChange += GameStateOnChangeHandler;
        LightBallTriggerEvent += TriggerEffect;
    }

    ~LightBall()
    {
        GameManager.OnStateChange -= GameStateOnChangeHandler;
    }

    private void GameStateOnChangeHandler(GameState state)
    {
        if (state == GameState.BattleStart && holder.IsOwner)
        {
            Debug.Log("Activating the Light Ball");
            InitializeItem();
            EffectRPCTransfer lightBallEffect = new EffectRPCTransfer();
            lightBallEffect.effectName = GetType().Name;
            lightBallEffect.priority = this.priority;
            lightBallEffect.userSpeed = this.holder.GetSpeedStat();
            if (NetworkManager.Singleton.IsHost)
            {
                lightBallEffect.userType = 1;
            }
            else
            {
                lightBallEffect.userType = 2;
            }
            EffectQueueController.Instance.AddEffectToQueueRpc(lightBallEffect);
        }
    }

    protected override void InitializeItem()
    {
        this.itemName = "Light Ball";
        this.description = "An item to be held by Pikachu. It’s a mysterious orb that boosts Pikachu’s Attack and Sp. Atk stats.";
        this.priority = 7;
    }

    public static void InvokeEffect(int userType)
    {
        LightBallTriggerEvent?.Invoke(userType);
    }

    protected override void TriggerEffect(int userType)
    {
        if ((holder.IsOwner && userType == 1) || (!holder.IsOwner && userType == 2))
        {
            if (holder.GetSpeciesName() == "Pikachu")
            {
                Debug.Log("Light Ball is Powering up Pikachu");
                holder.SetAttackStat(2 * holder.GetAttackStat());
                holder.SetSpecialAttackStat(2 * holder.GetSpecialAttackStat());
            }
            GameManager.Instance.FinishRPCTaskRpc();
        }

    }
    protected override void RevertEffect()
    {
        // Revert AttackStat to Base
        holder.SetAttackStat(Mathf.FloorToInt(0.01f * (2 * holder.GetBaseAttack() + holder.GetIvs().attack + Mathf.FloorToInt(0.25f * holder.GetEvs().attack)) * holder.GetLevel()) + 5);
        holder.SetSpecialAttackStat(Mathf.FloorToInt(0.01f * (2 * holder.GetBaseSpecialAttack() + holder.GetIvs().specialAttack + Mathf.FloorToInt(0.25f * holder.GetEvs().specialAttack)) * holder.GetLevel()) + 5);
        holder.AttackStage = holder.AttackStage;
        holder.SpecialAttackStage = holder.SpecialAttackStage;
    }
}