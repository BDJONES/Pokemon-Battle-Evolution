using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] private float speed = 5f;
    private float distanceMultiplier = 1f;
    EventsToTriggerManager eventsToTriggerManager;
    private void OnEnable()
    {
        //GameObject eventsToTriggerManagerGO = GameObject.Find("EventsToTriggerManager");
        NetworkCommands.UIControllerCreated += HandleUIControllerCreated;
    }

    private void HandleUIControllerCreated()
    {
        GameObject eventsToTriggerGO = GameObject.Find("EventsToTriggerManager");
        eventsToTriggerManager = eventsToTriggerGO.GetComponent<EventsToTriggerManager>();
        eventsToTriggerManager.OnTriggerEvent += HandleEventTriggered;
    }

    private void HandleEventTriggered(EventsToTrigger e)
    {
        if (this == null)
        {
            return;
        }
        if ((e == EventsToTrigger.YourPokemonSwitched || e == EventsToTrigger.OpposingPokemonSwitched) && transform.parent.gameObject.name != "TitleScreenUI")
        {
            Debug.Log("Attempting to move camera");
            if (transform.parent.gameObject.GetComponent<NetworkObject>().IsOwner)
            {
                Debug.Log("It's my turn to move the camera");
                MoveToPosition();
                Debug.Log("Finished moving the camera");
            }
            else
            {
                Debug.Log("Parent was not the owner");
            }
        }
    }

    public void MoveToPosition()
    {
        Debug.Log("Moving Camera");
        trainerController = transform.parent.gameObject.GetComponent<TrainerController>();
        //trainerController.SetOpponent(GameObject.Find("Trainer(Clone)").GetComponent<Trainer>());
        Transform activePokemonTransform = trainerController.GetPlayer().GetActivePokemonGameObject().transform;
        Debug.Log($"Active Pokemon's position = {activePokemonTransform.position}");
        if (activePokemonTransform != null)
        {
            float objectSize = CalculateObjectSize(activePokemonTransform);
            float desiredDistance = (objectSize * distanceMultiplier);
            if (desiredDistance > 4)
            {
                desiredDistance /= 2;
            }
            Debug.Log($"Desired Distance = {desiredDistance}");
            Debug.Log($"Current Position = {transform.position}");
            // Calculate the desired position based on the target object and desired distance
            Vector3 offsetPosition = -activePokemonTransform.forward * desiredDistance + activePokemonTransform.right * desiredDistance + activePokemonTransform.up * activePokemonTransform.gameObject.GetComponent<Collider>().bounds.size.y;
            Vector3 desiredPosition = activePokemonTransform.position + offsetPosition;
            Debug.Log($"offsetPosition = {offsetPosition}");
            // Set the camera position
            //transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
            transform.position = desiredPosition;
            Debug.Log($"New Position = {transform.position}");
            // Make the camera look at the target object
            AdjustFocalPoint();
            GameObject focalPoint = GameObject.Find("Focal Point");
            transform.LookAt(focalPoint.transform);
        }
        else
        {
            Debug.Log("Unable to get the game object. It was null");
        }
        //float newX = 1.0010611935763951383799303384732f * trainerController.GetPlayer().GetActivePokemon().gameObject.transform.position.x;
        //float newY = trainerController.GetPlayer().GetActivePokemon().gameObject.transform.position.y;
        //float newZ = 0.98881367717131229677498517352093f * trainerController.GetPlayer().GetActivePokemon().gameObject.transform.position.z;
        //Vector3 newPosition = new Vector3(newX, newY, newZ);
        //Debug.Log($"newX = {newX}, newY = {newY}, newZ = {newZ}");
        //gameObject.transform.position = newPosition;//* Time.deltaTime * speed
    }

    private void AdjustFocalPoint()
    {
        if (trainerController == null)
        {
            Debug.Log(transform.parent.gameObject.name);
            Debug.Log("TrainerController is null");
            return;
        }
        GameObject focalPoint = GameObject.Find("Focal Point");
        var playerBounds = trainerController.GetPlayer().GetActivePokemonGameObject().GetComponent<Collider>().bounds;
        var opponentBounds = trainerController.GetOpponent().GetActivePokemonGameObject().GetComponent<Collider>().bounds;
        float newX = (trainerController.GetPlayer().gameObject.transform.position.x + trainerController.GetOpponent().gameObject.transform.position.x) / 2f;
        float newY;
        Debug.Log($"playerBounds size = {playerBounds.size.y}, opponentBounds size {opponentBounds.size.y}");
        if (playerBounds.size.y > opponentBounds.size.y)
        {
            Debug.Log("Player is bigger");
            newY = (playerBounds.size.y) / 2f;
        }
        else if (playerBounds.size.y < opponentBounds.size.y && opponentBounds.size.y < 1.7f * playerBounds.size.y)
        {
            Debug.Log("Opponent is bigger, but not by much");
            newY = (opponentBounds.size.y) / 2f;
        }
        else
        {
            Debug.Log("The opponent is bigger by a wide margin");
            newY = (opponentBounds.size.y) / 4f;            
        }
        float newZ = (trainerController.GetPlayer().gameObject.transform.position.z + trainerController.GetOpponent().gameObject.transform.position.z) / 2f;
        focalPoint.transform.position = new Vector3(newX, newY, newZ);
    }

    float CalculateObjectSize(Transform obj)
    {
        // Calculate the size of the object along its forward axis
        Bounds bounds = CalculateObjectBounds(obj);
        return bounds.size.z;
    }

    Bounds CalculateObjectBounds(Transform obj)
    {
        // Calculate the bounding box of the object
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds;
        }
        else
        {
            // If renderer is not found, use collider bounds
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
            {
                return collider.bounds;
            }
            else
            {
                // Default bounds if neither renderer nor collider is found
                return new Bounds(obj.position, Vector3.one);
            }
        }
    }
}
