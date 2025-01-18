using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(InteractionTrigger))]
[RequireComponent(typeof(DialogueSequence))]
public class DialogueGuyTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DialogueSequence sequence = GetComponent<DialogueSequence>();

        sequence.SetDialogueSteps(new List<DialogueSequence.DialogueStep>
            {
            new() {
                text = "19 dollar fortnite card."
            },
            new()
            {
                text = "Who wants it?"
            }
            }
        );

        GetComponent<InteractionTrigger>().OnInteract += () => StartCoroutine(sequence.StartSequence());
    }
}
