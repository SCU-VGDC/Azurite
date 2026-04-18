using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
[ExecuteAlways]
public class PreferredRelativeSizer : UIBehaviour, ILayoutSelfController, ILayoutController
{
	[SerializeField]
	private RectTransform relativeWidth;

	[SerializeField]
	private float widthScale = 1f;

	[SerializeField]
	private RectTransform relativeHeight;

	[SerializeField]
	private float heightScale = 1f;

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

		this.element.minWidth = (this.relativeWidth != null ? this.relativeWidth.rect.size.x : Screen.width) * this.widthScale;
		this.element.minHeight = (this.relativeHeight != null ? this.relativeHeight.rect.size.y : Screen.height) * this.heightScale;
	}

	protected override void OnValidate()
	{
		this.delayUpdate = true;
	}

	public void SetLayoutHorizontal()
	{
		
	}

	public void SetLayoutVertical()
	{
		
	}
}