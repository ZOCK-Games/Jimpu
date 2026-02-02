using UnityEngine;
using System.Collections.Generic;

public class InventorySaveManager : MonoBehaviour
{
    public Inventory inventory;
    public List<ItemData> allItems;
    public List<InventorySlot> slots;
    public static InventorySaveManager Instance;
    private string SaveKey = "Inventory_Save";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }
    void Start()
    {
        allItems = inventory.ItemDatas;
        List<InventorySlot> NewSlots = new List<InventorySlot>();
        for (int i = 0; i < inventory.InvSlots.Count; i++)
        {
            if (inventory.InvSlots[i].inventorySlot != null)
            {
                NewSlots.Add(inventory.InvSlots[i].inventorySlot);
            }
        }
        slots = NewSlots;
    }
    void OnDisable()
    {
        SaveInventory();
    }


    public void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData();

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
                continue;

            saveData.slots.Add(new ItemSlotSaveData
            {
                slotIndex = i,
                itemID = slots[i].ItemStored.itemID,
                ItemAmount = slots[i].ItemCount
            });
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Inventory Saved");
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
            return;

        string json = PlayerPrefs.GetString(SaveKey);
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

        foreach (var slot in slots)
        {
            slot.ItemStored = null;
            slot.ItemCount = 0;
        }

        foreach (var savedSlot in saveData.slots)
        {
            ItemData item = GetItemSO(savedSlot.itemID);
            if (item == null)
                continue;

            slots[savedSlot.slotIndex].ItemStored = item;
            slots[savedSlot.slotIndex].ItemCount = savedSlot.ItemAmount;
        }
        Debug.Log("Inventory Loaded");
    }

    ItemData GetItemSO(string id)
    {
        return allItems.Find(i => i.itemID == id);
    }
}
