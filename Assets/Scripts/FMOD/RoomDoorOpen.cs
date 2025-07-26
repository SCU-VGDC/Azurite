using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoorOpen : MonoBehaviour
{
    [Header("FMOD Settings")]
    public FMODUnity.EventReference doorOpenEvent;

    [Header("Interaction Settings")]
    public float interactionRadius = 2.5f; // distance in units, may need to adjust in post
    private Transform player; 

    void Start()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
            {
                player = found.transform;
            }
            else
            {
                Debug.LogWarning("Player not found! Make sure the 'Player' tag exists in the scene.");
            }

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerWithinRadius())
        {
            FMODUnity.RuntimeManager.PlayOneShot(doorOpenEvent);
        }
    }

    bool IsPlayerWithinRadius()
    {
        return player != null && Vector2.Distance(transform.position, player.position) <= interactionRadius;
    }
}