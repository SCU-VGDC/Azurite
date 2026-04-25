using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class TextPopup : MonoBehaviour
{
    public bool showOnStart = false;

    private readonly Vector2 hidePos = Vector2.down * 0.3f;

    private RectTransform backgroundTrans;
    private TextMeshProUGUI mainText;
    private CanvasGroup cgroup;
    private Sequence currentTweens;

    public string Text
    {
        set => mainText.text = value;
    }

    public void Start()
    {
        cgroup = GetComponent<CanvasGroup>();
        mainText = GetComponentInChildren<TextMeshProUGUI>();
        backgroundTrans = (RectTransform)transform.Find("Background");
        GetComponent<Canvas>().worldCamera = GameManager.inst.MainCamera;
        cgroup.interactable = false;
        cgroup.blocksRaycasts = false;
        cgroup.alpha = 0f;
        backgroundTrans.anchoredPosition = hidePos;

        if (showOnStart)
            Show();
    }

    public void Show()
    {
        currentTweens?.Kill();
        currentTweens = DOTween.Sequence()
            .Append(backgroundTrans.DOAnchorPos(Vector2.zero, 0.3f))
            .Join(cgroup.DOFade(1f, 0.3f))
            .SetEase(Ease.OutCubic);
    }

    public void Hide(bool destroyOnHide = false)
    {
        currentTweens?.Kill();
        currentTweens = DOTween.Sequence()
            .Append(backgroundTrans.DOAnchorPos(hidePos, 0.3f))
            .Join(cgroup.DOFade(0f, 0.3f))
            .SetEase(Ease.InCubic);
        if (destroyOnHide)
            currentTweens.AppendCallback(() => Destroy(gameObject));
    }
}
