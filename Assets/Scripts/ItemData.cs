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

    public string getName()
    {
        return ItemName;
    }
    public Sprite getSprite()
    {
        return ItemIcon;
    }
    public string getItemDescription()
    {
        return ItemDescription;
    }
    public int getItemID()
    {
        return ItemID;
    }


}
