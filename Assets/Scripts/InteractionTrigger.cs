using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public delegate void InteractTriggerHandler();
    public event InteractTriggerHandler OnInteract;
    public float interactionDistance = 3;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private KeyCode triggerKey = KeyCode.E;
    //[SerializeField] private Collider2D collider;

    //Stuff I added +++
    public GameObject targetObject;
    public float interactionRange = 3f;

    private bool awaitingKeyUp;

    Renderer ren;


    // Start is called before the first frame update
    void Start()
    {
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

        //Updates I made to this branch


        float distance = Vector3.Distance(playerTransform.transform.position, transform.position);

        if (distance <= interactionRange)
        {
            ren=GetComponent<Renderer>();
            ren.material.color=Color.yellow;

            Debug.Log("Player is within range of the interactable object.");
            //Trigger interaction
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 look = Camera.main.transform.forward;

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, look, 100);
            if (hit.collider != null && hit.collider.gameObject == targetObject)
            {
                Debug.Log("Interaction pressed");
            }

        }

        else
        {
            ren=GetComponent<Renderer>();
            ren.material.color=Color.white;
        }

    }
}


