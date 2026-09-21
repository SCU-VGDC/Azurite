using System;
using TMPro;
using UnityEngine;

public class LostPageQuestion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionTextDisplay;
    [SerializeField] private TMP_InputField answerField;

    [NonSerialized] public string expectedAnswer;
    public bool IsTyping => answerField.isFocused;
    public bool Correct => answerField.text == expectedAnswer;
    public string PlayerAnswer
    {
        get => answerField.text;
        set => answerField.text = value;
    }
    public string QuestionText
    {
        get => questionTextDisplay.text;
        set => questionTextDisplay.text = value;
    }
}
