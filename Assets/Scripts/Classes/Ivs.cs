using UnityEditor;
using UnityEngine;


public class Ivs
{
    [SerializeField] public int hp;
    [SerializeField] public int attack;
    [SerializeField] public int defense;
    [SerializeField] public int specialAttack;
    [SerializeField] public int specialDefense;
    [SerializeField] public int speed;
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