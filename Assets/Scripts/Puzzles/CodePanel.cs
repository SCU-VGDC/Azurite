using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class CodePanel : MonoBehaviour
{
    public string solution;
    public UnityEvent onCorrectCode;
    public UnityEvent onIncorrectCode;
    public string allowedChars = string.Empty;
    private string currentInput = string.Empty;
    private bool _visible = false;
    private Image inputDisplayBackground;
    private Sequence currentTweenSequence;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private TextMeshProUGUI inputDisplay;
    [SerializeField] private CanvasGroup uiCanvas;
    [SerializeField] private GameObject buttonPrefab;

    public bool Visible
    {
        get => _visible;
        set
        {
            currentTweenSequence?.Kill();
            currentTweenSequence = null;

            if (value)
            {
                ClearInput();
                inputDisplayBackground.color = Color.black;
                inputDisplay.color = Color.green;
            }

            uiCanvas.DOFade(value ? 1 : 0, 0.2f).SetEase(Ease.InOutQuad);
            uiCanvas.interactable = value;
            uiCanvas.blocksRaycasts = value;
            _visible = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inputDisplayBackground = inputDisplay.GetComponentInParent<Image>();

        foreach (char c in allowedChars)
        {
            GameObject buttonGO = Instantiate(buttonPrefab);
            buttonGO.transform.SetParent(buttonContainer, false);
            buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => {
                if (_visible && !CheckTweenRunning()) AppendToInput(c);
            });
        }

        clearButton.onClick.AddListener(ClearInput);
        enterButton.onClick.AddListener(CheckSolution);
    }

    private bool CheckTweenRunning()
    {
        return currentTweenSequence != null && currentTweenSequence.IsActive();
    }

    private void Update()
    {
        if (!_visible || CheckTweenRunning()) return;
        foreach (char c in Input.inputString)
            if (allowedChars.Contains(c))
                AppendToInput(c);
        if (Input.GetButtonDown("Submit"))
            CheckSolution();
    }

    private void CheckSolution()
    {
        currentTweenSequence?.Kill();

        if (currentInput != solution)
        {
            currentTweenSequence = DOTween.Sequence()
                .Append(inputDisplay.DOColor(Color.black, 0.2f).SetEase(Ease.InCubic))
                .Join(inputDisplayBackground.DOColor(Color.red, 0.2f).SetEase(Ease.InCubic))
                .AppendInterval(0.3f)
                .AppendCallback(ClearInput)
                .Append(inputDisplay.DOColor(Color.green, 0.2f).SetEase(Ease.OutCubic))
                .Join(inputDisplayBackground.DOColor(Color.black, 0.2f).SetEase(Ease.OutCubic))
                .AppendCallback(onIncorrectCode.Invoke);

            return;
        }

        currentTweenSequence = DOTween.Sequence()
            .Append(inputDisplayBackground.DOColor(Color.green, 0.2f))
            .Join(inputDisplay.DOColor(Color.black, 0.2f))
            .AppendInterval(0.8f)
            .AppendCallback(onCorrectCode.Invoke)
            .AppendCallback(() => Visible = false);
    }

    private void ClearInput()
    {
        currentInput = string.Empty;
        inputDisplay.text = string.Empty;
    }

    private void AppendToInput(char append)
    {
        currentInput += append;
        inputDisplay.text = currentInput;
    }
}
