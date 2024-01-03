using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Attack
{
    private void Awake()
    {
        this.attackName = "Flamethrower";
        this.description = "The target is scorched with an intense blast of fire. This may also leave the target with a burn.";
        this.type = StaticTypeObjects.Fire;
        this.moveCategory = AttackCategory.Special;
        this.power = 90;
        this.accuracy = 100;
        this.currPowerPoints = 16;
        this.maxPowerPoints = 16;
    }



    // Start is called before the first frame update
    void Start()
    {
        //var flamethrower = new Flamethrower();
    }



    public override void TriggerEffect(Pokemon target)
    {
        base.TriggerEffect(target);
        int genereatedValue = Random.Range(0, 99);
        if (genereatedValue < 10)
        {
            if (target.GetHPStat() > 0)
            {
                target.SetStatus(StatusConditions.Burn);
                Debug.Log("Flamethrower burned the target");
            }

        }
    }
    public override bool UseAttack(Pokemon target)
    {
        if (this.accuracy == 100)
        {
            TriggerEffect(target);
        }
        else
        {
            int generatedValue = Random.Range(0, 99);
            int accurateRange = 100 - this.accuracy; 
            if (generatedValue < accurateRange)
            {
                Debug.Log("The Attack Missed");
                return false;
            }
            else
            {
                TriggerEffect(target);
            }
        }
        Debug.Log("The Attack Landed");
        return true;
    }
}
