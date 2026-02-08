using UnityEngine;

[CreateAssetMenu(fileName = "ItemSlotData", menuName = "Game Data/ItemSlotData")] // The Singel Item slot 
public class InventorySlot : ScriptableObject
{
    public string SlotName;
    public int SlotNumber;
    public int MaxItems;
    public int ItemCount;
    public ItemType itemType;
    public ItemData ItemStored;
    public bool IsEmpty => ItemStored == null;
}