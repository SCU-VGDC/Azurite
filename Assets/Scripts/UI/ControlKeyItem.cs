using TMPro;
using UnityEngine;

public class ControlKeyItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private TextMeshProUGUI keyText;

    public KeyCode Key
    {
        set => keyText.text = value.ToString();
    }

    public string Action
    {
        get => actionText.text;
        set => actionText.text = value;
    }
}
