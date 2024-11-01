using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

    bool IsOn;
    int buttonNumber;
    GameObject buttonScreen;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetOff();
    }

    public void setUp(int bnum, GameObject screen)
    {
        buttonNumber = bnum;
        buttonScreen = screen;
    }

    public void Clicked()
    {
        buttonScreen.GetComponent<ButtonScreenScript>().updateButtons(buttonNumber);
    }


    public void ToggleState() {

        if (IsOn)
        {
            SetOff();
        }
        else
        {
            SetOn();
        }
    
    }

    public void SetOn()
    {
        this.gameObject.GetComponent<Image>().color = Color.blue;
        IsOn= true;

    }
    public void SetOff()
    {
        this.gameObject.GetComponent<Image>().color = Color.black;
        IsOn= false;

    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
