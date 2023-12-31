using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Type type { get; set; }
    AttackCategory moveCategory { get; set; }
    int power { get; set; }
    
    
    float accuracy{ get; set; }
    
    
    int powerPoints { get; set; }
    int maxPowerPoints { get; set; }
    void TriggerEffect() {
        return;
    }
}