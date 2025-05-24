using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Dialogue.Data;
namespace Dialogue.Data 
{
    [Serializable]
    public struct DialogueStep
    {
        public string text;
        int nextDialogue;
        //public UnityEvent<string> continueCallback;
        //public List<string> choices;
        public List<DialogueChoice> choices;
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
    private List<DialogueStep> dialogueSteps;
    [SerializeField] private bool nextSteps;
    public GameObject dialogueChunk;
    public string subjectName = "<NAME>";
    private void Start()
    {
        GameObject tempObject = Instantiate(dialogueChunk);
        dialogueSteps = tempObject.GetComponent<DialogueHolder>().ReturnList();
    }
    public void AddDialogueStep(DialogueStep step)
    {
        dialogueSteps.Add(step);
    }

    public void SetDialogueSteps(List<DialogueStep> steps)
    {
        dialogueSteps = steps;
    }
    public void DialogueStart()
    {
        //GetComponent<InteractionTrigger>().onInteract.AddListener(() => StartCoroutine(StartSequence()));
        StartCoroutine(StartSequence());
    }
    public void UpdateDialogue(DialogueHolder nextDialogueSequence)
    {
        //GameObject tempGameObject = Instantiate(nextDialogueSequence);
        dialogueSteps = nextDialogueSequence.ReturnList();
        nextSteps = true;
        //Destroy(tempGameObject);
    }
    private IEnumerator StartSequence()
    {

        if (dialogueRunning) yield break;
        dialogueRunning = true;

        List<DialogueStep> nextDialogue = dialogueSteps;

        foreach (DialogueStep step in nextDialogue)
        {
            dialogueUI.DisplayText(step.text, subjectName, step.choices);
            //dialogueUI.DisplayText(step.text, subjectName);
            yield return dialogueUI.WaitForPlayerChoice();
            yield return new WaitForEndOfFrame();
            //step.continueCallback?.Invoke(dialogueUI.CurrentChoice);
        }
        dialogueUI.FadeOut();
        dialogueRunning = false;
        if (nextSteps == true)
        {
            nextSteps = false;
            Debug.Log("Next Step Detected");
            yield return StartCoroutine(StartSequence());
        }
    }
}