using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool paused;
    public GameObject InventoryMenu;
    [SerializeField] private bool inventoryMenuActivated = false;

    void Start()
    {
        paused = false;
    }

    void Update()
    {
        if(Input.GetButtonDown("Inventory") && inventoryMenuActivated) {
            InventoryMenu.SetActive(false);

            inventoryMenuActivated = false;
        } else if(Input.GetButtonDown("Inventory") && !inventoryMenuActivated) {
            InventoryMenu.SetActive(true);
            InventoryMenu.GetComponent<InventoryScreen>().UpdateSlots();

            inventoryMenuActivated = true;
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
