using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PokemonTeamInfo : INetworkSerializable
{
    public FixedString128Bytes pokemon1;
    public FixedString128Bytes pokemon2;
    public FixedString128Bytes pokemon3;
    public FixedString128Bytes pokemon4;
    public FixedString128Bytes pokemon5;
    public FixedString128Bytes pokemon6;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref pokemon1);
        serializer.SerializeValue(ref pokemon2);
        serializer.SerializeValue(ref pokemon3);
        serializer.SerializeValue(ref pokemon4);
        serializer.SerializeValue(ref pokemon5);
        serializer.SerializeValue(ref pokemon6);
    }

    public void PrintAllPokemonNames()
    {
        Debug.Log(pokemon1);
        Debug.Log(pokemon2);
        Debug.Log(pokemon3);
        Debug.Log(pokemon4);
        Debug.Log(pokemon5);
        Debug.Log(pokemon6);
    }
}