using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Progress;

public class EffectQueueController : NetworkSingleton<EffectQueueController>
{
    public List<EffectRPCTransfer> effectList;
    public Queue<EffectRPCTransfer> effectQueue;

    private void OnEnable()
    {
        Debug.Log("Enabling the list and queue");
        effectList = new List<EffectRPCTransfer>();
        effectQueue = new Queue<EffectRPCTransfer>();
        if (effectList != null)
        {
            Debug.Log("effect list is initialized");
        }
        if (effectQueue != null)
        {
            Debug.Log("effect queue is initialized");
        }
    }

    [Rpc(SendTo.Server)]
    public void AddEffectToQueueRpc(EffectRPCTransfer newEffect)
    {
        Debug.Log($"Added a new Effect = {newEffect.effectName}");
        effectList.Add(newEffect);
        Debug.Log($"Now there are {effectList.Count} effects waiting to be triggered");
    }

    public async UniTask EmptyQueue()
    {
        //if (effectList.Count == 0)
        //{
        //    await UniTask.WaitForSeconds(1);
        //}
        //Debug.Log($"Num items in List = {effectList.Count}");
        if (effectList.Count > 0)
        {
            SortEffectQueue();
            while (effectQueue.Count > 0)
            {
                EffectRPCTransfer effect = effectQueue.Dequeue();
                GameManager.Instance.RPCManager.BeginRPCBatch();
                int activeRPCs = GameManager.Instance.RPCManager.ActiveRPCs();
                TriggerEffect(effect);
                while (GameManager.Instance.RPCManager.ActiveRPCs() > activeRPCs)
                {
                    await UniTask.Yield();
                }
                await UniTask.WaitForSeconds(1);
            }
        }
    }

    private void SortEffectQueue()
    {
        var sortedEffects = effectList.OrderByDescending(item => item.priority).ThenByDescending(item => item.userSpeed);
        foreach (var effect in sortedEffects)
        {
            effectQueue.Enqueue(effect);
        }
        effectList.Clear();
    }
    
    private void TriggerEffect(EffectRPCTransfer effect)
    {
        Debug.Log("Triggering the effect, and starting an RPC Task");
        GameManager.Instance.AddRPCTaskRpc("EffectQueueController Trigger Effect");
        if (effect.effectName == "Leftovers")
        {
            Leftovers.InvokeEffect(effect.userType);
        }
        else if (effect.effectName == "LifeOrb")
        {
            LifeOrb.InvokeEffect(effect.userType);
        }
        else if (effect.effectName == "LightBall")
        {
            LightBall.InvokeEffect(effect.userType);
        }
    }
}
