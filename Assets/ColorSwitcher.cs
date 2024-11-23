using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitcher : MonoBehaviour
{

    GameObject Growth;// = GameObject.Find("Growth");
    GameObject Rigid;// = GameObject.Find("Rigid");
    GameObject Sticky;// = GameObject.Find("Sticky");
    GameObject Slippery;// = GameObject.Find("Slippery");
    GameObject Combustible;// = GameObject.Find("Combustible");
    GameObject Corrosive;// = GameObject.Find("Corrosive");
    GameObject Pretty;// = GameObject.Find("Pretty");
    [SerializeField] private InteractionTrigger interaction;
    int colorState;
    // Start is called before the first frame update

    void Start()
    {
        interaction.OnInteract += ColorSwitch;
    }

    void GreenLight()
    {
        GetComponentInChildren<SpriteRenderer>().material.color = Color.green;
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
        GetComponentInChildren<SpriteRenderer>().material.color = Color.red;
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
        GetComponentInChildren<SpriteRenderer>().material.color = Color.yellow;
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
        colorState = colorState++ % 3;
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
