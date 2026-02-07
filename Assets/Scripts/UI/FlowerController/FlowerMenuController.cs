using UnityEngine;
using UnityEngine.Events;

public class FlowerMenuController : MonoBehaviour
{
    [Tooltip("The menu prefab to instantiate. If not set, opens the player's inventory menu instead.")]
    [SerializeField] private FlowerMenu menuPrefab = null;

    [Tooltip("The inventory to display in the flower menu. If not set, uses the player's inventory.")]
    [SerializeField] private Inventory inventoryToShow = null;

    // No longer need to store the InteractionTrigger reference
    // We'll call OpenMenu() directly from the InteractionTrigger script

    [Tooltip("This event is called whenever the menu is opened.")]
    public UnityEvent<FlowerMenuController> onMenuOpen = new UnityEvent<FlowerMenuController>();

    [Tooltip("This event is called whenever the menu is closed.")]
    public UnityEvent<FlowerMenuController> onMenuClose = new UnityEvent<FlowerMenuController>();

    // Removed Start() method - we'll call OpenMenu() directly

    public bool IsMenuOpen()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

        if (canvas == null)
        {
            Debug.Log("Failed to find the main canvas!");
            return false;
        }

        return canvas.transform.GetComponentInChildren<FlowerMenu>() != null;
    }

    public FlowerMenu GetOpenMenu()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

        if (canvas == null)
        {
            Debug.Log("Failed to find the main canvas!");
            return null;
        }

        return canvas.transform.GetComponentInChildren<FlowerMenu>();
    }

    public void OpenMenu()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Main Canvas");

        if (canvas == null || canvas.transform.GetComponentInChildren<MenuBase>() != null)
        {
            Debug.Log("A menu is already open!");
            return;
        }

        Inventory inventory = this.inventoryToShow != null
            ? this.inventoryToShow
            : (GameManager.inst != null && GameManager.inst.player != null ? GameManager.inst.player.Inventory : null);

        if (inventory == null)
        {
            Debug.LogError("No inventory to show: assign one in the inspector or ensure GameManager and player exist.");
            return;
        }

        if (this.menuPrefab == null)
        {
            // Default to player's inventory menu (same as pressing I).
            if (GameManager.inst != null && GameManager.inst.player != null && GameManager.inst.player.Inventory != null)
            {
                GameManager.inst.player.Inventory.OpenMenu();
                this.onMenuOpen.Invoke(this);
            }
            else
            {
                Debug.LogError("No menu prefab assigned and no player inventory available.");
            }
            return;
        }

        Instantiate(this.menuPrefab, canvas.transform).Init(inventory);
        this.onMenuOpen.Invoke(this);
    }

    /// <summary>
    /// Closes the flower menu if it is open. Call this from code or wire to a button's onClick.
    /// </summary>
    public void CloseMenu()
    {
        FlowerMenu openMenu = this.GetOpenMenu();
        if (openMenu != null)
        {
            openMenu.Close();
            this.onMenuClose.Invoke(this);
        }
    }
}