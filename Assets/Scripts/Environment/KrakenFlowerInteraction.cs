using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KrakenFlowerInteraction : ItemSubmissionInteraction
{
    [SerializeField] private GameObject krakenObject;
    [SerializeField] private SpriteRenderer flowerDisplay;
    [SerializeField] private Item correctItem;
    [SerializeField] private Dialog correctDialog;
    [SerializeField] private Dialog incorrectDialog;

    private Vector3 flowerDisplayStartPos;

    private void Start()
    {
        flowerDisplayStartPos = flowerDisplay.transform.localPosition;
    }

    public override bool CheckSubmittedItem(Item item)
    {
        return item.Categories.Contains(Item.Category.FLOWER);
    }

    public async override void OnSubmissionPassed(Item flower)
    {
        flowerDisplay.sprite = flower.Icon;
        flowerDisplay.transform.localPosition = flowerDisplayStartPos;
        flowerDisplay.transform.Translate(-((flower.Icon.rect.size / 2f - flower.Icon.pivot) / flower.Icon.pixelsPerUnit), Space.Self);

        GameManager.Instance.Player.Freeze("KrakenScene");
        UIManager.Instance.Inventory.Close();
        await Awaitable.WaitForSecondsAsync(1.5f);
        GameManager.Instance.Player.Unfreeze("KrakenScene");
        flowerDisplay.sprite = null;

        if (flower == correctItem)
        {
            Destroy(krakenObject);
            UIManager.Instance.CreateDialog(correctDialog);
        }
        else
            UIManager.Instance.CreateDialog(incorrectDialog);
    }
}
