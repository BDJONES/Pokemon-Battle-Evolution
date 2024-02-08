using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField] private GameObject[] team = new GameObject[6];
    [SerializeField] private GameObject activePokemonGameObject;
    public Pokemon[] pokemonTeam = new Pokemon[6];
    public Pokemon activePokemon;


    // Start is called before the first frame update
    void Start()
    {
        activePokemonGameObject = team[0];
        int i = 0;
        foreach (var go in team)
        {
            if (go == null)
            {
                pokemonTeam[i] = null;
            }
            else
            {
                pokemonTeam[i] = go.GetComponent<Pokemon>();
            }
            i++;
        }
        activePokemon = pokemonTeam[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Switch(int index)
    {
        if (index == 0)
        {
            Debug.LogError("Invalid Switch");
            return;
        }
        GameObject newPokemon = team[index];
        GameObject temp = activePokemonGameObject;
        // Play animation
        team[index] = temp;
        activePokemonGameObject = newPokemon;
        activePokemon = activePokemonGameObject.GetComponent<Pokemon>();
        temp.SetActive(false);
        activePokemonGameObject.SetActive(true);
        activePokemonGameObject.transform.position = temp.transform.position;
    }
}
