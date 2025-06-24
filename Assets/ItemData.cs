using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/New Item")]

public class ItemData : ScriptableObject
{
    public Sprite ItemImagePrev1;
    public Sprite ItemImageInHand;
    public String ItemNameText;
    public int ItemPriceInt;
    public String ItemInfosText;

}
