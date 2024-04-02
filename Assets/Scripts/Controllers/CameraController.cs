using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] private float speed = 5f;
    private float distanceMultiplier = 1.5f;
    private void OnEnable()
    {
        EventsToTriggerManager.OnTriggerEvent += HandleEventTriggered;
    }

    private void HandleEventTriggered(EventsToTrigger e)
    {
        if (e == EventsToTrigger.YourPokemonSwitched && transform.parent.gameObject.name != "TitleScreenUI")
        {
            MoveToPosition();
        }
    }

    public void MoveToPosition()
    {
        AdjustFocalPoint();
        Transform activePokemonTransform = trainerController.GetPlayer().GetActivePokemonGameObject().transform;
        if (activePokemonTransform != null)
        {
            float objectSize = CalculateObjectSize(activePokemonTransform);
            float desiredDistance = objectSize * distanceMultiplier;

            // Calculate the desired position based on the target object and desired distance
            Vector3 offsetPosition = -activePokemonTransform.forward * desiredDistance + activePokemonTransform.right * desiredDistance;
            Vector3 desiredPosition = activePokemonTransform.position + offsetPosition;

            // Set the camera position
            //transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
            transform.position = desiredPosition;

            // Make the camera look at the target object
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
        GameObject focalPoint = GameObject.Find("Focal Point");
        if (trainerController == null)
        {
            Debug.Log(transform.parent.gameObject.name);
            Debug.Log("TrainerController is null");
        }
        float newY = (trainerController.GetPlayer().GetActivePokemonGameObject().transform.position.y + trainerController.GetOpponent().GetActivePokemonGameObject().transform.position.y) / 2f;
        focalPoint.transform.position = new Vector3(focalPoint.transform.position.x, newY, focalPoint.transform.position.z);
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
