using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour
{
    public Action OnInteract;
    private Transform playerTransform;
    [SerializeField] private KeyCode triggerKey = KeyCode.E;
    public float interactionRange = 3f;

    public static HashSet<InteractionTrigger> interactSet = new();

    private bool awaitingKeyUp;
    private float distance;

    [SerializeField] public GameObject textPopUp;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameManager.inst.player.transform;

        ToggleTextPopup(false);
        textPopUp.GetComponent<TextMesh>().text = triggerKey.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(playerTransform.transform.position, transform.position);
        
        if (distance <= interactionRange)
        {   
            interactSet.Add(this);
            
            foreach (InteractionTrigger intTrig in interactSet) {
                if (this.distance > intTrig.GetDistance())
                {
                    ToggleTextPopup(false);
                    return;
                }
            }
            
            ToggleTextPopup(true);

            if (Input.GetKeyDown(triggerKey))
            {
                if (awaitingKeyUp || distance > interactionRange)
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

            if (hit.collider != null && hit.collider.gameObject == GetComponent<Collider2D>() && Input.GetMouseButtonDown(0))
            {
                OnInteract?.Invoke();
            }
        } else // if player is out of item's range
        {
            interactSet.Remove(this);  // rmv from hash of possible interactable objects
            ToggleTextPopup(false);
        }
    }

    void ToggleTextPopup(bool value)
    {
        textPopUp.gameObject.SetActive(value);
    }

    float GetDistance() {
        return distance;
    }
}
