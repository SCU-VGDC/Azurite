using UnityEngine;

public class CodeInputTester : MonoBehaviour
{
    [SerializeField] private CodePanel codePanel;

    // Start is called before the first frame update
    void Start()
    {
        codePanel.onCorrectCode.AddListener(() => Debug.Log("Correct code!"));
        codePanel.onIncorrectCode.AddListener(() => Debug.Log("Incorrect code!"));

        GetComponent<InteractionTrigger>().onInteract.AddListener(ShowPanel);
    }

    private void ShowPanel()
    {
        codePanel.Visible = true;
    }
}
