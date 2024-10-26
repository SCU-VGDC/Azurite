using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool paused;
    public GameObject InventoryMenu;
    private bool menuActivated = false;
    [SerializeField] private KeyCode triggerKey = KeyCode.I;

    void Start()
    {
        paused = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(triggerKey) && menuActivated) {
            InventoryMenu.SetActive(false);
            menuActivated = false;
        } else if(Input.GetKeyDown(triggerKey) && !menuActivated) {
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
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
}
