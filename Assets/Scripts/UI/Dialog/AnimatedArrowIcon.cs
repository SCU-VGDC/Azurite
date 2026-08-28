using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AnimatedArrowIcon : MonoBehaviour
{
    public float targetLineLength = 20;
    public float moveTime = 0.55f;

    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform line;

    private bool moveStateDown = false;
    private float t = 0;

    private void Update()
    {
        if (GetComponent<CanvasGroup>().alpha == 0) return;

        t += Time.deltaTime;
        if (t >= moveTime)
        {
            moveStateDown = !moveStateDown;
            t = 0;
        }

        arrow.anchoredPosition = new Vector2(0, DOVirtual.EasedValue(
            moveStateDown ? 6 : 0,
            moveStateDown ? 0 : 6,
            t / moveTime,
            moveStateDown ? Ease.InQuad : Ease.OutQuad
        ));

        line.sizeDelta = new Vector2(DOVirtual.EasedValue(
            moveStateDown ? 5 : targetLineLength,
            moveStateDown ? targetLineLength : 5,
            t / moveTime,
            moveStateDown ? Ease.InQuad : Ease.OutQuad
        ), 2);
    }

    public void Show()
    {
        GetComponent<CanvasGroup>().DOFade(1, 0.3f);
    }

    public void Hide()
    {
        GetComponent<CanvasGroup>().DOFade(0, 0.3f);
    }
}
