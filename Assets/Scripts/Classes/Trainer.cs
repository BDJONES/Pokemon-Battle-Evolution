using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField] public Pokemon[] team = new Pokemon[1];
    [SerializeField] public Pokemon activePokemon;

    // Start is called before the first frame update
    void Start()
    {
        activePokemon = team[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
