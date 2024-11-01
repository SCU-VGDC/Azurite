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

    
    
    // Start is called before the first frame update
    void Start()
    {
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
                buttons[width * i + j].GetComponent<ButtonScript>().setUp(width * i + j, this.gameObject);

            }
        }
    }

    public void updateButtons(int bnum)
    {
        buttons[bnum].GetComponent<ButtonScript>().ToggleState();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
