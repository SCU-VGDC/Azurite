using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public Action OnInteract;
    public float interactionDistance = 3;
    private Transform playerTransform;
    [SerializeField] private KeyCode triggerKey = KeyCode.E;

    public GameObject targetObject;
    public float interactionRange = 3f;

    private bool awaitingKeyUp;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameManager.inst.player.transform;
    }


    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(playerTransform.transform.position, transform.position);
        
        if (distance <= interactionRange)
        {
            if (Input.GetKeyDown(triggerKey))
            {
                if (awaitingKeyUp || distance > interactionDistance)
                    return;

                awaitingKeyUp = true;
                OnInteract?.Invoke();
            }
            else
                awaitingKeyUp = false;

            //Trigger interaction
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 look = Camera.main.transform.forward;

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, look, 100);

            if (hit.collider != null && hit.collider.gameObject == targetObject && Input.GetMouseButtonDown(0))
            {
                OnInteract?.Invoke();
            }
        }
    }
}
