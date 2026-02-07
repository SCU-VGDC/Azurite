using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class ScribeInteraction : MonoBehaviour
{
    [SerializeField] private DialogController firstMeetDialogue, incorrectDialogue, correctDialogue, repeatAfterCorrectDialogue;
    [SerializeField] private ScribeQuizPaper quizPaperUI;
    [SerializeField] private Item quizPaperItem;

    private bool quizSolved = false;

    public void OnInteract()
    {
        if (GameManager.inst.player.Inventory.HasItem(quizPaperItem))
        {
            if (quizPaperUI.IsCorrect)
                correctDialogue.Select(0);
            else
                incorrectDialogue.Select(0);
        }
        else
        {
            if (!quizSolved)
                firstMeetDialogue.Select(0);
            else
                repeatAfterCorrectDialogue.Select(0);
        }
    }

    public void OnQuizSolved()
    {
        quizSolved = true;
        // something else happens later
    }

    public void GiveQuizItem()
    {
        GameManager.inst.player.Inventory.AddItem(quizPaperItem, 1);
    }
}
