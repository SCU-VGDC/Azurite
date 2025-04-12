using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueSequence))]
[RequireComponent(typeof(InteractionTrigger))]
public class ScribeInteraction : MonoBehaviour
{
    private InteractionTrigger interactionTrigger;
    private DialogueSequence sequence;

    void Awake()
    {
        interactionTrigger = GetComponent<InteractionTrigger>();
        sequence = GetComponent<DialogueSequence>();

        interactionTrigger.OnInteract += () => StartCoroutine(sequence.StartSequence());
    }

    public void ShowQuestionsUI(string playerChoice)
    {
        if (playerChoice != "Yes") return;
        Debug.Log("wow");
    }
}
