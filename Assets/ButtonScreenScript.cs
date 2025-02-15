using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScreenScript : MonoBehaviour
{
    
    int width, height;
    //int WidthOffset = 100;
    //int HeightOffset = 200;
    GameObject intObect;
    [SerializeField] GameObject buttonPrefab;
    GameObject[] buttons;
    [SerializeField] bool toggleNearby;
    [SerializeField] bool SolutionIsAllOn;

    [SerializeField] bool LoadPuzzleFromPuzzles;
    [SerializeField] Puzzles puzzle;
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

    bool[] Solution;

    // Start is called before the first frame update
    void Start()
    {
        if (LoadPuzzleFromPuzzles)
        {
            loadPuzzle(puzzle);
            
            return;
        }
        /*
        int buttonCounter = 0;

        buttons = new GameObject[width * height];

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
        */
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
            bool buttonState = buttons[i].GetComponent<ButtonScript>().CheckStatus(); 
            if (buttonState != Solution[i % Solution.Length])
            {
                return false;
            }

        }


        return true;
    }



    public bool TestSolution()
    {
        if (SolutionIsAllOn)
        {
            if (isAllOn())
            {

                Debug.Log("Correct");
                
                return true;
            }
            else
            {

                Debug.Log("Wrong");
                return false;
            }


        }
        else
        {
            if (CheckSolution())
            {
                Debug.Log("Correct");
                //intObect.GetComponent<InteractableObject>().Correct();
                return true;

            }
            else
            {

                Debug.Log("Wrong");
                return false;
            }

        }

    }
    
    public void SetInteractable(GameObject inter)
    {
        intObect = inter;
    }


    public void loadPuzzle(Puzzles p)
    {
        int buttonCounter = 0;
        Solution = new bool[p.ButtonCount];
        height = p.height;
        width= p.width;
        buttons = new GameObject[p.ButtonCount];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++) 
            {



                Vector3 buttonPos = this.transform.position;
                buttonPos.x += (50 * j) * (10 / height) - (25 * width) * (10 / height);
                buttonPos.y -= (50 * i) * (10 / height) - (25 * height) * (10 / height); 
                Debug.Log("Set up button number" + (width * i + j));
                buttons[width * i + j] = Instantiate(buttonPrefab, buttonPos, this.transform.rotation, this.transform);
                buttons[width * i + j].GetComponent<ButtonScript>().setUp(width * i + j, this.gameObject, p.StartOn[width * i + j], p.IsLocked[width * i + j], p.ButtonIcons[width * i + j]);
                buttons[width * i + j].gameObject.transform.localScale= Vector3.one * (10 / height);
                Solution[buttonCounter] = p.SolutionIsOn[buttonCounter];

                buttonCounter++;

            }
        }
        toggleNearby = p.ToggleNearby;
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
