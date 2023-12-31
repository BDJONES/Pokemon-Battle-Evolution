using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoketCreature : Pokemon
{
    private PoketCreature()
    {
        speciesName = "Poket Creature";
        level = 100;
        gender = Gender.Male;
        Type1 = StaticTypeObjects.Fire;
        hitPoints = 60;
        attack = 60;
        defense = 60;
        specialAttack = 60;
        specialDefense = 60;
        speed = 60;
        ivs = new Ivs();
        evs = new Evs();
    }
}
