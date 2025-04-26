using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowNote : MonoBehaviour
{
    [SerializeField] private InteractionTrigger interaction;
    [SerializeField] private Image noteImage;
    [SerializeField] private TextMeshProUGUI noteText;
    private bool noteShown = false;

     void Start()
    {
        noteImage.GetComponentInParent<Canvas>(true).worldCamera = Camera.main;
        noteText.GetComponentInParent<Canvas>(true).worldCamera = Camera.main;
        interaction.OnInteract += Read;
        TogglePopup(noteShown);
    }

    public void Read()
    {
        this.PauseGame();
        TogglePopup(noteShown = !noteShown);
    }

    public void TogglePopup(bool value)
    {
        noteImage.GetComponentInParent<Canvas>(true).gameObject.SetActive(value);
        noteText.GetComponentInParent<Canvas>(true).gameObject.SetActive(value);
    }

     public void PauseGame()
    {
        if (GameManager.inst.paused == false)
        {
            Debug.Log("Paused");
            Time.timeScale = 0f;
            
            GameManager.inst.paused = true;
        }

        else if (GameManager.inst.paused == true)
        {
            Debug.Log("Resume");
            Time.timeScale = 1;

            GameManager.inst.paused = false;
        }
    }
}
