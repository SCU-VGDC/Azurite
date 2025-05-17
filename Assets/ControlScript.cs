using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject popupUI;

    void Start()
    {
        if (popupUI != null) popupUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (popupUI != null) popupUI.SetActive(true);
    }
}
