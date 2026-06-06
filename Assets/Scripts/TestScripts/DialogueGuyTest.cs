using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
[RequireComponent(typeof(DialogueSequence))]
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

    public void GiveItem(Player player)
    {
		player.GetInventory().AddItem(GetComponent<ItemStack>());
    }
}
