using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Trainer trainer1;
    public Trainer trainer2;
    [SerializeField] Pokemon pokemon1;
    [SerializeField] Pokemon pokemon2;
    // Start is called before the first frame update
    void Start()
    {
        pokemon1 = trainer1.activePokemonScript;
        pokemon2 = trainer2.activePokemonScript;
    }

    // Update is called once per frame
    void Update()
    {
        //Trigger Flamethrower Attack
        if (Input.GetKeyDown(KeyCode.A))
        {
            //GameObject myObject = new GameObject("MyObject");
            Flamethrower flamethrower = ScriptableObject.CreateInstance<Flamethrower>();
            int damage = CalculateDamage(flamethrower);
            Debug.Log($"Damage = {damage}");
            if (trainer2.activePokemonScript.GetHPStat() - damage <= 0)
            {
                trainer2.activePokemonObject.GetComponent<PoketCreature>().SetHPStat(0);
                Debug.Log("Opponent fainted");
                Destroy(trainer2.activePokemonObject);
            }
            trainer2.activePokemonScript.SetHPStat(trainer2.activePokemonScript.GetHPStat() - damage);
            flamethrower.UseAttack(pokemon2);
        }
    }

    private int CalculateDamage(Attack attack)
    {
        int stab = 1; // Same Type Attack Bonus
        float typeMatchup = 1f;
        int damageRange = Random.Range(217, 255);

        // Damage Formula comes from this attack https://www.math.miami.edu/~jam/azure/compendium/battdam.htm
        // Visual for Formula https://gamerant.com/pokemon-damage-calculation-help-guide/
        if (attack.moveCategory == AttackCategory.Physical)
        {
            Debug.Log("Fired a Physical Attack");
            int step1 = (2 * trainer1.activePokemonScript.GetLevel() / 5 + 2);
            Debug.Log($"Step 1 = {step1}");
            int step2 = step1 * trainer1.activePokemonScript.GetAttackStat() * attack.power;
            Debug.Log($"Step 2 = {step2}");
            int step3 = step2 / trainer2.activePokemonScript.GetDefenseStat();
            Debug.Log($"Step 3 = {step3}");
            int step4 = step3 / 50;
            Debug.Log($"Step 4 = {step4}");
            int step5 = step4 + 2;
            Debug.Log($"Step 5 = {step5}");
            int step6 = step5 * stab;
            Debug.Log($"Step 6 = {step6}");
            float step7 = step6 * typeMatchup;
            Debug.Log($"Step 7 = {step7}");
            float step8 = step7 * damageRange;
            Debug.Log($"Step 8 = {step8}");
            int step9 = Mathf.FloorToInt(step8 / 255);
            Debug.Log($"Step 9 = {step9}");
            int damage = step9; 
            return damage;
            
        }
        else
        {
            Debug.Log("Fired a Special Attack");
            Debug.Log($"Special Attack Stat = {pokemon1.GetSpecialAttackStat()}");
            int step1 = (2 * trainer1.activePokemonScript.GetLevel() / 5 + 2);
            Debug.Log($"Step 1 = {step1}");
            int step2 = step1 * trainer1.activePokemonScript.GetSpecialAttackStat() * attack.power;
            Debug.Log($"Step 2 = {step2}");
            int step3 = step2 / trainer2.activePokemonScript.GetSpecialDefenseStat();
            Debug.Log($"Step 3 = {step3}");
            int step4 = step3 / 50;
            Debug.Log($"Step 4 = {step4}");
            int step5 = step4 + 2;
            Debug.Log($"Step 5 = {step5}");
            int step6 = step5 * stab;
            Debug.Log($"Step 6 = {step6}");
            float step7 = step6 * typeMatchup;
            Debug.Log($"Step 7 = {step7}"); 
            float step8 = step7 * damageRange;
            Debug.Log($"Step 8 = {step8}");
            int step9 = Mathf.FloorToInt(step8 / 255);
            Debug.Log($"Step 9 = {step9}");
            int damage = step9;
            return damage;
        }
    }
}
