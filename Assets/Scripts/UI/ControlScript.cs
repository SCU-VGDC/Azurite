using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlScript : MonoBehaviour
{
    // Start is called before the first frame update

    // [SerializeField] private GameObject popupUI;
    private Canvas canvas;

    void Start()
    {
        GameObject tempObject = GameObject.Find("SubmarineCanvas");
        if (tempObject != null)
        {
            //If we found the object , get the Canvas component from it.
            canvas = tempObject.GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.Log("Could not locate Canvas component on " + tempObject.name);
            }
        }
        if (canvas != null) canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Exit()
    {
        if (canvas != null) canvas.gameObject.SetActive(false);

    }

    public void MoveToRoom(string roomName)
    {
        ActionManager.Instance.ChangeSubmarineState(roomName);
    }

    public void Activate()
    {
        if (canvas != null){
            canvas.gameObject.SetActive(true);
            Debug.Log("Activating Canvas");
                };
        
    }
}
