using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    bool IsGlass;
    bool IsOn;
    int buttonNumber;
    GameObject buttonScreen;
    
    
    // Start is called before the first frame update


    public void setUp(int bnum, GameObject screen, ButtonScreenScript.ButtonState bs)
    {
        buttonNumber = bnum;
        buttonScreen = screen;
        switch(bs)
        {
            case ButtonScreenScript.ButtonState.OffNoGlass:
                SetOff();
                IsGlass= false;
                break;
            case ButtonScreenScript.ButtonState.OffWithGlass:
                SetOff();
                IsGlass= true;
                break;
            case ButtonScreenScript.ButtonState.OnNoGlass:
                SetOn();
                IsGlass= false;
                break;
            case ButtonScreenScript.ButtonState.OnWithGlass: 
                SetOn();
                IsGlass= true;
                break;
        }
    }

    public void Clicked()
    {
        if(!IsGlass)
        {
            buttonScreen.GetComponent<ButtonScreenScript>().updateButtons(buttonNumber);

        }
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
    public bool CheckStatus()
    {
        return IsOn;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
