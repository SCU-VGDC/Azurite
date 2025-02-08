using UnityEngine;

public class UIManager : MonoBehaviour
{
    public InventoryScreen InventoryMenu;
    [SerializeField] private bool inventoryMenuActivated = false;

    void Update()
    {
        if (Input.GetButtonDown("Inventory") && inventoryMenuActivated) {
            InventoryMenu.gameObject.SetActive(false);
            inventoryMenuActivated = false;
        } else if (Input.GetButtonDown("Inventory") && !inventoryMenuActivated) {
            InventoryMenu.gameObject.SetActive(true);
            InventoryMenu.UpdateSlots();
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
