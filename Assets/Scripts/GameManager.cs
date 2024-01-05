using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Trainer trainer1;
    public Trainer trainer2;

    // Start is called before the first frame update
    void Start()
    {
        var player1Item = trainer1.activePokemon.GetItem();
        var player2Item = trainer2.activePokemon.GetItem();
        if (player1Item)
        {
            player1Item.TriggerEffect(trainer1.activePokemon);
        }
        if (player2Item)
        {
            player2Item.TriggerEffect(trainer2.activePokemon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Trigger Tackle
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Tackle was used");
            Attack move1 = trainer1.activePokemon.GetMoveset()[0];
            move1.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
            var player2Item = trainer2.activePokemon.GetItem();
            player2Item.RevertEffect(trainer2.activePokemon);
        }
        // Trigger Flamethrower
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Flamethrower was used");
            Attack move2 = trainer1.activePokemon.GetMoveset()[1];
            move2.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
        }
        // Trigger Earthquake
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Earthquake was used");
            Attack move3 = trainer1.activePokemon.GetMoveset()[2];
            move3.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
        }
        // Trigger Thunder Wave
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Thunder Wave was used");
            Attack move4 = trainer1.activePokemon.GetMoveset()[3];
            move4.UseAttack(trainer1.activePokemon, trainer2.activePokemon);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Intimidate was used");
            Ability ability = trainer1.activePokemon.GetAbility();
            ability.TriggerEffect(trainer1.activePokemon, trainer2.activePokemon);
        }
    }
}
