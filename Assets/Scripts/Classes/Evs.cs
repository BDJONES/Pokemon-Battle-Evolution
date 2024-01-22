using UnityEditor;
using UnityEngine;


public class Evs
{
    [SerializeField] public int hp;
    [SerializeField] public int attack;
    [SerializeField] public int defense;
    [SerializeField] public int specialAttack;
    [SerializeField] public int specialDefense;
    [SerializeField] public int speed;
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
