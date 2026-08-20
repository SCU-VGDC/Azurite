using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This code is intepretted from AspectRatioFitter.

[RequireComponent(typeof(LayoutElement))]
[ExecuteAlways]
[Obsolete]
public class PreferredAspectRatioFitter : UIBehaviour, ILayoutSelfController, ILayoutController
{
    public enum AspectMode
    {
        None,
        WidthControlsHeight,
        HeightControlsWidth
    }

    [SerializeField]
    private AspectMode aspectMode;

    [SerializeField]
    private float aspectRatio = 1f;

    private RectTransform rectTransform = null;
    private LayoutElement element = null;
    private bool delayUpdate = false;

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        element = GetComponent<LayoutElement>();
    }

    protected virtual void Update()
    {
        if (delayUpdate)
        {
            delayUpdate = false;
            UpdatePreferred();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdatePreferred();
    }

    protected override void OnTransformParentChanged()
    {
        base.OnTransformParentChanged();
        UpdatePreferred();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        UpdatePreferred();
    }

    public void UpdatePreferred()
    {
        if (!IsActive())
        {
            return;
        }

        switch (aspectMode)
        {
            case AspectMode.None:
                if (!Application.isPlaying)
                {
                    aspectRatio = Mathf.Clamp(rectTransform.rect.width / rectTransform.rect.height, 0.001f, 1000f);
                }

                break;
            case AspectMode.HeightControlsWidth:
                element.preferredWidth = rectTransform.rect.height * aspectRatio;
                break;
            case AspectMode.WidthControlsHeight:
                element.preferredHeight = rectTransform.rect.width / aspectRatio;
                break;
        }
    }

    protected override void OnValidate()
    {
        aspectRatio = Mathf.Clamp(aspectRatio, 0.001f, 1000f);
        delayUpdate = true;
    }

    public void SetLayoutHorizontal()
    {

    }

    public void SetLayoutVertical()
    {

    }
}