using UnityEngine;
using UnityEngine.Events;

public class FlowerMenuController : MonoBehaviour
{
    [Tooltip(
        "The menu prefab to instantiate."
    )]
    [SerializeField]
    private FlowerMenu menuPrefab = null;

    [Tooltip(
        "The inventory to display in the flower menu."
    )]
    [SerializeField]
    private Inventory inventoryToShow = null;

    [Tooltip("The flower combiner inventory. If set, Space in the menu transfers the selected item here.")]
    [SerializeField]
    private FlowerInventory flowerInventory = null;

    [Tooltip("This event is called whenever the menu is opened.")]
    public UnityEvent<FlowerMenuController> onMenuOpen = new UnityEvent<FlowerMenuController>();

    [Tooltip("This event is called whenever the menu is closed.")]
    public UnityEvent<FlowerMenuController> onMenuClose = new UnityEvent<FlowerMenuController>();

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
            Debug.Log("A menu is already open");
            return;
        }

        Inventory inventory =
            this.inventoryToShow != null
                ? this.inventoryToShow
                : (
                    GameManager.inst != null && GameManager.inst.player != null
                        ? GameManager.inst.player.Inventory
                        : null
                );

        if (inventory == null)
        {
            Debug.LogError(
                "No inventory to show: assign one in the inspector or ensure GameManager and player exist."
            );
            return;
        }

        if (this.menuPrefab == null)
        {
            if (
                GameManager.inst != null
                && GameManager.inst.player != null
                && GameManager.inst.player.Inventory != null
            )
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

        string playerInvLog = "Player Inventory: ";
        foreach (var item in inventory.GetItems())
            playerInvLog += $"{item.GetDisplayName()} (x{inventory.GetCount(item)}), ";
        Debug.Log(playerInvLog);

        if (this.flowerInventory != null)
        {
            string combinerLog = "Combiner Inventory: ";
            if (this.flowerInventory.Slot1 != null) combinerLog += $"{this.flowerInventory.Slot1.GetDisplayName()} (x1), ";
            if (this.flowerInventory.Slot2 != null) combinerLog += $"{this.flowerInventory.Slot2.GetDisplayName()} (x1), ";
            Debug.Log(combinerLog);
        }
        else
        {
            Debug.LogWarning("Combiner Inventory is NULL on the controller!");
        }

        Instantiate(this.menuPrefab, canvas.transform).Init(inventory, this.flowerInventory);
        this.onMenuOpen.Invoke(this);
    }

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
