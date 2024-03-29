using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCManager
{
    private int rpcCounter = 0;
    private bool allRPCsCompleted = false;

    public void BeginRPCBatch()
    {
        //Debug.Log("Beginning an RPC Batch");
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
}
