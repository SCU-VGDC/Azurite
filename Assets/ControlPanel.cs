using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [Range(0.0f, 1.0f)]

    public float CoverPercentage;
    // Start is called before the first frame update
    void Start()
    {
        float Border = (1f - (CoverPercentage)) / 2;
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(Border, Border);
        rt.anchorMax = new Vector2((1 - Border), (1 - Border));
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;  // ensures stretching, no fixed offset

    }

    public void Exit()
    {
        gameObject.SetActive(false);

    }

    public void MoveToRoom(string roomName)
    {
        ActionManager.Instance.ChangeSubmarineState(roomName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
