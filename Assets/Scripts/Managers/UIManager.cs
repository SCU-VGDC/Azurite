using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    [SerializeField] private bool inventoryMenuActivated = false;

    void Start()
    {

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
        if (GameManager.inst.paused == false)
        {
            Debug.Log("Paused");
            Time.timeScale = 0f;
            
            GameManager.inst.paused = true;
        }

        else if (GameManager.inst.paused == true)
        {
            Debug.Log("Resume");
            Time.timeScale = 1;

            GameManager.inst.paused = false;
        }
    }
}
