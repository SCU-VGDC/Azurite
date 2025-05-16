using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue.Data;
public class DialogueHolder : MonoBehaviour
{
    public List<DialogueStep> dialogueSteps;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public List<DialogueStep> ReturnList()
    {
        return dialogueSteps;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
