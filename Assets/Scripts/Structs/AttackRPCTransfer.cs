using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct AttackRPCTransfer : INetworkSerializable
{
    public FixedString128Bytes attackName;
    public int remainingPP;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref attackName);
        serializer.SerializeValue(ref remainingPP);
    }
}