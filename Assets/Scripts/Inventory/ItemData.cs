using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]

public class ItemData : ScriptableObject
{
    [SerializeField] private string ItemName; // Name Displayed to Player
    [SerializeField] private Sprite ItemIcon; // Image of item
    [SerializeField] private string ItemDescription; // Description of Item for the Player
    [SerializeField] private int ItemID; // Place in ITEM_LIST in persistant Data

    public string GetName()
    {
        return ItemName;
    }
    public Sprite GetSprite()
    {
        return ItemIcon;
    }
    public string GetItemDescription()
    {
        return ItemDescription;
    }
    public int GetItemID()
    {
        return ItemID;
    }
}
