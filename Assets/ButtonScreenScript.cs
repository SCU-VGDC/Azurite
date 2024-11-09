using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreenScript : MonoBehaviour
{
    
    [SerializeField] int width, height;
    int WidthOffset = 100;
    int HeightOffset = 100;
    [SerializeField] GameObject buttonPrefab;
    GameObject[] buttons;
    [SerializeField] bool toggleNearby;
    [SerializeField] bool SolutionIsAllOn;
    public enum ButtonState
    {
        OffNoGlass,
        OnNoGlass,
        OffWithGlass,
        OnWithGlass,
    }
    
    public enum SolutionState
    {
        Off,
        On,
    }

    [SerializeField] ButtonState[] LoadButtons;
    [SerializeField] SolutionState[] Solution;

    // Start is called before the first frame update
    void Start()
    {
        int buttonCounter = 0;
        buttons= new GameObject[width * height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++) 
            {
                Vector3 buttonPos = this.transform.position;
                buttonPos.x += (75 * j) - WidthOffset;
                buttonPos.y -= (75 * i) - HeightOffset;
                Debug.Log("Set up button number" + (width * i + j));
                buttons[width * i + j] = Instantiate(buttonPrefab, buttonPos, this.transform.rotation ,this.transform);
                buttons[width * i + j].GetComponent<ButtonScript>().setUp(width * i + j, this.gameObject, LoadButtons[buttonCounter % LoadButtons.Length]);
                buttonCounter++;

            }
        }
    }   

    public void updateButtons(int bnum)
    {
        buttons[bnum].GetComponent<ButtonScript>().ToggleState();
        if (toggleNearby)
        {


            if (bnum + width < width * height)
            {
                buttons[bnum+ width].GetComponent<ButtonScript>().ToggleState();
            }
            if (bnum - width >= 0)
            {
                buttons[bnum - width].GetComponent<ButtonScript>().ToggleState();
            }
            if ((bnum % width) - 1 >= 0){
                buttons[bnum - 1].GetComponent<ButtonScript>().ToggleState();
            }
            if ((bnum % width) + 1 < width){
                buttons[bnum + 1].GetComponent<ButtonScript>().ToggleState();


            }
            
        }
    }

    public bool isAllOn()
    {
        
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].GetComponent<ButtonScript>().CheckStatus())
            {
                return false;
            }

        }
        return true;
    }

    public bool CheckSolution()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            SolutionState button; //Solution[i % Solution.Length];
            if (buttons[i].GetComponent<ButtonScript>().CheckStatus())
            {
                button = SolutionState.On;
            }
            else
            {
                button = SolutionState.Off;
            }
            if (button != Solution[i % Solution.Length])
            {
                return false;
            }

        }


        return true;
    }

    public void TestSolution()
    {
        if (SolutionIsAllOn)
        {
            if (isAllOn())
            {
                Debug.Log("Correct");
            }
            else
            {
                Debug.Log("Wrong");
            }
            return;
        }
        else
        {
            if (CheckSolution())
            {
                Debug.Log("Correct");

            }
            Debug.Log("Wrong");

        }

    }


    public void OpenScreen()
    {
        this.gameObject.SetActive(true);
    }
    public void CloseScreen()
    {
        this.gameObject.SetActive(false);
    }
}
