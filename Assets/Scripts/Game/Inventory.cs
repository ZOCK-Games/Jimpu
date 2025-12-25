using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public List<ItemData> ItemDatas;
    public List<GameObject> Items;
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
    public GameObject MouseItemPrefab;
    public bool MovingItem;
    private ItemData MovingItemData;
    private InventorySlot currentInvSlotMovingItem;
    private Button HandButton;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        ItemDatas = new List<ItemData>(Resources.LoadAll<ItemData>("Items"));
        for (int i = 0; i < InvSlots.Count; i++)
        {
            if (InvSlots[i].inventorySlot.SlotName == "Hand")
            {
                HandButton = InvSlots[i].ItemSlot.GetComponent<Button>();
            }
        }
    }
    void Start()
    {
        for (int i = 0; i < InvSlots.Count; i++)
        {
            int SlotNumber = i;
            var currentInvSlot = InvSlots[SlotNumber].inventorySlot;
            Button SlotButton = InvSlots[SlotNumber].ItemSlot.GetComponent<Button>();

            SlotButton.onClick.AddListener(() =>
            {
                if (!currentInvSlot.IsEmpty && !MovingItem)
                {
                    LoadItemInfo(currentInvSlot.ItemStored);
                    StartCoroutine(MoveItem(currentInvSlot, SlotNumber));
                }
                else if (MovingItem)
                {
                    Debug.Log("New Item Pos");
                    currentInvSlotMovingItem = currentInvSlot;
                }
            });
        }

        for (int i = 0; i < InvSlots.Count; i++)
        {
            if (InvSlots[i].inventorySlot.name == "Hand")
            {
                HandSlot = InvSlots[i].inventorySlot;
            }
        }


    }

    void OnDisable()
    {
        inputActions.Player.Enable();
        inputActions.UI.Disable();
        InventorySaveManager.Instance.SaveInventory();
    }
    public void OnEnable()
    {
        inputActions.Player.Disable();
        inputActions.UI.Enable();

        InventorySaveManager.Instance.LoadInventory();
        MovingItem = false;
        MouseItemPrefab.SetActive(false);
        currentInvSlotMovingItem = null;

        CloseButton.onClick.AddListener(() => InventoryUI.SetActive(false));
        HandButton.Select();
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
            Destroy(HandItemParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < InvSlots.Count; i++)
        {
            var Slot = InvSlots[i].ItemSlot;
            InventorySlot Inv = InvSlots[i].inventorySlot;
            if (Inv.ItemCount > Inv.MaxItems)
            {
                Inv.ItemCount = Inv.MaxItems;
            }
            if (!Inv.IsEmpty)
            {
                Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Inv.ItemCount.ToString();
                Slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Inv.ItemStored.ItemNameText;
                Slot.transform.GetChild(0).GetComponent<Image>().sprite = Inv.ItemStored.ItemImagePrev;
            }
            else
            {
                Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = 0.ToString();
                Slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "null";
                Slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
    }
    public void AddItem(ItemData item, InventorySlot slot, int? ItemCount)
    {
        if (slot == null)
        {
            for (int i = 0; i < InvSlots.Count; i++)
            {
                InventorySlot Inv = InvSlots[i].inventorySlot;
                GameObject Slot = InvSlots[i].ItemSlot;
                if (Inv.itemType == item.itemType)
                {
                    if (Inv.ItemCount < Inv.MaxItems || Inv.ItemStored == null)
                    {
                        if (Inv.ItemCount <= 0 || Inv.ItemStored == null)  // Adding New information's to the item slot 
                        {
                            if (ItemCount != null)
                            {
                                Inv.ItemCount = (int)ItemCount;
                            }
                            else
                            {
                                Inv.ItemCount = 1;
                            }
                            Inv.ItemStored = item;
                            Inv.itemType = item.itemType;
                            InventorySaveManager.Instance.SaveInventory();
                            ReloadAllItemInfos();
                            return;
                        }
                        else if (Inv.ItemCount > 0 && Inv.ItemStored.ItemNameText == item.ItemNameText) // increasing the item count and refreshing
                        {
                            if (ItemCount != null)
                            {
                                Inv.ItemCount += (int)ItemCount;
                            }
                            else
                            {
                                Inv.ItemCount += 1;
                            }
                            Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Inv.ItemCount.ToString();
                            InventorySaveManager.Instance.SaveInventory();
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
        else
        {
            InventorySlot Inv = slot;
            if (Inv.ItemCount <= 0 || Inv.ItemStored == null)  // Adding New information's to the item slot 
            {
                Inv.ItemCount += 1;
                Inv.ItemStored = item;
                Inv.itemType = item.itemType;
                ReloadAllItemInfos();
                return;
            }
            else if (Inv.ItemCount > 0 && Inv.ItemStored.ItemNameText == item.ItemNameText) // increasing the item count and refreshing
            {
                Inv.ItemCount += 1;
                ReloadAllItemInfos();
                return;
            }
            else
            {
                Debug.Log("There Is no free Item Space");
            }
        }
    }
    public void RemoveItem(ItemData item, int ItemCount, int? InvSlot)
    {
        if (InvSlot == null)
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

                    }
                    InventorySaveManager.Instance.SaveInventory();
                    ReloadAllItemInfos();
                    Debug.Log($"Removed Items");
                    return;
                }
            }
        }
        else
        {
            InventorySlot Inv = InvSlots[InvSlot.Value].inventorySlot;
            if (Inv.ItemStored == item)
            {
                GameObject Slot = InvSlots[InvSlot.Value].ItemSlot;
                Inv.ItemCount += ItemCount;
                Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Inv.ItemCount.ToString();
                if (Inv.ItemCount <= 0)
                {
                    Inv.ItemStored = null;
                }
                InventorySaveManager.Instance.SaveInventory();
                ReloadAllItemInfos();

                Debug.Log($"Removed Items");
                return;
            }
        }
        Debug.Log("There Was no item found to remove");
    }

    public IEnumerator MoveItem(InventorySlot inventorySlot, int InvSlot)
    {
        InventorySlot Inv = InvSlots[InvSlot].inventorySlot;
        ItemData item = inventorySlot.ItemStored;
        if (item != null && !MovingItem)
        {
            int ItemMovingCount = inventorySlot.ItemCount;

            GameObject MouseItem = MouseItemPrefab;
            MouseItem.GetComponent<Image>().sprite = item.ItemImagePrev;
            MouseItem.transform.position = Mouse.current.position.ReadValue();
            MouseItem.SetActive(true);
            ReloadAllItemInfos();
            InvSlots[InvSlot].ItemSlot.transform.GetChild(0).gameObject.SetActive(false);
            MovingItem = true;
            float ElapsedTime = 0;
            currentInvSlotMovingItem = null;
            while (currentInvSlotMovingItem == null)
            {
                MouseItem.transform.position = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
                ElapsedTime += Time.deltaTime;
                yield return null;
            }
            ;
            MovingItem = false;
            InvSlots[InvSlot].ItemSlot.transform.GetChild(0).gameObject.SetActive(true);
            if (ElapsedTime > 5)
            {
                Debug.Log("Item was not set to a new Slot so it canceled");
                yield break;
            }
            RemoveItem(item, -ItemMovingCount, InvSlot);
            AddItem(item, currentInvSlotMovingItem, ItemMovingCount);
            InventorySaveManager.Instance.SaveInventory();
            ReloadAllItemInfos();
            MouseItem.SetActive(false);
        }
        else
        {
            Debug.LogError("Ended no item selected");
        }
        currentInvSlotMovingItem = null;
        MouseItemPrefab.GetComponent<Image>().sprite = null;
        MovingItem = false;
    }

    void Update()
    {
        if (Add)
        {
            Debug.Log("add");
            AddItem(ItemDatas[1], null, null);
            Add = false;
        }
        if (Remove)
        {
            RemoveItem(ItemDatas[1], -1, null);
            Remove = false;
        }

        if (inputActions.Player.Inventory.WasPerformedThisFrame())
        {
            InventoryUI.SetActive(!InventoryUI.activeSelf);
        }
        if (HandSlot.ItemStored != null &&
        (
            HandItemParent.transform.childCount == 0 ||
            HandItemParent.transform.GetChild(0).gameObject.name != HandSlot.ItemStored.ItemObject.name
        ))
        {
            for (int i = 0; i < HandItemParent.transform.childCount; i++)
            {
                Destroy(HandItemParent.transform.GetChild(i).gameObject);
            }
            GameObject HandItem = Instantiate(HandSlot.ItemStored.ItemObject);
            HandItem.transform.SetParent(HandItemParent.transform);
            HandItem.name = HandSlot.ItemStored.ItemObject.name;
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

[CreateAssetMenu(fileName = "NewItemSlotData", menuName = "Game Data/Item Slot Data")] // The Singel Item slot 
public class InventorySlot : ScriptableObject
{
    public String SlotName;
    public int SlotNumber;
    public int MaxItems;
    public int ItemCount;
    public ItemType itemType;
    public ItemData ItemStored;
    public bool IsEmpty => ItemStored == null;
}

[System.Serializable]
public class ItemSlotSaveData
{
    public int slotIndex;
    public string itemID;
    public int ItemAmount;
}

[System.Serializable]
public class InventorySaveData
{
    public List<ItemSlotSaveData> slots = new List<ItemSlotSaveData>();
}


public enum ItemType
{
    AllItems,
    Head,
    Chest,
    Leg,
    Feet
}
