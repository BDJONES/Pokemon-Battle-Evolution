using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Trainer : NetworkBehaviour
{
    [SerializeField] private GameObject[] teamObjects = new GameObject[6];
    [SerializeField] private GameObject activePokemonGameObject;
    private Pokemon[] pokemonTeam = new Pokemon[6];
    private Pokemon activePokemon;
    public string trainerName;
    public event Action<Trainer> UpdatedTeam;
    private RPCManager rpcManager;

    // Start is called before the first frame update
    private async void Start()
    {
        rpcManager = new RPCManager();
        UpdatePokemonList();
        await RandomizeTeam();
        UpdatePokemonList();
        //rpcManager.BeginRPCBatch();
        PlacePokemonRpc();
        //while (!rpcManager.AreAllRPCsCompleted())
        //{
        //    await UniTask.Yield();
        //}
        
        //activePokemonGameObject = teamObjects[0];
        //activePokemon = pokemonTeam[0];
        activePokemon.ActiveState = true;
        GameManager.OnStateChange += HandleMenuChange;
    }

    private void HandleMenuChange(GameState state)
    {
        if (state == GameState.LoadingPokemonInfo)
        {

        }
    }

    public bool IsTeamDead()
    {
        //UpdatePokemonList();
        Debug.Log("In IsTeamDead");
        int count = 0;
        foreach (Pokemon pokemon in pokemonTeam)
        {
            count++;
            if (pokemon == null)
            {
                
                continue;
            }
            //Debug.Log($"Pokemon {count} = {pokemon.GetHPStat()}");
            if (pokemon.GetHPStat() > 0)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdatePokemonList()
    {
        Debug.Log("Updating the Pokemon List");
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
                //Debug.Log();
            }
            i++;
        }
        activePokemon = pokemonTeam[0];
        activePokemonGameObject = activePokemon.gameObject;
        activePokemon.ActiveState = true;
        UpdatedTeam?.Invoke(this);
    }

    public GameObject[] GetTeamObjects()
    {
        return teamObjects;
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

    private void SetActivePokemon(Pokemon newPokemon)
    {
        activePokemon = newPokemon;
    }

    private void SetActivePokemonGameObject(GameObject newPokemonGO)
    {
        activePokemonGameObject = newPokemonGO;
    }

    private void SetPokemonTeam(Pokemon[] newTeam)
    {
        pokemonTeam = newTeam;
    }

    private void SetTeamObjects(GameObject[] newObjects)
    {
        teamObjects = newObjects;
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SwitchRpc(int index)
    {
        // WIll try 'if (IsOwner && IsHost)' 
        //Debug.Log("Starting SwitchRpc");
        //Debug.Log("I think I'm apart of an RPC Batch");
        if (index == 0)
        {
            Debug.LogError("Invalid Switch");
            return;
        }
        //GameManager.Instance.AddRPCTaskRpc();
        GameObject newObject = teamObjects[index];
        Pokemon newPokemon = pokemonTeam[index];
        SwapPokemon(newObject, newPokemon, index);
        //while(GameManager.Instance.RPCManager.ActiveRPCs() > 0)
        //{
        //    Debug.Log("Hello");
        //}
    }

    private void SwapPokemon(GameObject newObject, Pokemon newPokemon, int index)
    {
        // Play animation
        GameObject tempGameObject = activePokemonGameObject;
        Pokemon tempPokemon = activePokemon;        
        teamObjects[index] = tempGameObject;
        pokemonTeam[index] = tempPokemon;
        activePokemon.ResetStatStages();
        activePokemon.ResetBattleEffects();
        activePokemon.ActiveState = false;
        activePokemonGameObject = newObject;
        activePokemon = newPokemon;
        activePokemon.ActiveState = true;
        pokemonTeam[0] = newPokemon;
        teamObjects[0] = newObject;
        if (gameObject.name == "Me")
        {
            SetLocation(tempGameObject, newObject);
        }
        //GameManager.Instance.FinishRPCTaskRpc();
    }

    private void SetLocation(GameObject oldPokemon, GameObject newPokemon)
    {
        Debug.Log("In Set Location Function");
        //oldPokemon.SetActive(false);
        //newPokemon.SetActive(true);

        float oldX = oldPokemon.transform.position.x;
        float oldZ = oldPokemon.transform.position.z;
        //float newPokemonHeight = newPokemon.GetComponent<Collider>().bounds.size.y;
        //Debug.Log($"Gameobject name = {newPokemon.name}");

        if (IsOwner && !IsHost)
        {
            SendUpdatedPositionToServerRpc(oldPokemon.GetComponent<Pokemon>().GetSpeciesName(), newPokemon.transform.position, new Vector3(oldX, 0, oldZ));
            oldPokemon.transform.position = newPokemon.transform.position;
            newPokemon.transform.position = new Vector3(oldX, 0, oldZ);
            newPokemon.transform.eulerAngles = gameObject.transform.eulerAngles;
        }
        else if (IsOwner && IsHost)
        {
            oldPokemon.transform.position = newPokemon.transform.position;
            newPokemon.transform.position = new Vector3(oldX, 0, oldZ);
            newPokemon.transform.eulerAngles = gameObject.transform.eulerAngles;
        }
    }

    private async UniTask RandomizeTeam()
    {
        Debug.Log("Randomizing the team");
        if (!IsOwner) return;
        int n = 6;
        while (n > 1)
        {
            int randomIndex1 = UnityEngine.Random.Range(0, 6);
            int randomIndex2 = UnityEngine.Random.Range(0, 6);
            GameObject tempGO = teamObjects[randomIndex1];
            Pokemon tempPokemon = pokemonTeam[randomIndex1];
            teamObjects[randomIndex1] = teamObjects[randomIndex2];
            teamObjects[randomIndex2] = tempGO;
            pokemonTeam[randomIndex1] = pokemonTeam[randomIndex2];
            pokemonTeam[randomIndex2] = tempPokemon;
            n--;
        }
        if (!IsHost)
        {
            PokemonTeamInfo pokemonTeamInfo = new PokemonTeamInfo();
            for(int j = 0; j < pokemonTeam.Length; j++)
            {
                if (j == 0)
                {
                    pokemonTeamInfo.pokemon1 = pokemonTeam[j].GetSpeciesName();
                }
                else if (j == 1)
                {
                    pokemonTeamInfo.pokemon2 = pokemonTeam[j].GetSpeciesName();
                }
                else if (j == 2)
                {
                    pokemonTeamInfo.pokemon3 = pokemonTeam[j].GetSpeciesName();
                }
                else if (j == 3)
                {
                    pokemonTeamInfo.pokemon4 = pokemonTeam[j].GetSpeciesName();
                }
                else if (j == 4)
                {
                    pokemonTeamInfo.pokemon5 = pokemonTeam[j].GetSpeciesName();
                }
                else
                {
                    pokemonTeamInfo.pokemon6 = pokemonTeam[j].GetSpeciesName();
                }
            }
            rpcManager.BeginRPCBatch();
            UpdateTeamOnServerRpc(pokemonTeamInfo);
            RequestHostTeamRpc();
            while (rpcManager.AreAllRPCsCompleted())
            {
                await UniTask.Yield();
            }
        }
        activePokemon = pokemonTeam[0];
        activePokemonGameObject = activePokemon.gameObject;
    }

    [Rpc(SendTo.Server)]
    private void RequestHostTeamRpc()
    {
        rpcManager.RPCStarted();
        PokemonTeamInfo pokemonTeamInfo = new PokemonTeamInfo();
        Trainer player = NetworkManager.LocalClient.PlayerObject.gameObject.GetComponent<Trainer>();
        for (int j = 0; j < player.pokemonTeam.Length; j++)
        {
            if (j == 0)
            {
                pokemonTeamInfo.pokemon1 = player.pokemonTeam[j].GetSpeciesName();
            }
            else if (j == 1)
            {
                pokemonTeamInfo.pokemon2 = player.pokemonTeam[j].GetSpeciesName();
            }
            else if (j == 2)
            {
                pokemonTeamInfo.pokemon3 = player.pokemonTeam[j].GetSpeciesName();
            }
            else if (j == 3)
            {
                pokemonTeamInfo.pokemon4 = player.pokemonTeam[j].GetSpeciesName();
            }
            else if (j == 4)
            {
                pokemonTeamInfo.pokemon5 = player.pokemonTeam[j].GetSpeciesName();
            }
            else
            {
                pokemonTeamInfo.pokemon6 = player.pokemonTeam[j].GetSpeciesName();
            }
        }
        RecieveHostTeamRpc(pokemonTeamInfo);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void RecieveHostTeamRpc(PokemonTeamInfo pokemonTeamInfo)
    {
        if (IsHost)
        {
            return;
        }
        UpdateOpponentTeamOnClient(pokemonTeamInfo);
        rpcManager.RPCFinished();
        //GameManager.Instance.FinishRPCTaskRpc();
    }

    [Rpc(SendTo.Server)]
    private void SendUpdatedPositionToServerRpc(string oldPokemonName, Vector3 oldPokemonPosition, Vector3 newPokemonPosition)
    {
        GameObject oldPokemon = null!;
        foreach (Pokemon pokemon in pokemonTeam)
        {
            if (pokemon.GetSpeciesName() == oldPokemonName)
            {
                oldPokemon = pokemon.gameObject;
            }
        }

        Debug.Log($"oldPokemonPosition = {oldPokemonPosition}");
        Debug.Log($"newPokemonPosition = {newPokemonPosition}");
        oldPokemon.transform.position = oldPokemonPosition;
        activePokemonGameObject.transform.position = newPokemonPosition;
        activePokemonGameObject.transform.eulerAngles = gameObject.transform.eulerAngles;
        //while (oldPokemon.transform.position != oldPokemonPosition && activePokemonGameObject.transform.position != newPokemonPosition) ;
        Debug.Log($"oldPokemon = {oldPokemon.name}, {oldPokemon.transform.position}");
        Debug.Log($"activePokemon = {activePokemonGameObject.name}, {activePokemonGameObject.transform.position}");
        //SendUpdatedPositionToClientRpc(oldPokemonName, oldPokemonPosition, newPokemonPosition);
        GameManager.Instance.FinishRPCTaskRpc();
    }
    [Rpc(SendTo.ClientsAndHost)]
    private void SendUpdatedPositionToClientRpc(string oldPokemonName, Vector3 oldPokemonPosition, Vector3 newPokemonPosition)
    {

        GameObject oldPokemon = null!;
        foreach (Pokemon pokemon in pokemonTeam)
        {
            if (pokemon.GetSpeciesName() == oldPokemonName)
            {
                oldPokemon = pokemon.gameObject;
            }
        }
        oldPokemon.transform.position = oldPokemonPosition;
        activePokemonGameObject.transform.position = newPokemonPosition;
        activePokemonGameObject.transform.eulerAngles = gameObject.transform.eulerAngles;
        //while (oldPokemon.transform.position != oldPokemonPosition && activePokemonGameObject.transform.position != newPokemonPosition) ;
        Debug.Log($"oldPokemon = {oldPokemon.name}, {oldPokemon.transform.position}");
        Debug.Log($"activePokemon = {activePokemonGameObject.name}, {activePokemonGameObject.transform.position}");
        
    }

    private void UpdateOpponentTeamOnClient(PokemonTeamInfo pokemonTeamInfo)
    {
        Trainer opponent = GameObject.Find("Trainer(Clone)").GetComponent<Trainer>();
        Dictionary<string, Pokemon> pokemonNameInstanceDictionary = new();

        foreach (Pokemon pokemon in opponent.GetPokemonTeam())
        {
            pokemonNameInstanceDictionary.Add(pokemon.GetSpeciesName(), pokemon);
        }

        Pokemon[] randomizedTeam = new Pokemon[6];
        for (int i = 0; i < 6; i++)
        {
            if (i == 0)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon1.ToString()];
            }
            else if (i == 1)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon2.ToString()];
            }
            else if (i == 2)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon3.ToString()];
            }
            else if (i == 3)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon4.ToString()];
            }
            else if (i == 4)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon5.ToString()];
            }
            else
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon6.ToString()];
            }
        }
        Debug.Log("Setting the host's team on the client");
        opponent.SetPokemonTeam(randomizedTeam);
        GameObject[] newTeamObjects = new GameObject[6];
        for (int j = 0; j < opponent.GetPokemonTeam().Length; j++)
        {
            newTeamObjects[j] = opponent.GetPokemonTeam()[j].gameObject;
        }
        opponent.SetTeamObjects(newTeamObjects);
        opponent.SetActivePokemon(opponent.pokemonTeam[0]);
        opponent.SetActivePokemonGameObject(opponent.teamObjects[0]);
        opponent.gameObject.GetComponent<TrainerController>().SetOpponent(opponent);
    }

    [Rpc(SendTo.Server)]
    private void UpdateTeamOnServerRpc(PokemonTeamInfo pokemonTeamInfo)
    {
        rpcManager.RPCStarted();
        Debug.Log($"Name of the attached Object = {gameObject.name}");
        Dictionary<string, Pokemon> pokemonNameInstanceDictionary = new();

        foreach (Pokemon pokemon in pokemonTeam)
        {
            pokemonNameInstanceDictionary.Add(pokemon.GetSpeciesName(), pokemon);
        }

        Pokemon[] randomizedTeam = new Pokemon[pokemonNameInstanceDictionary.Count];
        for (int i = 0;  i < 6; i++)
        {
            if (i == 0)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon1.ToString()];
            }
            else if (i == 1)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon2.ToString()];
            }
            else if (i == 2)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon3.ToString()];
            }
            else if (i == 3)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon4.ToString()];
            }
            else if (i == 4)
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon5.ToString()];
            }
            else
            {
                randomizedTeam[i] = pokemonNameInstanceDictionary[pokemonTeamInfo.pokemon6.ToString()];
            }
        }
        int j = 0;
        Trainer opponent = GameObject.Find("Trainer(Clone)").GetComponent<Trainer>();
        opponent.SetPokemonTeam(randomizedTeam);
        GameObject[] newTeamObjects = new GameObject[6];
        foreach(Pokemon pokemon in opponent.pokemonTeam)
        {
            newTeamObjects[j] = pokemon.gameObject;
            j++;
        }
        opponent.SetTeamObjects(newTeamObjects);
        opponent.SetActivePokemon(opponent.GetPokemonTeam()[0]);
        opponent.SetActivePokemonGameObject(opponent.GetTeamObjects()[0]);
        opponent.gameObject.GetComponent<TrainerController>().SetOpponent(opponent);
        rpcManager.RPCFinished();
        //GameManager.Instance.FinishRPCTaskRpc();
    }

    [Rpc(SendTo.Server)]
    private void PlacePokemonRpc()
    {
        //rpcManager.RPCStarted();
        
        activePokemonGameObject.transform.position = gameObject.transform.position;
        activePokemonGameObject.transform.eulerAngles = gameObject.transform.eulerAngles;
        //gameObject.transform.eulerAngles = new Vector3(0f, 52f, 0f);
        //activePokemonGameObject.transform.Rotate(new Vector3(0, -127, 0));
        for (int i = 1; i < teamObjects.Length; i++)
        {
            teamObjects[i].transform.position = new Vector3(0, 50, 0);
        }
        PlacePokemonOnClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void PlacePokemonOnClientRpc()
    {
        //activePokemonGameObject.transform.Rotate(new Vector3(0, 52, 0));
        activePokemonGameObject = teamObjects[0];
        activePokemonGameObject.transform.position = gameObject.transform.position;
        activePokemonGameObject.transform.eulerAngles = gameObject.transform.eulerAngles;
        for (int i = 1; i < teamObjects.Length; i++)
        {
            teamObjects[i].transform.position = new Vector3(0, 50, 0);
        }
        //rpcManager.RPCFinished();
    }
}
