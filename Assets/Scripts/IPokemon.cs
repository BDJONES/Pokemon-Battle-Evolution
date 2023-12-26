#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPokemon : MonoBehaviour
{
    string speciesName;
    int level;
    List<IAbility> ability;
    string gender;
    IType Type1;
    IType? Type2;
    List<IAttack> moveSet;
    List<IAttack> learnSet;
    IItem heldItem;
    int hitPoints;
    int attack;
    int defense;
    int specialAttack;
    int specialDefense;
    int speed;
    Dictionary<string, int> ivs;
    Dictionary<string, int> evs;
    //TypeModel newType = TypeModel.Bug;
}
