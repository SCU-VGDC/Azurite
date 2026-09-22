using UnityEngine;
using UnityEngine.Events;

public class FlowerMenuInteraction : MonoBehaviour
{
    [SerializeField]
    private FlowerMenu menuPrefab = null;

    [Tooltip("The flower combiner inventory. If set, Space in the menu transfers the selected item here.")]
    [SerializeField]
    private FlowerInventory flowerInventory = null;

    public void OpenMenu()
    {
        InventoryMenu inv = UIManager.Instance.Inventory;

        var flowerMenu = Instantiate(menuPrefab, inv.transform);
        flowerMenu.Init(flowerInventory);
        flowerMenu.Open();
    }

    public void CloseMenu()
    {
        FlowerMenu openMenu = UIManager.Instance.ScreenCanvas.GetComponentInChildren<FlowerMenu>();
        if (openMenu != null)
        {
            openMenu.Close();
        }
    }
}
