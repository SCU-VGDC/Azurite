using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBasicTester : MonoBehaviour
{
    [SerializeField] private InteractionTrigger interaction;

    // Start is called before the first frame update
    void Start()
    {
        interaction.onInteract.AddListener(this.TestTrigger);
    }

    private void TestTrigger()
    {
        Debug.Log($"Interaction pressed {gameObject.name}");
    }
}
