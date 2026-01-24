using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBasicTester : MonoBehaviour
{
    [SerializeField] private InteractionTrigger interaction;

    // Start is called before the first frame update
    void Start()
    {
        interaction.playerInteractEvent.AddListener(this.TestTrigger);
    }

    private void TestTrigger(Player _)
    {
        Debug.Log($"Interaction pressed {gameObject.name}");
    }
}
