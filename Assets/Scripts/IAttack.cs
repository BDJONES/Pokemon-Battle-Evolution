using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    TypeModel type { get; set; }
    string moveCategory { get; set; }
    int power { get; set; }
    
    
    float accuracy{ get; set; }
    
    
    int powerPoints { get; set; }
    int maxPowerPoints { get; set; }
    void TriggerEffect();
}
