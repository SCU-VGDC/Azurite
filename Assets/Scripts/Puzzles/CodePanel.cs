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
    private bool awaitingCloseFade = false;
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
            if (awaitingCloseFade) return;

            if (value)
            {
                currentInput = string.Empty;
                inputDisplay.text = string.Empty;
                inputDisplay.GetComponentInParent<Image>().color = Color.black;
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
        foreach (char c in allowedChars)
        {
            GameObject buttonGO = Instantiate(buttonPrefab);
            buttonGO.transform.SetParent(buttonContainer, false);
            buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => {
                if (_visible && !awaitingCloseFade) AppendToInput(c);
            });
        }

        clearButton.onClick.AddListener(ClearInput);
        enterButton.onClick.AddListener(() => StartCoroutine(CheckSolution()));
    }

    private void Update()
    {
        if (!_visible || awaitingCloseFade) return;
        foreach (char c in Input.inputString)
            if (allowedChars.Contains(c))
                AppendToInput(c);
        if (Input.GetButtonDown("Submit"))
            StartCoroutine(CheckSolution());
    }

    private IEnumerator CheckSolution()
    {
        if (currentInput != solution)
        {
            onIncorrectCode.Invoke();
            ClearInput();
            yield break;
        }

        onCorrectCode.Invoke();
        inputDisplay.GetComponentInParent<Image>().DOColor(Color.green, 0.2f);
        inputDisplay.DOColor(Color.black, 0.2f);
        awaitingCloseFade = true;
        yield return new WaitForSeconds(0.7f);
        awaitingCloseFade = false;
        Visible = false;
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
