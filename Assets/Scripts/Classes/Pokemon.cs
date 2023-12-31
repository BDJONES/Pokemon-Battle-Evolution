#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pokemon: MonoBehaviour
{
    protected string speciesName = null!;
    protected int level;
    protected List<Ability> ability;
    protected Gender gender;
    protected Type Type1 = null!;
    protected Type? Type2 { get; set; }
    protected List<Attack> moveSet = null!;
    protected List<Attack> learnSet = null!;
    protected Item? heldItem;
    protected int hitPoints;
    protected int currHitPoints;
    protected int attack;
    protected int currAttack;
    protected int defense;
    protected int currDefense;
    protected int specialAttack;
    protected int currSpecialAttack;
    protected int specialDefense;
    protected int currSpecialDefense;
    protected int speed;
    protected int currSpeed;
    protected Ivs ivs = null!;
    protected Evs evs = null!;
}