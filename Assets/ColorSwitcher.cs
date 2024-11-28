using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ColorSwitcher : MonoBehaviour
{

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
    Color[] redLightColors = new Color[] { Color.black, Color.black, Color.red, Color.red, Color.red, Color.red, Color.red, Color.red };
    Color[] greenLightColors = new Color[] { Color.blue, Color.blue, Color.green, Color.green, Color.green, Color.green, Color.green, Color.green };
    Color[] yellowLightColors = new Color[] { Color.red, Color.red, Color.yellow, Color.yellow, Color.yellow, Color.yellow, Color.yellow, Color.yellow };
    // Start is called before the first frame update

    void Start()
    {
        Transform[] childTransforms = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in childTransforms)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
                childObject.Add(child.gameObject);
        }
        colorRenderer = GetComponentInChildren<SpriteRenderer>();
        interaction.OnInteract += ColorSwitch;
    }

    void GreenLight()
    {
        colorRenderer.material.color = Color.green;
        for (int i = 1; i < childObject.Count; i++)
        {
            childObject[i].GetComponent<SpriteRenderer>().material.color = greenLightColors[i];
        }
        /*Growth.GetComponent<SpriteRenderer>().material.color = Color.green;
        Rigid.GetComponent<SpriteRenderer>().material.color = Color.green;
        Sticky.GetComponent<SpriteRenderer>().material.color = Color.green;
        Slippery.GetComponent<SpriteRenderer>().material.color = Color.green;
        Combustible.GetComponent<SpriteRenderer>().material.color = Color.green;
        Corrosive.GetComponent<SpriteRenderer>().material.color = Color.green;
        Pretty.GetComponent<SpriteRenderer>().material.color = Color.green;*/
    }
    void RedLight()
    {
        colorRenderer.material.color = Color.red;
        for (int i = 1; i < childObject.Count; i++)
        {
            childObject[i].GetComponent<SpriteRenderer>().material.color = redLightColors[i];
        }
        /*Growth.GetComponent<SpriteRenderer>().material.color = Color.red;
        Rigid.GetComponent<SpriteRenderer>().material.color = Color.red;
        Sticky.GetComponent<SpriteRenderer>().material.color = Color.red;
        Slippery.GetComponent<SpriteRenderer>().material.color = Color.red;
        Combustible.GetComponent<SpriteRenderer>().material.color = Color.red;
        Corrosive.GetComponent<SpriteRenderer>().material.color = Color.red;
        Pretty.GetComponent<SpriteRenderer>().material.color = Color.red;*/
    }
    void YellowLight()
    {
        colorRenderer.material.color = Color.yellow;
        for (int i = 1; i < childObject.Count; i++)
        {
            childObject[i].GetComponent<SpriteRenderer>().material.color = yellowLightColors[i];
        }
        /*Growth.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        Rigid.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        Sticky.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        Slippery.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        Combustible.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        Corrosive.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        Pretty.GetComponent<SpriteRenderer>().material.color = Color.yellow;*/
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
                GreenLight();
                break;
            case 1:
                YellowLight();
                break;
            case 2:
                RedLight();
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
