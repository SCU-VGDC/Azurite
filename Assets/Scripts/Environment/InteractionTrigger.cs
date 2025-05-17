using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour, IComparable<InteractionTrigger>
{
    public UnityEvent onInteract;
    public KeyCode triggerKey = KeyCode.E;
    public TextMeshProUGUI textPopUp;

    public int CompareTo(InteractionTrigger other)
    {
        Transform plrTransform = GameManager.inst.player.transform;
        float myDist = Vector2.Distance(transform.position, plrTransform.position);
        float otherDist = Vector2.Distance(other.transform.position, plrTransform.position);
        return MathF.Sign(myDist - otherDist);
    }

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = textPopUp.GetComponentInParent<Canvas>(true);
        canvas.transform.localPosition = new Vector3(0, 1, 0);
        canvas.worldCamera = Camera.main;
        textPopUp.text = triggerKey.ToString();
        ToggleTextPopup(false);
    }

    public void Trigger()
    {
        if (!GameManager.inst.paused)
        {
            this.onInteract?.Invoke();
        }
    }

    public void ToggleTextPopup(bool value)
    {
        textPopUp.GetComponentInParent<Canvas>(true).gameObject.SetActive(value);
    }
}
