using Unity.Collections;
using Unity.Netcode;

public struct SwitchRPCTransfer : INetworkSerializable
{
    public int pokemonIndex;
    public ulong trainerNetworkObjectID;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref pokemonIndex);
        serializer.SerializeValue(ref trainerNetworkObjectID);
    }
}