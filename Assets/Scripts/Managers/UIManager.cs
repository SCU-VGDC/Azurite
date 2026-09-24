using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Canvas))]
[AutoStaticsCleanup]
public partial class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [field: SerializeField] public Menu FullscreenMenuContainer { get; private set; }
    [field: SerializeField] public InventoryMenu Inventory { get; private set; }
    [field: SerializeField] public ControlDisplay ControlDisplay { get; private set; }
    [SerializeField] private TitleScreen title;
    [SerializeField] private FadeMenu transitionScreen;
    [SerializeField] private DialogMenu dialogMenuPrefab;
    [SerializeField] private Menu notePopupPrefab;
    [SerializeField] private ItemSubmissionMenu itemSubmissionPrefab;

    public bool AnyMenuOpen => openMenus.Count > 0;
    public Canvas ScreenCanvas => GetComponent<Canvas>();

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

    private void CheckMenuRestrictingControls()
    {
        if (openMenus.Any(menu => menu.restrictPlayerActions))
            GameManager.Instance.Player.Freeze("UIManager");
        else
            GameManager.Instance.Player.Unfreeze("UIManager");
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
        var notePopup = Instantiate(notePopupPrefab, FullscreenMenuContainer.transform);
        notePopup.GetComponentInChildren<TextMeshProUGUI>().text = text;
        notePopup.Open();

        return notePopup;
    }

    public ItemSubmissionMenu CreateItemSubmission()
    {
        var itemSub = Instantiate(itemSubmissionPrefab, Inventory.transform);
        itemSub.Open();
        return itemSub;
    }

    public Tween SetTransitionVisible(bool active, float fadeTime = 0.3f)
    {
        transitionScreen.fadeTime = fadeTime;

        if (active)
            transitionScreen.Open();
        else
            transitionScreen.Close();

        return transitionScreen.CurrentTween;
    }

    public Tween SetTitleVisible(bool active)
    {
        if (active)
        {
            while (openMenus.Count > 0)
                openMenus.First().Close();
            GameManager.Instance.Player.Inventory.Clear();
            title.Open();
        }
        else
            title.Close();

        return title.CurrentTween;
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
