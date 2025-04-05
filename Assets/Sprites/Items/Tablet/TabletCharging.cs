using UnityEngine;
using UnityEngine.UI; // Or use TMPro if you're using TextMeshPro
using TMPro; // Make sure to include this if you're using TextMeshPro

public class TabletCharging : MonoBehaviour
{
    [SerializeField] private TMP_Text myText; // Drag your UI Text here
    [SerializeField] private float duration = 2f; // Time to reach 100%

    private float currentPercent = 0f;

    void Start()
    {
        StartCoroutine(GrowPercentage());
    }

    private System.Collections.IEnumerator GrowPercentage()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentPercent = Mathf.Clamp01(elapsed / duration) * 100f;

            myText.text = Mathf.RoundToInt(currentPercent) + "%";

            yield return null;
        }

        myText.text = "100%"; // Make sure we end at 100%
    }
}