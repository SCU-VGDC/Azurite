using UnityEngine;

public class TitleSceneStartup : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.SetTitleVisible(true);
    }
}
