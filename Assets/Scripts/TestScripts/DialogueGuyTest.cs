using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
[RequireComponent(typeof(DialogueSequence))]
[RequireComponent(typeof(ItemStack))]
[RequireComponent(typeof(SpriteRenderer))]
public class DialogueGuyTest : MonoBehaviour
{
    public void ChangeGreen()
    {
        Debug.Log("Color Change");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;
    }
    public void ChangeRed()
    {
        Debug.Log("Color Change");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
    }

    public void GiveItem()
    {
        GetComponent<ItemStack>().AddTo();
    }
}
