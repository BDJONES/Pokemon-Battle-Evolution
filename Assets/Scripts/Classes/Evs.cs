using UnityEditor;
using UnityEngine;


public class Evs : ScriptableObject
{
    public int hp;
    public int attack;
    public int defense;
    public int specialAttack;
    public int specialDefense;
    public int speed;
    private void Awake()
    {
        hp = 0;
        attack = 0;
        defense = 0;
        specialAttack = 0;
        specialDefense = 0;
        speed = 0;
    }
}
