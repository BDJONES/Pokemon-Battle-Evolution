using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonModel : MonoBehaviour
{
    string speciesName;
    int level;
    List<AbilityModel> ability;
    string gender;
    TypeModel Type1;
    TypeModel Type2;
    List<AttackModel> moveSet;
    ItemModel heldItem;
    int hitPoints;
    int attack;
    int defense;
    int specialAttack;
    int specialDefense;
    int speed;
    Dictionary<string, int> ivs;
    Dictionary<string, int> evs;
}
