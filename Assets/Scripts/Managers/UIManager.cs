using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [field: SerializeField] public Menu FullscreenMenuContainer { get; private set; }
    [SerializeField] private DialogMenu dialogMenuPrefab;

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
        var dialogMenu = Instantiate(dialogMenuPrefab, FullscreenMenuContainer.transform);
        dialogMenu.Init(dialog);
        return dialogMenu;
    }
}
