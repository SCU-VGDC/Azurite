using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue.Data 
{
    [Serializable]
    public struct DialogueStep
    {
        public string text;
        public List<DialogueChoice> choices;
        public UnityEvent endCallback;
    }
    [Serializable]
    public struct DialogueChoice
    {
        public string choiceText;
        public UnityEvent<string> choiceCallback;
    }
}

public class DialogueSequence : MonoBehaviour
{
    
    private bool dialogueRunning = false;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueHolder currentDialogueHolder;
    private readonly Queue<DialogueHolder> dialogueHolderQueue = new();
    private Coroutine runningCoroutine;
    public string subjectName = "<NAME>";
    private void Start()
    {
        SetDialogueHolder(currentDialogueHolder);
    }

    public void DialogueStart()
    {
        runningCoroutine = StartCoroutine(StartSequence());
    }
    public void DialogueStop()
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        dialogueUI.FadeOut();
        dialogueRunning = false;
    }
    // Loads the dialogue sequence, queuing it without actively transferring to it once the data has been entered.
    public void SetDialogueHolder(DialogueHolder nextDialogueSequence)
    {
        currentDialogueHolder = nextDialogueSequence;
    }
    // Queues a dialogue sequence; the queue is checked when all dialogue steps are exhausted
    public void QueueDialogueHolder(DialogueHolder nextDialogueSequence)
    {
        dialogueHolderQueue.Enqueue(nextDialogueSequence);
    }
    
    private IEnumerator StartSequence()
    {

        if (dialogueRunning) yield break;
        dialogueRunning = true;

        foreach (var step in currentDialogueHolder.dialogueSteps)
        {
            dialogueUI.DisplayText(step.text, subjectName, step.choices);
            yield return dialogueUI.WaitForPlayerChoice();
            yield return new WaitForEndOfFrame();

            string chosenText = dialogueUI.CurrentChoice;
            if (string.IsNullOrEmpty(chosenText))
            {
                Debug.Log("No choice text was set! Skipping lookup.");
            }
            else
            {
                Debug.Log("Player picked: " + chosenText);
                Transform found = currentDialogueHolder.transform.Find(chosenText);
                if (found == null)
                {
                    Debug.Log($"No child named '{chosenText}' under {currentDialogueHolder.name}. Skipping UpdateDialogue.");
                }
                else
                {
                    Debug.Log($"found {chosenText}");
                    QueueDialogueHolder(found.GetComponent<DialogueHolder>());
                }
            }
            step.endCallback.Invoke();
        }

        dialogueUI.FadeOut();
        dialogueRunning = false;

        if (dialogueHolderQueue.TryDequeue(out var holder))
        {
            SetDialogueHolder(holder);
            yield return StartCoroutine(StartSequence());
        }
    }
}