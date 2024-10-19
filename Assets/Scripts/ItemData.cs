using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]

public class ItemData : ScriptableObject
{
    [SerializeField] private string ItemName;
    [SerializeField] private Sprite ItemIcon;
    [SerializeField] private string ItemDescription;
    [SerializeField] private int ItemID;

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
