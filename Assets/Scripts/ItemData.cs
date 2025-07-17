using System;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/New Item")]

public class ItemData : ScriptableObject
{
    public Sprite ItemImagePrev1;
    public Sprite ItemImageInHand;
    public string ItemNameText;
    public int ItemPriceInt;
    public string ItemInfosText;
    public int Price;
    public int Sell_Price;
}
