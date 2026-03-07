using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class TextPopup : MonoBehaviour
{
    private readonly Vector2 hidePos = Vector2.down * 0.3f;

    private TextMeshProUGUI mainText;
    private CanvasGroup cgroup;
    private Sequence currentTweens;
    private RectTransform rtransform;

    public string Text
    {
        set => mainText.text = value;
    }

    public void Start()
    {
        rtransform = (RectTransform)transform;
        cgroup = GetComponent<CanvasGroup>();
        mainText = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Canvas>().worldCamera = GameManager.inst.MainCamera;
        cgroup.blocksRaycasts = false;
        cgroup.alpha = 0f;
        rtransform.anchoredPosition = hidePos;
    }

    public void Show()
    {
        currentTweens?.Kill();
        currentTweens = DOTween.Sequence()
            .Append(rtransform.DOAnchorPos(Vector2.zero, 0.3f))
            .Join(cgroup.DOFade(1f, 0.3f))
            .SetEase(Ease.OutCubic);
    }

    public void Hide(bool destroyOnHide = false)
    {
        currentTweens?.Kill();
        currentTweens = DOTween.Sequence()
            .Append(rtransform.DOAnchorPos(hidePos, 0.3f))
            .Join(cgroup.DOFade(0f, 0.3f))
            .SetEase(Ease.InCubic);
        if (destroyOnHide)
            currentTweens.AppendCallback(() => Destroy(gameObject));
    }
}
