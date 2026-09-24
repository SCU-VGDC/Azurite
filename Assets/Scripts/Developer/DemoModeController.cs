using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoModeController : MonoBehaviour
{
    public static bool DemoMode => Debug.isDebugBuild;

    public string gameResetTargetScene = "Title";
    public KeyCode[] gameResetShortcut = new KeyCode[] { KeyCode.LeftControl, KeyCode.RightControl, KeyCode.R };

    private void Awake()
    {
        if (!DemoMode)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (gameResetShortcut.All(key => Input.GetKey(key)))
        {
            SceneManager.LoadScene(gameResetTargetScene);
            PersistentDataManager.Instance.Clear();
        }
    }
}
