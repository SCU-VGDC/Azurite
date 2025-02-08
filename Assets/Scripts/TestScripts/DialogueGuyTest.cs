using UnityEngine;

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

    public void ProcessPlayerResponse(string choice)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (choice == "me")
            spriteRenderer.color = Color.green;
        else
            spriteRenderer.color = Color.red;
    }
}
