using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class TextPopup : MonoBehaviour
{
    public bool showOnStart = false;

    private readonly Vector3 hideOffset = Vector3.down * 0.3f;

    private TextMeshProUGUI mainText;
    private CanvasGroup cgroup;
    private Sequence currentTweens;
    private string _text;

    public Vector3 popupOffset;

    public string Text
    {
        set
        {
            _text = value;
            if (mainText != null) mainText.text = value;
        }
        get => _text;
    }

    public void Start()
    {
        cgroup = GetComponent<CanvasGroup>();
        mainText = GetComponentInChildren<TextMeshProUGUI>();
        mainText.text = _text;
        GetComponent<Canvas>().worldCamera = GameManager.Instance.MainCamera;
        cgroup.interactable = false;
        cgroup.blocksRaycasts = false;
        cgroup.alpha = 0f;
        transform.localPosition = popupOffset + hideOffset;

        if (showOnStart)
            Show();
    }

    public void Show()
    {
        currentTweens?.Kill();
        currentTweens = DOTween.Sequence()
            .Append(transform.DOLocalMove(popupOffset, 0.3f))
            .Join(cgroup.DOFade(1f, 0.3f))
            .SetEase(Ease.OutCubic);
    }

    public void Hide(bool destroyOnHide = false)
    {
        currentTweens?.Kill();
        currentTweens = DOTween.Sequence()
            .Append(transform.DOLocalMove(popupOffset + hideOffset, 0.3f))
            .Join(cgroup.DOFade(0f, 0.3f))
            .SetEase(Ease.InCubic);
        if (destroyOnHide)
            currentTweens.AppendCallback(() => Destroy(gameObject));
    }
}
