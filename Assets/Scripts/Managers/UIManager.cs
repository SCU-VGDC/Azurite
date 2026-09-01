using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [field: SerializeField] public Menu FullscreenMenuContainer { get; private set; }
    [SerializeField] private DialogMenu dialogMenuPrefab;
    [SerializeField] private Menu notePopupPrefab;

    private readonly HashSet<Menu> openMenus = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = GameManager.Instance.MainCamera;
    }

    public DialogMenu CreateDialog(Dialog dialog)
    {
        var child = FullscreenMenuContainer.GetComponentInChildren<DialogMenu>();
        if (child != null)
            Destroy(child.gameObject);

        var dialogMenu = Instantiate(dialogMenuPrefab, FullscreenMenuContainer.transform);
        dialogMenu.canBeClosedBySiblings = dialog.allowEarlyExit;
        dialogMenu.Init(dialog);
        return dialogMenu;
    }

    public Menu CreateNotePopup(string text)
    {
        var notePopup = Instantiate(notePopupPrefab, FullscreenMenuContainer.transform).GetComponent<Menu>();
        notePopup.GetComponentInChildren<TextMeshProUGUI>().text = text;
        notePopup.Open();

        return notePopup;
    }

    private void CheckMenuRestrictingControls()
    {
        if (openMenus.Any(menu => menu.restrictPlayerActions))
            GameManager.Instance.Player.Freeze("UIManager");
        else
            GameManager.Instance.Player.Unfreeze("UIManager");
    }

    public void OnMenuOpened(Menu menu)
    {
        openMenus.Add(menu);
        CheckMenuRestrictingControls();
    }

    public void OnMenuClosed(Menu menu)
    {
        openMenus.Remove(menu);
        CheckMenuRestrictingControls();
    }
}
