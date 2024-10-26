using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }


    public void PauseGame()
    {
        if (paused == false)
        {
            Debug.Log("Paused");
            Time.timeScale = 0f;
            paused = true;
        }

        else if (paused == true)
        {
            Debug.Log("Resume");
            Time.timeScale = 1;
            paused = false;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
   

}
