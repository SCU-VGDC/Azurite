using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PopupMenu : Menu
{
    protected override Tween AnimateOnOpen()
    {
        return GetComponent<CanvasGroup>().DOFade(1, 0.3f);
    }

    protected override Tween AnimateOnClose()
    {
        return GetComponent<CanvasGroup>().DOFade(0, 0.3f);
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) && IsOpen)
            Close();
    }
}
