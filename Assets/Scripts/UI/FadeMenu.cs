using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeMenu : Menu
{
    public float fadeTime = 0.3f;

    protected override Tween AnimateOnOpen()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.blocksRaycasts = true;
        return cg.DOFade(1, fadeTime);
    }

    protected override Tween AnimateOnClose()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        return cg.DOFade(0, fadeTime);
    }
}
