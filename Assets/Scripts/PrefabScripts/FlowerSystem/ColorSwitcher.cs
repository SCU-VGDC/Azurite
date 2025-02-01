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

    GameObject Growth;// = GameObject.Find("Growth");
    GameObject Rigid;// = GameObject.Find("Rigid");
    GameObject Sticky;// = GameObject.Find("Sticky");
    GameObject Slippery;// = GameObject.Find("Slippery");
    GameObject Combustible;// = GameObject.Find("Combustible");
    GameObject Corrosive;// = GameObject.Find("Corrosive");
    GameObject Pretty;// = GameObject.Find("Pretty");
    SpriteRenderer colorRenderer;
    [SerializeField] private InteractionTrigger interaction;
    int colorState = 0;
    public List<GameObject> childObject;
    Color[] redLightColors = new Color[] { Color.red, Color.black, Color.black, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red };
    Color[] greenLightColors = new Color[] { Color.green, Color.blue, Color.blue, Color.green, Color.green, Color.green, Color.green, Color.green, Color.green };
    Color[] yellowLightColors = new Color[] { Color.yellow, Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.yellow, Color.yellow, Color.yellow };
    // Start is called before the first frame update

    void Start()
    {
        colorRenderer = GetComponentInChildren<SpriteRenderer>();
        RandomlyPlaceChildObjects();

        interaction.OnInteract += ColorSwitch;
    }

    void RandomlyPlaceChildObjects()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < childObject.Count; i++)
        {
            availableIndices.Add(i);
            Debug.Log("Added: " + i);
        }

        for (int i = 0; i < childObject.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableIndices.Count);
            Debug.Log("Random Index at " + randomIndex);
            int childIndex = availableIndices[randomIndex];
            Debug.Log("Random Index of " + childIndex);
            availableIndices.RemoveAt(randomIndex);
            childObject[i].transform.position = Placements[childIndex].transform.position;
            Debug.Log("Placing Child Object " + i + " At Placement " +  childIndex);
        }
    }

    void GreenLight()
    {
        colorRenderer.material.color = Color.green;
        for (int i = 1; i < childObject.Count; i++)
        {
            if (childObject[i] != null)
            {
                childObject[i].GetComponent<SpriteRenderer>().material.color = greenLightColors[i];
            }
        }
    }
    void RedLight()
    {
        colorRenderer.material.color = Color.red;
        for (int i = 1; i < childObject.Count; i++)
        {
            if (childObject[i] != null) 
            { 
                childObject[i].GetComponent<SpriteRenderer>().material.color = redLightColors[i]; 
            }
        }
    }
    void YellowLight()
    {
        colorRenderer.material.color = Color.yellow;
        for (int i = 1; i < childObject.Count; i++)
        {
            if (childObject[i] != null)
            {
                childObject[i].GetComponent<SpriteRenderer>().material.color = yellowLightColors[i];
            }
        }
    }

    void ChangeAll(Color[] color)
    {
        GetComponent<ColorStore>().ColorChange(colorState);
        for (int i = 0; i < childObject.Count; i++)
        {
            if (childObject[i] != null)
            {
                childObject[i].GetComponent<ColorStore>().ColorChange(colorState);
                //childObject[i].GetComponent<SpriteRenderer>().material.color = color[i];
            }
        }
    }
    
    void ColorSwitch()
    {
        colorState++;
        colorState = colorState % 3;
        Debug.Log("Color Switch");
        Debug.Log(colorState);
        switch (colorState)
        {
            case 0:
                ChangeAll(greenLightColors);
                break;
            case 1:
                ChangeAll(yellowLightColors);
                break;
            case 2:
                ChangeAll(redLightColors);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
