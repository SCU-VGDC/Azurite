using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    public struct DialogueStep
    {
        public string text;
        public Action<string> continueCallback;
        public List<string> choices;
    }

    private bool dialogueRunning = false;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private List<DialogueStep> dialogueSteps;
    public string subjectName = "<NAME>";

    public void AddDialogueStep(DialogueStep step)
    {
        dialogueSteps.Add(step);
    }

    public void SetDialogueSteps(List<DialogueStep> steps)
    {
        dialogueSteps = steps;
    }

    public IEnumerator StartSequence()
    {
        if (dialogueRunning) yield break;
        dialogueRunning = true;

        foreach (DialogueStep step in dialogueSteps)
        {
            dialogueUI.DisplayText(step.text, subjectName);
            yield return dialogueUI.WaitForPlayerChoice();
            yield return new WaitForEndOfFrame();
            step.continueCallback?.Invoke(dialogueUI.CurrentChoice);
        }
        dialogueUI.FadeOut();
        dialogueRunning = false;
    }
}