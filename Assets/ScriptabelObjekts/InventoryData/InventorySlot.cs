using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "ItemSlotData", menuName = "Game Data/ItemSlotData")] // The Singel Item slot 
public class InventorySlot : ScriptableObject
{
    public string SlotName;
    public int SlotNumber;
    public int MaxItems;

    [SerializeField]
    private int itemCount;

    public int ItemCount
    {
        get
        {
            if (ItemStored == null)
                return 0;

            return itemCount;
        }
        set
        {
            itemCount = value;
        }
    }

    public ItemType itemType;
    public ItemData ItemStored;

    public bool IsEmpty => ItemStored == null;
}