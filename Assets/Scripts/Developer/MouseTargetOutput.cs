using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public class MouseTargetOutput : MonoBehaviour
{
    private void Update()
    {
        List<RaycastResult> results = new();
        PointerEventData pointer = new(EventSystem.current)
        {
            position = Input.mousePosition
        };
        GetComponent<GraphicRaycaster>().Raycast(pointer, results);
        foreach (var res in results)
            Debug.Log(res.gameObject);
    }
}
