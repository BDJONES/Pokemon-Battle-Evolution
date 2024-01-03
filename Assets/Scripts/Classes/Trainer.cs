using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField] public GameObject[] team = new GameObject[1];
    [SerializeField] public GameObject activePokemonObject;
    [SerializeField] public Pokemon activePokemonScript;

    // Start is called before the first frame update
    void Start()
    {
        team[0] = activePokemonObject;
        activePokemonScript = activePokemonObject.GetComponent<Pokemon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
