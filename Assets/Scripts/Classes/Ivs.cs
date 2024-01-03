using UnityEditor;
using UnityEngine;


public class Ivs : ScriptableObject
{
    public int hp;
    public int attack;
    public int defense;
    public int specialAttack;
    public int specialDefense;
    public int speed;
    private void Awake()
    {
        hp = 31;
        attack = 31;
        defense = 31;
        specialAttack = 31;
        specialDefense = 31;
        speed = 31;
    }
}