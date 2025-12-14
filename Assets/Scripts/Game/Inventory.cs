using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotInfo
{
    public InventorySlot inventorySlot;
    public GameObject ItemSlot;
}
[System.Serializable]
public class ItemInfo
{
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemRarity;
    public TextMeshProUGUI ItemType;
    public Image ItemDisplayImage;
    public TextMeshProUGUI ItemDescription;
    public TextMeshProUGUI ItemCanBeFoundIn;
    public TextMeshProUGUI ItemSellPrice;
    public TextMeshProUGUI ItemBuyPrice;
}

public class Inventory : MonoBehaviour, IDataPersitence
{
    public List<ItemData> itemData;
    public List<GameObject> Item;
    public string CurentItem;
    public int CurentItemData;
    public List<SlotInfo> InvSlots = new List<SlotInfo>();
    public List<ItemInfo> ItemInfos = new List<ItemInfo>();
    public bool Add;
    public bool Remove;
    public Button CloseButton;
    public GameObject InventoryUI;
    public GameObject HandItemParent;
    private InputSystem_Actions inputActions;
    public InventorySlot HandSlot;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        inputActions.Player.Enable();
    }
    void OnDisable()
    {
        inputActions.Player.Disable();
    }
    public void Start()
    {
        for (int i = 0; i < InvSlots.Count; i++)
        {
            var currentInvSlot = InvSlots[i].inventorySlot;
            Button SlotButton = InvSlots[i].ItemSlot.GetComponent<Button>();
            if (currentInvSlot.ItemStored != null)
            {
                SlotButton.onClick.AddListener(() => LoadItemInfo(currentInvSlot.ItemStored));
            }
        }
        for (int i = 0; i < InvSlots.Count; i++)
        {
            if (InvSlots[i].inventorySlot.name == "Hand")
            {
                HandSlot = InvSlots[i].inventorySlot;
            }
        }
        CloseButton.onClick.AddListener(() => InventoryUI.SetActive(false));
        ReloadAllItemInfos();
    }
    public void LoadItemInfo(ItemData itemData)
    {
        if (itemData != null)
        {
            ItemInfos[0].ItemBuyPrice.text = itemData.BuyPrice.ToString();
            ItemInfos[0].ItemCanBeFoundIn.text = itemData.ItemCanBeFoundIn;
            ItemInfos[0].ItemDescription.text = itemData.ItemDescription.ToString();
            ItemInfos[0].ItemDisplayImage.sprite = itemData.ItemImagePrev;
            ItemInfos[0].ItemName.text = itemData.ItemNameText;
            ItemInfos[0].ItemRarity.text = itemData.ItemRarity.ToString();
            ItemInfos[0].ItemSellPrice.text = itemData.SellPrice.ToString();
            ItemInfos[0].ItemType.text = itemData.itemType.ToString();
        }
        else
        {
            Debug.Log("ItemData is null");
        }
    }
    public void ReloadAllItemInfos()
    {
        for (int i = 0; i < HandItemParent.transform.childCount; i++)
        {
            Destroy(HandItemParent.transform.GetChild(i));
        }
        for (int i = 0; i < InvSlots.Count; i++)
        {
            if (InvSlots[i].inventorySlot.ItemStored != null)
            {
                GameObject Slot = InvSlots[i].ItemSlot;
                InventorySlot Inv = InvSlots[i].inventorySlot;
                Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Inv.ItemCount.ToString();
                Slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Inv.ItemStored.ItemNameText;
                Slot.transform.GetChild(0).GetComponent<Image>().sprite = Inv.ItemStored.ItemImagePrev;
            }
        }
    }
    public void AddItem(ItemData item)
    {
        for (int i = 0; i < InvSlots.Count; i++)
        {
            InventorySlot Inv = InvSlots[i].inventorySlot;
            GameObject Slot = InvSlots[i].ItemSlot;
            if (Inv.itemType == item.itemType)
            {
                if (Inv.ItemCount < Inv.MaxItems)
                {
                    if (Inv.ItemCount == 0)  // Adding New information's to the item slot 
                    {
                        Inv.ItemCount += 1;
                        Inv.ItemStored = item;
                        Inv.itemType = item.itemType;
                        Start();
                        return;
                    }
                    else if (Inv.ItemCount > 0 && Inv.ItemStored.ItemNameText == item.ItemNameText) // increasing the item count and refreshing
                    {
                        Inv.ItemCount += 1;
                        Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Inv.ItemCount.ToString();
                        return;
                    }
                    else
                    {
                        Debug.Log("There Is no free Item Space");
                    }
                }
                else
                {
                    Debug.Log("Found Item Slot But there is no space left");
                }
            }
            else
            {
                Debug.Log($"There is no Item slot for this item type: {item.itemType}");
            }
        }
    }
    public void RemoveItem(ItemData item, int ItemCount)
    {
        for (int i = 0; i < InvSlots.Count; i++)
        {
            InventorySlot Inv = InvSlots[i].inventorySlot;
            if (Inv.ItemStored == item)
            {
                GameObject Slot = InvSlots[i].ItemSlot;
                Inv.ItemCount += ItemCount;
                Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Inv.ItemCount.ToString();
                if (Inv.ItemCount <= 0)
                {
                    Inv.ItemStored = null;
                    Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = 0.ToString();
                    Slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
                    Slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
                }

                Debug.Log($"Removed Items");
                return;
            }
        }
        Debug.Log("There Was no item found to remove");
    }

    void Update()
    {
        if (Add)
        {
            Debug.Log("add");
            AddItem(itemData[0]);
            Add = false;
        }
        if (Remove)
        {
            RemoveItem(itemData[0], -1);
            Remove = false;
        }

        if (inputActions.Player.Inventory.WasPerformedThisFrame())
        {
            InventoryUI.SetActive(!InventoryUI.activeSelf);
        }
        if (HandSlot.ItemStored != null && HandItemParent.transform.GetChild(0).gameObject.name != HandSlot.ItemStored.ItemObject.name || HandSlot.ItemStored != null &&  HandItemParent.transform.GetChild(0) == null)
        {
            for (int i = 0; i < HandItemParent.transform.childCount; i++)
            {
                Destroy(HandItemParent.transform.GetChild(i).gameObject);
            }
            GameObject HandItem = Instantiate(HandSlot.ItemStored.ItemObject);
            HandItem.name = HandSlot.ItemStored.ItemObject.name;
            HandItem.transform.SetParent(HandItemParent.transform);
        }
        if (HandSlot.ItemStored == null)
        {
            for (int i = 0; i < HandItemParent.transform.childCount; i++)
            {
                Destroy(HandItemParent.transform.GetChild(i).gameObject);
            }
        }


    }
    public void RemoveHandItem(int Amount)
    {
        HandSlot.ItemCount += Amount;
        HandSlot.ItemStored = null;
        ReloadAllItemInfos();
    }
    public void LoadGame(GameData data)
    {
        this.CurentItem = data.CurentItem;
    }

    public void SaveGame(ref GameData data)
    {
        data.CurentItem = this.CurentItem;
    }
}

[CreateAssetMenu(fileName = "NewItemSlotData", menuName = "Game Data/Item Slot Data")]
public class InventorySlot : ScriptableObject
{
    public String SlotName;
    public int SlotNumber;
    public int MaxItems;
    public int ItemCount;
    public ItemType itemType;
    public ItemData ItemStored;
}

public enum ItemType
{
    AllItems,
    Head,
    Chest,
    Leg,
    Feet
}
