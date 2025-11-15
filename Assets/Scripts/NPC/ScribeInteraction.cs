using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class ScribeInteraction : MonoBehaviour
{
    [SerializeField] private DialogueSequence firstMeetDialogue, incorrectDialogue, correctDialogue, repeatAfterCorrectDialogue;
    [SerializeField] private ScribeQuizPaper quizPaperUI;
    [SerializeField] private Item quizPaperItem;

    private bool quizSolved = false;

    public void OnInteract()
    {
        if (GameManager.inst.player.Inventory.HasItem(quizPaperItem))
        {
            if (quizPaperUI.IsCorrect)
                correctDialogue.DialogueStart();
            else
                correctDialogue.DialogueStart();
        }
        else
        {
            if (!quizSolved)
                firstMeetDialogue.DialogueStart();
            else
                repeatAfterCorrectDialogue.DialogueStart();
        }
    }

    public void OnQuizSolved()
    {
        quizSolved = true;
    }

    public void GiveQuizItem()
    {
        GameManager.inst.player.Inventory.AddItem(quizPaperItem, 1);
    }
}
