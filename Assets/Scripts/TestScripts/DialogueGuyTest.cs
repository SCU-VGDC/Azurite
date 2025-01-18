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
        GetComponent<InteractionTrigger>().OnInteract += () => StartCoroutine(sequence.StartSequence());
    }
}
