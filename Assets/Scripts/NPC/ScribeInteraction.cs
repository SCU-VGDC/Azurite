using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueSequence))]
[RequireComponent(typeof(InteractionTrigger))]
public class ScribeInteraction : MonoBehaviour
{
    private DialogueSequence dialogue;
    [SerializeField] private ScribeQuizPaper quizPaper;

    void Awake()
    {
        dialogue = GetComponent<DialogueSequence>();
    }

    public void OnInteract()
    {
        dialogue.DialogueStart();
    }
}
