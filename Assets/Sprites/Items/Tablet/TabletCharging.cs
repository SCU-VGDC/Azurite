using UnityEngine;
using TMPro;

public class TabletCharging : MonoBehaviour
{
    [SerializeField] private TMP_Text myText;
    [SerializeField] private float duration = 2f;

    private float currentPercent = 0f;
    private bool isCharging = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCharging)
        {
            Debug.Log("Starting charging process...");
            StartCoroutine(GrowPercentage());
        }
    }

    private System.Collections.IEnumerator GrowPercentage()
    {
        isCharging = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentPercent = Mathf.Clamp01(elapsed / duration) * 100f;

            Debug.Log("Current Percent: " + currentPercent);
            myText.text = Mathf.RoundToInt(currentPercent) + "%";

            yield return null;
        }

        myText.text = "100%";
        isCharging = false;
    }
}