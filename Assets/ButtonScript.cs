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

    Sprite Icon;

    // Start is called before the first frame update


    public void setUp(int bnum, GameObject screen, bool StartOn, bool Glass, ButtonSprites S)
    {
        buttonNumber = bnum;
        buttonScreen = screen;
        if (StartOn)
        {
            SetOn();
        }
        else
        {
            SetOff();
        }
        IsGlass = Glass;
        
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

    public enum ButtonSprites { 

        Default
    }
}
