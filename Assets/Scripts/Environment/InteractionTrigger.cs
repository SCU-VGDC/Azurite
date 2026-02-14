using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour, IComparable<InteractionTrigger>
{
	[Tooltip("This event is called whenever the player interacts with this object.")]
    public UnityEvent<Player> playerInteractEvent;

	[Tooltip("The key used to trigger an interaction.")]
    [SerializeField] private KeyCode triggerKey = KeyCode.E;

	[Tooltip("The text popup that appears over the interactable object.")]
    [SerializeField] private string popupText;

	[Tooltip("The amount of actions interacting with this object costs.")]
    [SerializeField] private int actionCount = 0;

	private TextMeshProUGUI textPopup = null;

    public int CompareTo(InteractionTrigger other)
    {
        Transform plrTransform = GameManager.inst.player.transform;
        float myDist = Vector2.Distance(transform.position, plrTransform.position);
        float otherDist = Vector2.Distance(other.transform.position, plrTransform.position);
        return MathF.Sign(myDist - otherDist);
    }

	public void Update()
	{
		if(this.textPopup != null)
		{
			this.textPopup.transform.position = this.transform.position + new Vector3(0, 1, 0);
		}
	}

	public void Trigger(Player interactingPlayer)
    {
        if(!GameManager.inst.paused)
        {
            this.playerInteractEvent.Invoke(interactingPlayer);
            ActionManager.Instance.IncrementAction(actionCount);
        }        
    }

    public void ToggleTextPopup(bool value)
    {
		if(value)
		{
			if(this.textPopup != null)
			{
				return;
			}

			GameObject canvas = GameObject.FindGameObjectWithTag("World Canvas");

			if(canvas == null)
			{
				Debug.Log("Failed to find the world canvas!");
				return;
			}

			this.textPopup = new GameObject("InteractionPopup", typeof(TextMeshProUGUI), typeof(ContentSizeFitter)).GetComponent<TextMeshProUGUI>();
			this.textPopup.SetText(this.popupText);
			this.textPopup.color = Color.blue;
			this.textPopup.fontSize = 1;
			this.textPopup.transform.SetParent(canvas.transform);
			this.textPopup.rectTransform.anchoredPosition = new Vector2(0.5f, 1);

			ContentSizeFitter fitter = this.textPopup.GetComponent<ContentSizeFitter>();
			fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			return;
		}
		else if(this.textPopup != null)
		{
			Destroy(this.textPopup.gameObject);
			this.textPopup = null;
			return;
		}
	}

	public KeyCode GetInteractionKey()
	{
		return this.triggerKey;
	}
}
