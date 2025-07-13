using System;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/New Item")]

public class ItemData : ScriptableObject
{
    public Sprite ItemImagePrev1;
    public Sprite ItemImageInHand;
    public String ItemNameText;
    public int ItemPriceInt;
    public String ItemInfosText;
    public int Price;
    public int Sell_Price;
}
