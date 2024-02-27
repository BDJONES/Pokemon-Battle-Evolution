using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField] private GameObject[] teamObjects = new GameObject[6];
    [SerializeField] private GameObject activePokemonGameObject;
    private Pokemon[] pokemonTeam = new Pokemon[6];
    private Pokemon activePokemon;
    public string trainerName;

    // Start is called before the first frame update
    void Start()
    {
        activePokemonGameObject = teamObjects[0];
        int i = 0;
        foreach (var go in teamObjects)
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

    public bool isTeamDead()
    {
        foreach (Pokemon pokemon in pokemonTeam)
        {
            if (pokemon.GetHPStat() > 0)
            {
                return false;
            }
        }
        return true;
    }

    public Pokemon[] GetPokemonTeam()
    {
        return pokemonTeam;
    }

    public Pokemon GetActivePokemon()
    {
        return activePokemon;
    }

    public GameObject GetActivePokemonGameObject()
    {
        return activePokemonGameObject;
    }

    public void Switch(int index)
    {
        if (index == 0)
        {
            Debug.LogError("Invalid Switch");
            return;
        }
        activePokemon.ResetStatStages();
        GameObject newObject = teamObjects[index];
        Pokemon newPokemon = pokemonTeam[index];
        SwapPokemon(newObject, newPokemon, index);
 
    }

    private void SwapPokemon(GameObject newObject, Pokemon newPokemon, int index)
    {
        // Play animation
        GameObject tempGameObject = activePokemonGameObject;
        Pokemon tempPokemon = activePokemon;        
        teamObjects[index] = tempGameObject;
        pokemonTeam[index] = tempPokemon;
        activePokemonGameObject = newObject;
        activePokemon = newPokemon;
        pokemonTeam[0] = newPokemon;
        teamObjects[0] = newObject;
        SetLocation(tempGameObject, newObject);
    }

    private void SetLocation(GameObject oldPokemon, GameObject newPokemon)
    {
        //Debug.Log("In Set Location Function");
        oldPokemon.SetActive(false);
        newPokemon.SetActive(true);
        float oldX = oldPokemon.transform.position.x;
        float oldZ = oldPokemon.transform.position.z;
        float newPokemonHeight = newPokemon.GetComponent<Collider>().bounds.size.y;
        newPokemon.transform.position = new Vector3(oldX, 40.42f + (newPokemonHeight / 2), oldZ); 
    }
}
