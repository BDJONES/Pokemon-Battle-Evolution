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
        // Trigger Tackle
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Tackle was used");
            Attack move1 = pokemon1.GetMoveset()[0];
            int damage = CalculateDamage(move1);
            //Debug.Log($"Damage = {damage}");
            if (trainer2.activePokemonScript.GetHPStat() - damage <= 0)
            {
                trainer2.activePokemonObject.GetComponent<PoketCreature>().SetHPStat(0);
                //Debug.Log("Opponent fainted");
                Destroy(trainer2.activePokemonObject);
            }
            trainer2.activePokemonScript.SetHPStat(trainer2.activePokemonScript.GetHPStat() - damage);
            move1.UseAttack(pokemon1, pokemon2);
        }
        // Trigger Flamethrower
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Flamethrower was used");
            Attack move2 = pokemon1.GetMoveset()[1];
            int damage = CalculateDamage(move2);
            Debug.Log($"Damage = {damage}");
            if (trainer2.activePokemonScript.GetHPStat() - damage <= 0)
            {
                trainer2.activePokemonObject.GetComponent<PoketCreature>().SetHPStat(0);
                Debug.Log("Opponent fainted");
                Destroy(trainer2.activePokemonObject);
            }
            trainer2.activePokemonScript.SetHPStat(trainer2.activePokemonScript.GetHPStat() - damage);
            move2.UseAttack(pokemon1, pokemon2);
        }
        // Trigger Earthquake
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Earthquake was used");
            Attack move3 = pokemon1.GetMoveset()[2];
            int damage = CalculateDamage(move3);
            Debug.Log($"Damage = {damage}");
            if (trainer2.activePokemonScript.GetHPStat() - damage <= 0)
            {
                trainer2.activePokemonObject.GetComponent<PoketCreature>().SetHPStat(0);
                Debug.Log("Opponent fainted");
                Destroy(trainer2.activePokemonObject);
            }
            trainer2.activePokemonScript.SetHPStat(trainer2.activePokemonScript.GetHPStat() - damage);
            move3.UseAttack(pokemon1, pokemon2);
        }
        // Trigger Thunder Wave
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Thunder Wave was used");
            Attack move4 = pokemon1.GetMoveset()[3];
            int damage = CalculateDamage(move4);
            Debug.Log($"Damage = {damage}");
            if (trainer2.activePokemonScript.GetHPStat() - damage <= 0)
            {
                trainer2.activePokemonObject.GetComponent<PoketCreature>().SetHPStat(0);
                Debug.Log("Opponent fainted");
                Destroy(trainer2.activePokemonObject);
            }
            trainer2.activePokemonScript.SetHPStat(trainer2.activePokemonScript.GetHPStat() - damage);
            move4.UseAttack(pokemon1, pokemon2);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Intimidate was used");
            Ability ability = pokemon1.GetAbility();
            ability.TriggerEffect(pokemon1, pokemon2);
        }
    }

    private int CalculateDamage(Attack attack)
    {
        int stab = 1; // Same Type Attack Bonus
        float typeMatchup = 1f;
        int damageRange = Random.Range(217, 255);
        
        // Damage Formula comes from this attack https://www.math.miami.edu/~jam/azure/compendium/battdam.htm
        // Visual for Formula https://gamerant.com/pokemon-damage-calculation-help-guide/
        if (attack.GetAttackCategory() == AttackCategory.Physical)
        {
            Debug.Log("Fired a Physical Attack");
            int step1 = (2 * trainer1.activePokemonScript.GetLevel() / 5 + 2);
            //Debug.Log($"Step 1 = {step1}");
            int step2 = step1 * trainer1.activePokemonScript.GetAttackStat() * attack.GetAttackPower();
            //Debug.Log($"Step 2 = {step2}");
            int step3 = step2 / trainer2.activePokemonScript.GetDefenseStat();
            //Debug.Log($"Step 3 = {step3}");
            int step4 = step3 / 50;
            //Debug.Log($"Step 4 = {step4}");
            int step5 = step4 + 2;
            //Debug.Log($"Step 5 = {step5}");
            int step6 = step5 * stab;
            //Debug.Log($"Step 6 = {step6}");
            float step7 = step6 * typeMatchup;
            //Debug.Log($"Step 7 = {step7}");
            float step8 = step7 * damageRange;
            //Debug.Log($"Step 8 = {step8}");
            int step9 = Mathf.FloorToInt(step8 / 255);
            //Debug.Log($"Step 9 = {step9}");
            int damage = step9; 
            return damage;
            
        }
        else if (attack.GetAttackCategory() == AttackCategory.Special)
        {
            Debug.Log("Fired a Special Attack");
            //Debug.Log($"Special Attack Stat = {pokemon1.GetSpecialAttackStat()}");
            int step1 = (2 * trainer1.activePokemonScript.GetLevel() / 5 + 2);
            //Debug.Log($"Step 1 = {step1}");
            int step2 = step1 * trainer1.activePokemonScript.GetSpecialAttackStat() * attack.GetAttackPower();
            //Debug.Log($"Step 2 = {step2}");
            int step3 = step2 / trainer2.activePokemonScript.GetSpecialDefenseStat();
            //Debug.Log($"Step 3 = {step3}");
            int step4 = step3 / 50;
            //Debug.Log($"Step 4 = {step4}");
            int step5 = step4 + 2;
            //Debug.Log($"Step 5 = {step5}");
            int step6 = step5 * stab;
            //Debug.Log($"Step 6 = {step6}");
            float step7 = step6 * typeMatchup;
            //Debug.Log($"Step 7 = {step7}"); 
            float step8 = step7 * damageRange;
            //Debug.Log($"Step 8 = {step8}");
            int step9 = Mathf.FloorToInt(step8 / 255);
            //Debug.Log($"Step 9 = {step9}");
            int damage = step9;
            return damage;
        }
        else
        {
            return 0;
        }
    }
}
