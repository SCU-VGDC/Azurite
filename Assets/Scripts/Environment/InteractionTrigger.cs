using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public delegate void InteractTriggerHandler();
    public event InteractTriggerHandler OnInteract;
    public float interactionDistance = 3;
    private Transform playerTransform;
    [SerializeField] private KeyCode triggerKey = KeyCode.E;

    private bool awaitingKeyUp;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameManager.inst.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (awaitingKeyUp || Vector3.Distance(playerTransform.position, transform.position) > interactionDistance)
                return;
            awaitingKeyUp = true;
            OnInteract?.Invoke();
        }
        else
            awaitingKeyUp = false;
    }
}
