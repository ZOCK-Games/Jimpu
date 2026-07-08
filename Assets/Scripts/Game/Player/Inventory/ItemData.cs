using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/New Item")]

public class ItemData : ScriptableObject
{
    //General
    public string itemID;
    public int MaxStackAble;
    public float DropChance;
    public PlayerBodyPartTypes playerBodyPartType;
    public GamePlaySettings gamePlaySettings;
    //Types 
    public ItemType itemType;
    public ItemRarityEnum ItemRarity;

    //UI
    [Header("UI Element Settings")]
    public Sprite ItemImagePrev;
    public Sprite ItemImageInHand;
    public string ItemNameText;
    [TextArea(1, 3)]
    public string ItemInfosText;
    [TextArea(2, 10)]
    public string ItemDescription;
    [TextArea(1, 2)]
    public string ItemCanBeFoundIn; // A tip for the player where he can find the item 

    //Economy
    public int BuyPrice;
    public int SellPrice;
    [Header("The Object that gets created when trigger")]
    public AssetReference ItemReference;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
        }
    }
}

[System.Serializable]
public class GamePlaySettings
{
    // Gameplay
    public bool IsQuestItem;
    public bool IsConsumable;
    public bool IsCraftingMaterial;
    public bool IsPlaceable;
    public bool IsTradable;
    public bool IsDroppable;
}
public enum ItemRarityEnum
{
    Common,
    Rare,
    ExtremeRare
}
