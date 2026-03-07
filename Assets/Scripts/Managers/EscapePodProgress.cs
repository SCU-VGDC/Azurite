using Unity.VisualScripting;
using UnityEditor.U2D.Sprites;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;


public class EscapePodProgress : MonoBehaviour
{

    [SerializeField] private Item escapePart;
    [SerializeField] private List<GameObject> doorObjects = new List<GameObject>();
    [SerializeField] private GameObject door;
    
    private int EscapePodParts
    {
        get { return PersistentDataManager.Instance.Get<int>("collectedParts"); }
        set { PersistentDataManager.Instance.Set<int>("collectedParts", value); }
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
            print("parts used: " + EscapePodParts);
            UpdateEscapeDoor();
            return;
        }
        
        print("no escape part in inventory");
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
        //GameObject doorPart = new GameObject();
        //doorPart.transform.SetParent(transform);
        print("update: " + (EscapePodParts - 1));
        print("length " + doorObjects.Count);
        
        GameObject currentObject = doorObjects[EscapePodParts -1];
        if (currentObject != null)
        {
            currentObject.SetActive(true);
        }

        //doorPart.AddComponent<SpriteRenderer>().sprite = sprites[EscapePodParts - 1];
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        EscapePodParts = 0;
        LoadEscapeDoorProgress();
        print("list length " + doorObjects.Count);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
