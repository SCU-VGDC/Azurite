using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Dialogue.Data;
using static UnityEngine.Rendering.DebugUI.Table;
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
    [SerializeField] private List<DialogueStep> dialogueSteps;
    [SerializeField] private bool nextSteps;
    public GameObject dialogueChunk;
    public string subjectName = "<NAME>";
    private void Start()
    {
        dialogueSteps = dialogueChunk.GetComponent<DialogueHolder>().ReturnList();
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
        StartCoroutine(StartSequence());
    }
    //Loads the next dialogue sequence, queuing it without actively transferring to it once the data has been entered.
    public void LoadNextDialogue(GameObject nextDialogueSequence)
    {
        dialogueChunk = nextDialogueSequence;
        dialogueSteps = nextDialogueSequence.GetComponent<DialogueHolder>().ReturnList();
    }
    //Loads the next dialogue sequence, queuing it up and loading it in as the next dialogue chunk.
    public void UpdateDialogue(GameObject nextDialogueSequence)
    {
        dialogueChunk = nextDialogueSequence;
        dialogueSteps = nextDialogueSequence.GetComponent<DialogueHolder>().ReturnList();
        nextSteps = true;
    }
    public List<GameObject> GetChildren(GameObject parent)
    {
        var result = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            result.Add(parent.transform.GetChild(i).gameObject);
        }
        return result;
    }

    public List<GameObject> GetChildNames(GameObject parent)
    {
        var children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }
    private void LogAllChildNames()
    {
        foreach (Transform child in transform)
        {
            Debug.Log($"Child: {child.gameObject.name}");
        }
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
            string chosenText = dialogueUI.CurrentChoice;
            if (string.IsNullOrEmpty(chosenText))
            {
                Debug.LogWarning("No choice text was set! Skipping lookup.");
            }
            else
            {
                Debug.Log("Player picked: " + chosenText);
                Transform found = dialogueChunk.transform.Find(chosenText);
                if (found == null)
                {
                    LogAllChildNames();
                    Debug.LogWarning($"No child named '{chosenText}' under {dialogueChunk.name}. Skipping UpdateDialogue.");
                }
                else
                {
                    Debug.Log(chosenText);
                    UpdateDialogue(found.gameObject);
                }
            }
            step.endCallback.Invoke();
            
            

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