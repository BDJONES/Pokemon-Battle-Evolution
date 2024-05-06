using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RPCManager : INetworkSerializable
{
    private int rpcCounter = 0;
    private bool allRPCsCompleted = false;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref  rpcCounter);
        serializer.SerializeValue(ref allRPCsCompleted);
    }

    public void BeginRPCBatch()
    {
        Debug.Log("Beginning an RPC Batch");
        rpcCounter = 0;
        allRPCsCompleted = false;
    }

    public void RPCStarted()
    {
        rpcCounter++;
        Debug.Log($"Started a new RPC, there are now ({rpcCounter}) active RPCs");
    }

    public void RPCFinished()
    {
        rpcCounter--;
        Debug.Log("Just finished an RPC");
        if (rpcCounter == 0)
        {
            Debug.Log("All RPCs are completed");
            allRPCsCompleted = true;
        }
    }

    public bool AreAllRPCsCompleted() {
        return allRPCsCompleted; 
    }

    public void CurrentRPCCount()
    {
        Debug.Log($"There are Currently {rpcCounter} active RPCs");
    }
    public int ActiveRPCs()
    {
        return rpcCounter;
    }
}
