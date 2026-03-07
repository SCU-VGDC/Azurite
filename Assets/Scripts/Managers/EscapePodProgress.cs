using UnityEngine;
using System.Collections.Generic;

public class EscapePodProgress : MonoBehaviour
{
    [SerializeField] private Item escapePart;
    [SerializeField] private List<GameObject> doorObjects = new List<GameObject>();
    [SerializeField] private GameObject door;
    
    private int EscapePodParts
    {
        get => PersistentDataManager.Instance.Get<int>("collectedParts");
        set => PersistentDataManager.Instance.Set<int>("collectedParts", value);
    }

    public void EscapePartUsed(Player player)
    {
        // get the player instance from the interaction trigger!?
        // call inventory from a player instance

        if (player.Inventory.HasItem(escapePart)) // replace collectedParts with the part in the inventory
        {
            // uhhhh, adding one more part to the current amount of parts we have
            EscapePodParts++;
            player.Inventory.RemoveItem(escapePart, 1);
            UpdateEscapeDoor();
        }
    }
    
    public void LoadEscapeDoorProgress()
    {
        for (int i = 0; i < EscapePodParts; i++)
        {
            //GameObject doorPart = new GameObject();
            //doorPart.transform.SetParent(transform);
            //doorPart.AddComponent<SpriteRenderer>().sprite = sprites[i];

            GameObject currentObject = doorObjects[i];
            if (currentObject != null)
            {
                currentObject.SetActive(true);
            }
        }
    }

    private void UpdateEscapeDoor()
    {        
        GameObject currentObject = doorObjects[EscapePodParts - 1];
        if (currentObject != null)
        {
            currentObject.SetActive(true);
        }
    }
    
    void Start()
    { 
        EscapePodParts = 0;
        LoadEscapeDoorProgress();
    }
}
