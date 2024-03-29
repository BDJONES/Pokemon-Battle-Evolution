﻿using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class Ivs : INetworkSerializable
{
    [SerializeField] public int hp;
    [SerializeField] public int attack;
    [SerializeField] public int defense;
    [SerializeField] public int specialAttack;
    [SerializeField] public int specialDefense;
    [SerializeField] public int speed;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref hp);
        serializer.SerializeValue(ref attack);
        serializer.SerializeValue(ref defense);
        serializer.SerializeValue(ref specialAttack);
        serializer.SerializeValue(ref specialDefense);
        serializer.SerializeValue(ref speed);
    }
    public Ivs()
    {
        hp = 31;
        attack = 31;
        defense = 31;
        specialAttack = 31;
        specialDefense = 31;
        speed = 31;
    }
}