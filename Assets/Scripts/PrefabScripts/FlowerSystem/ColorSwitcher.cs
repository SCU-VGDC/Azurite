using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.UI.Image;

public class ColorSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> Placements;

    [SerializeField] private InteractionTrigger interaction;
    private int colorState = 0;
    public List<GameObject> childObject;
    private readonly int maxLightColor = 3; //Number of different colors for the lights.
    // Start is called before the first frame update

    void Start()
    {
        RandomlyPlaceChildObjects(); //Randomly assigns the child objects to the placement game objects.
        interaction.OnInteract += ColorSwitch;
    }

    void RandomlyPlaceChildObjects()
    {
        List<int> availableIndices = new(); //List of available slots for a child object to be placed.
        for (int i = 0; i < childObject.Count; i++)
        {
            availableIndices.Add(i); //One available index for every child object.
        }

        for (int i = 0; i < childObject.Count; i++)
        {
            //This code randomly selects from the available indices, places a game object at placement, and removes that placement from the available indices.
            int randomIndex = UnityEngine.Random.Range(0, availableIndices.Count);
            int childIndex = availableIndices[randomIndex];
            availableIndices.RemoveAt(randomIndex);
            childObject[i].transform.position = Placements[childIndex].transform.position;
        }
    }

    void ChangeAll()
    {
        GetComponent<ColorStore>().ColorChange(colorState); //Changes switch color
        for (int i = 0; i < childObject.Count; i++)
        {
            if (childObject[i] != null)
            {
                childObject[i].GetComponent<ColorStore>().ColorChange(colorState); //Changes the color of every child object. (All child objects require color store)
            }
        }
    }
    
    void ColorSwitch() //Switches color of room light.
    {
        colorState++;
        colorState %= maxLightColor;
        ChangeAll(); //Switches color of children dependent on current lighting.
    }
}
