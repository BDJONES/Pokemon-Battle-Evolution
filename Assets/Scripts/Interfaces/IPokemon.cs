#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPokemon
{
    string speciesName { get; set; }
    int level { get; set; }
    public List<IAbility> ability { get; set; }
    public Gender gender { get; set; }
    public IType Type1 { get; set; }
    public IType? Type2 { get; set; }
    public List<IAttack> moveSet { get; set; }
    public List<IAttack> learnSet { get; set; }
    public IItem heldItem { get; set; }
    public int hitPoints { get; set; }
    public int currHitPoints { get; set; }
    public int attack { get; set; }
    public int currAttack { get; set; }
    public int defense { get; set; }
    public int currDefense { get; set; }
    public int specialAttack { get; set; }
    public int currSpecialAttack { get; set; }
    public int specialDefense { get; set; }
    public int currSpecialDefense { get; set; }
    public int speed { get; set; }
    public int currSpeed { get; set; }
    public Dictionary<string, int> ivs { get; set; }
    public Dictionary<string, int> evs { get; set; }
}
