using System.Collections.Generic;
using UnityEngine;

public class ControlDisplay : MonoBehaviour
{
    [SerializeField] private ControlKeyItem controlItemPrefab;
    [SerializeField] private RectTransform itemContainer;

    private readonly Dictionary<KeyCode, ControlKeyItem> actions = new();

    public void ShowControl(KeyCode key, string action)
    {
        if (actions.ContainsKey(key))
            return;

        actions[key] = Instantiate(controlItemPrefab, itemContainer);
        actions[key].Action = action;
        actions[key].Key = key;
    }

    public void RemoveControl(KeyCode key)
    {
        if (actions.Remove(key, out var control))
            Destroy(control.gameObject);
    }
}
