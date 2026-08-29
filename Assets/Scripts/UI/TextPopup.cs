using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class TextPopup : MonoBehaviour
{
    private readonly Vector3 hideOffset = Vector3.down * 0.2f;

    private TextMeshProUGUI mainText;
    private Sequence currentTweens;
    private bool initDone = false;
    public Vector3 popupOffset;

    private string _text;
    public string Text
    {
        set
        {
            _text = value;
            if (mainText != null) mainText.text = value;
        }
        get => _text;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (initDone) return;

        mainText = GetComponentInChildren<TextMeshProUGUI>();
        mainText.text = _text;
        GetComponent<Canvas>().worldCamera = GameManager.Instance.MainCamera;

        var cgroup = GetComponent<CanvasGroup>();
        cgroup.interactable = false;
        cgroup.blocksRaycasts = false;
        cgroup.alpha = 0f;

        transform.localPosition = popupOffset + hideOffset;
    }

    public void Show()
    {
        Init();

        currentTweens?.Kill();
        currentTweens = DOTween.Sequence()
            .Append(transform.DOLocalMove(popupOffset, 0.3f))
            .Join(GetComponent<CanvasGroup>().DOFade(1f, 0.3f))
            .SetEase(Ease.OutCubic);
    }

    public void Hide(bool destroyOnHide = false)
    {
        currentTweens?.Kill();
        currentTweens = DOTween.Sequence()
            .Append(transform.DOLocalMove(popupOffset + hideOffset, 0.3f))
            .Join(GetComponent<CanvasGroup>().DOFade(0f, 0.3f))
            .SetEase(Ease.InCubic);
        if (destroyOnHide)
            currentTweens.AppendCallback(() => Destroy(gameObject));
    }
}
