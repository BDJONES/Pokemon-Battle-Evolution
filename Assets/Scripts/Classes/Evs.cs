using Unity.Netcode;
using UnityEditor;
using UnityEngine;


public class Evs : INetworkSerializable
{
    public int hp;
    public int attack;
    public int defense;
    public int specialAttack;
    public int specialDefense;
    public int speed;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref hp);
        serializer.SerializeValue(ref attack);
        serializer.SerializeValue(ref defense);
        serializer.SerializeValue(ref specialAttack);
        serializer.SerializeValue(ref specialDefense);
        serializer.SerializeValue(ref speed);
    }

    public Evs()
    {
        hp = 0;
        attack = 0;
        defense = 0;
        specialAttack = 0;
        specialDefense = 0;
        speed = 0;
    }
}
