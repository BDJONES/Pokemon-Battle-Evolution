using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    IType type { get; set; }
    Category moveCategory { get; set; }
    int power { get; set; }
    
    
    float accuracy{ get; set; }
    
    
    int powerPoints { get; set; }
    int maxPowerPoints { get; set; }
    void TriggerEffect();
}

public enum Category
{
    Physical,
    Special
}
