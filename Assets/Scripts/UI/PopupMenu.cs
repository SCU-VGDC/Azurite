using UnityEngine;

public class PopupMenu : FadeMenu
{
    public KeyCode closeKey = KeyCode.E;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(closeKey) || Input.GetMouseButtonDown(0)) && IsOpen)
            Close();
    }
}
