using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This code is intepretted from AspectRatioFitter.

[RequireComponent(typeof(LayoutElement))]
[ExecuteAlways]
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
		this.rectTransform = this.GetComponent<RectTransform>();
		this.element = this.GetComponent<LayoutElement>();
	}

	protected virtual void Update()
	{
		if(this.delayUpdate)
		{
			this.delayUpdate = false;
			this.UpdatePreferred();
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		this.UpdatePreferred();
	}

	protected override void OnTransformParentChanged()
	{
		base.OnTransformParentChanged();
		this.UpdatePreferred();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdatePreferred();
	}

	public void UpdatePreferred()
	{
		if(!this.IsActive())
		{
			return;
		}

		switch(this.aspectMode)
		{
			case AspectMode.None:
				if(!Application.isPlaying)
				{
					this.aspectRatio = Mathf.Clamp(this.rectTransform.rect.width / this.rectTransform.rect.height, 0.001f, 1000f);
				}

				break;
			case AspectMode.HeightControlsWidth:
				this.element.preferredWidth = this.rectTransform.rect.height * this.aspectRatio;
				break;
			case AspectMode.WidthControlsHeight:
				this.element.preferredHeight = this.rectTransform.rect.width / this.aspectRatio;
				break;
		}
	}

	protected override void OnValidate()
	{
		this.aspectRatio = Mathf.Clamp(this.aspectRatio, 0.001f, 1000f);
		this.delayUpdate = true;
	}

	public void SetLayoutHorizontal()
	{
		
	}

	public void SetLayoutVertical()
	{
		
	}
}