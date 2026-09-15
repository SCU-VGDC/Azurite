using UnityEngine;

public class PopupMenu : FadeMenu
{
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) && IsOpen)
            Close();
    }
}
