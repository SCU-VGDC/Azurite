using DG.Tweening;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(CanvasGroup))]
public class LostPagesMenu : Menu
{
    

    public KeyCode closeKey = KeyCode.E;
    [SerializeField] private LostPageQuestion questionPrefab;
    [SerializeField] private RectTransform questionContainer;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    private LostPages pageItem;
    private readonly List<LostPageQuestion> questionUIs = new();

    private int currentPage;
    public int Page
    {
        get => currentPage;
        set
        {
            questionUIs[currentPage].gameObject.SetActive(false);
            questionUIs[value].gameObject.SetActive(true);
            prevButton.interactable = value > 0;
            nextButton.interactable = value < questionUIs.Count - 1;
            currentPage = value;
        }
    }

    protected override Tween AnimateOnOpen()
    {
        return GetComponent<CanvasGroup>().DOFade(1, 0.4f);
    }

    protected override Tween AnimateOnClose()
    {
        return GetComponent<CanvasGroup>().DOFade(0, 0.4f);
    }

    public override void Close()
    {
        base.Close();

        for (int i = 0; i < pageItem.questions.Count; i++)
            pageItem.questions[i].savedAnswer = questionUIs[i].PlayerAnswer;
    }

    private void Start()
    {
        pageItem = GameManager.Instance.Player.Inventory.Items.First(item => item is LostPages) as LostPages;

        foreach (var q in pageItem.questions)
        {
            var qObj = Instantiate(questionPrefab, questionContainer);
            qObj.QuestionText = q.question;
            qObj.expectedAnswer = q.correctAnswer;
            qObj.PlayerAnswer = q.savedAnswer;
            qObj.gameObject.SetActive(false);
            questionUIs.Add(qObj);
        }

        questionUIs[0].gameObject.SetActive(true);

        nextButton.onClick.AddListener(() => ++Page);
        prevButton.onClick.AddListener(() => --Page);
    }

    private void Update()
    {
        if (!IsOpen)
            return;

        if (Input.GetKeyDown(closeKey) && !questionUIs.Any(q => q.IsTyping))
            Close();

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData data = new(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> res = new();
            EventSystem.current.RaycastAll(data, res);

            if (!res.Any(r => r.gameObject.transform.IsChildOf(transform)))
                Close();
        }
    }
}
