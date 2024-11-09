using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TMPro;

public class InteractionTrigger : MonoBehaviour
{
    public delegate void InteractTriggerHandler();
    public event InteractTriggerHandler OnInteract;
    public float interactionDistance = 3;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private KeyCode triggerKey = KeyCode.E;
    public TextMeshProUGUI Interact;

    private bool awaitingKeyUp;

    // Start is called before the first frame update
    void Start()
    {
        Interact.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(playerTransform.position, transform.position) < interactionDistance)
        {
            Interact.enabled = true;

        }
        else
        {
            Interact.enabled = false;
        }
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
