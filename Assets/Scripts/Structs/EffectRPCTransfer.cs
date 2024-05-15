using Unity.Collections;
using Unity.Netcode;

public struct EffectRPCTransfer : INetworkSerializable
{
    public FixedString128Bytes effectName;
    public int priority;
    public int userSpeed;
    public int userType; // Host/Client
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref effectName);
        serializer.SerializeValue(ref priority);
        serializer.SerializeValue(ref userSpeed);
        serializer.SerializeValue(ref userType);
    }
}