using System;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/New Item")]

public class ItemData : ScriptableObject
{
    public string itemID;
    public Sprite ItemImagePrev;
    public Sprite ItemImageInHand;
    public string ItemNameText;
    public string ItemInfosText;
    public string ItemDescription;
    public ItemRarityEnum ItemRarity;
    public string ItemCanBeFoundIn;
    public int BuyPrice;
    public int SellPrice;
    public ItemType itemType;
    public GameObject ItemObject;
}
    public enum ItemRarityEnum
    {
        Common,
        Rare,
        ExtremeRare
    }
