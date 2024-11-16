using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractionTrigger : MonoBehaviour, IComparable<InteractionTrigger>
{
    public Action OnInteract;
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
        textPopUp.GetComponentInParent<Canvas>().worldCamera = Camera.main;
        textPopUp.text = triggerKey.ToString();
        ToggleTextPopup(false);
    }

    public void Trigger()
    {
        OnInteract?.Invoke();
    }

    public void ToggleTextPopup(bool value)
    {
        textPopUp.GetComponentInParent<Canvas>(true).gameObject.SetActive(value);
    }

    /*
    float GetDistance() {
        return distance;
    }
    */
}
