using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour 
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    [SerializeField] private KeyCode triggerKey = KeyCode.I;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && menuActivated) {
            InventoryMenu.SetActive(false);
            menuActivated = false;
        } else if (Input.GetKeyDown(triggerKey) && !menuActivated) {
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }
}
