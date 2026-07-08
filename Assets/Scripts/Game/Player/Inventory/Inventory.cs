using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

[System.Serializable]
public class SlotInfo
{
    public InventorySlot inventorySlot;
    public GameObject ItemSlot;
}


[System.Serializable]
public class ItemInfoUI // For the UI Elements
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
    public static Inventory instance { get; private set; }
    public List<ItemData> ItemDatas;
    public string CurentItem;
    public int CurentItemData;
    public List<SlotInfo> InvSlots = new List<SlotInfo>();
    public List<ItemInfoUI> ItemInfosUI = new List<ItemInfoUI>();
    public bool Add;
    public bool Remove;
    public Button CloseButton;
    public GameObject InventoryUI;
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
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        for (int i = 0; i < InvSlots.Count; i++)
        {
            int SlotNumber = i;
            var currentInvSlot = InvSlots[SlotNumber].inventorySlot;
            Button SlotButton = InvSlots[SlotNumber].ItemSlot.GetComponent<Button>();

            SlotButton.onClick.AddListener(async () =>
            {
                if (!currentInvSlot.IsEmpty && !MovingItem)
                {
                    LoadItemInfo(currentInvSlot.ItemStored);
                    await MoveItem(currentInvSlot, SlotNumber);
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

        SaveManager.instance.Load();

        gameObject.AddComponent<InvPlayerEquip>();

        CheckBodyPartItemObjects();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }
    public async Task OnEnable()
    {
        inputActions.Player.Enable();
        MovingItem = false;
        MouseItemPrefab.SetActive(false);
        currentInvSlotMovingItem = null;

        inputActions.Player.Inventory.performed += ctx => ToggleInventory();
        CloseButton.onClick.AddListener(() => ToggleInventory());

        HandButton.Select();
        await ReloadAllItemInfos();
    }
    public void ToggleInventory()
    {
        bool IsActive = InventoryUI.activeSelf;
        if (!IsActive)
        {
            CloseButton.Select();
            playerControl.instance.CanMove = false;
            playerControl.instance.PlayerState.CanAttack = false;
        }
        else
        {
            playerControl.instance.CanMove = true;
            playerControl.instance.PlayerState.CanAttack = true;
        }
        InventoryUI.SetActive(!IsActive);
    }
    public void LoadItemInfo(ItemData itemData)
    {
        if (itemData != null)
        {
            ItemInfosUI[0].ItemBuyPrice.text = itemData.BuyPrice.ToString();
            ItemInfosUI[0].ItemCanBeFoundIn.text = itemData.ItemCanBeFoundIn;
            ItemInfosUI[0].ItemDescription.text = itemData.ItemDescription.ToString();
            ItemInfosUI[0].ItemDisplayImage.sprite = itemData.ItemImagePrev;
            ItemInfosUI[0].ItemName.text = itemData.ItemNameText;
            ItemInfosUI[0].ItemRarity.text = itemData.ItemRarity.ToString();
            ItemInfosUI[0].ItemSellPrice.text = itemData.SellPrice.ToString();
            ItemInfosUI[0].ItemType.text = itemData.itemType.ToString();
        }
        else
        {
            Debug.Log("ItemData is null");
        }
    }
    public async Task ReloadAllItemInfos()
    {
        while (playerControl.instance == null)
        {
            await Task.Yield();
        }

        // Removes all Items that are set as a child for the body part
        foreach (var x in playerControl.instance.playerBodyParts.playerBodyParts)
        {
            for (int i = 0; i < x.gameObject.transform.childCount; i++)
            {
                Destroy(x.gameObject.transform.GetChild(i).gameObject);
            }
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
                Slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "0";
                Slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "null";
                Slot.transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
        List<SlotSaveData> saveDatas = new List<SlotSaveData>();
        saveDatas.Clear();
        for (int i = 0; i < InvSlots.Count; i++)
        {
            SlotSaveData X = new SlotSaveData();
            X.itemCount = InvSlots[i].inventorySlot.ItemCount;
            if (InvSlots[i].inventorySlot.ItemStored != null)
            {
                X.ItemName = InvSlots[i].inventorySlot.ItemStored.ItemNameText;
            }
            else
            {
                X.ItemName = "null";
            }
            X.slotName = InvSlots[i].inventorySlot.SlotName;
            X.slotNumber = InvSlots[i].inventorySlot.SlotNumber;

            saveDatas.Add(X);
        }
        SaveManager.instance.dataSOs.inventorDataSO.SaveFromInventory(saveDatas);
    }
    public async Task AddItem(ItemData item, InventorySlot slot, int? ItemCount)
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
                            await ReloadAllItemInfos();
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
                    Debug.LogWarning($"There is no Item slot for this item type: {item.itemType}");
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
                await ReloadAllItemInfos();
                return;
            }
            else if (Inv.ItemCount > 0 && Inv.ItemStored.ItemNameText == item.ItemNameText) // increasing the item count and refreshing
            {
                Inv.ItemCount += 1;
                await ReloadAllItemInfos();
                return;
            }
            else
            {
                Debug.Log("There Is no free Item Space");
            }
        }
    }
    public async Task RemoveItem(ItemData item, int ItemCount, int? InvSlot = null)
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
                    await ReloadAllItemInfos();
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
                await ReloadAllItemInfos();

                Debug.Log($"Removed Items");
                return;
            }
        }
        Debug.Log("There Was no item found to remove");
    }

    public async Task MoveItem(InventorySlot inventorySlot, int InvSlot)
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
            await ReloadAllItemInfos();
            InvSlots[InvSlot].ItemSlot.transform.GetChild(0).gameObject.SetActive(false);
            MovingItem = true;
            float ElapsedTime = 0;
            bool VerifiedItemSpace = false;
            currentInvSlotMovingItem = null;
            while (currentInvSlotMovingItem == null && !VerifiedItemSpace)
            {
                MouseItem.transform.position = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
                ElapsedTime += Time.deltaTime;
                await Task.Yield();
                if (currentInvSlotMovingItem != null)
                {
                    int itemSpace = currentInvSlotMovingItem.MaxItems - currentInvSlotMovingItem.ItemCount;
                    if (itemSpace >= ItemMovingCount)
                    {
                        VerifiedItemSpace = true;
                    }
                    else
                    {
                        VerifiedItemSpace = false;
                        currentInvSlotMovingItem = null;
                    }
                }
            }
            ;
            MovingItem = false;
            InvSlots[InvSlot].ItemSlot.transform.GetChild(0).gameObject.SetActive(true);

            await RemoveItem(item, -ItemMovingCount, InvSlot);
            await AddItem(item, currentInvSlotMovingItem, ItemMovingCount);
            await ReloadAllItemInfos();
            MouseItem.SetActive(false);
            CheckBodyPartItemObjects();
        }
        else
        {
            Debug.LogError("Ended no item selected");
        }
        currentInvSlotMovingItem = null;
        MouseItemPrefab.GetComponent<Image>().sprite = null;
        MovingItem = false;
    }

    async Task Update()
    {
        if (Add)
        {
            Debug.Log("add");
            await AddItem(ItemDatas[Random.Range(0, ItemDatas.Count)], InvSlots[Random.Range(0, InvSlots.Count)].inventorySlot, 1);
            Add = false;
        }
        if (Remove)
        {
            await RemoveItem(ItemDatas[1], -1, null);
            Remove = false;
        }
    }

    void CheckBodyPartItemObjects()
    {
        foreach (var x in playerControl.instance.playerBodyParts.playerBodyParts)
        {
            for (int i = 0; i < x.gameObject.transform.childCount; i++)
            {
                var ItemReference = x.gameObject.transform.GetChild(i).GetComponent<ItemHolder>().ItemReference;
                if (ItemReference != null && ItemReference.playerBodyPartType != x.playerBodyPartTypes)
                {
                    Destroy(x.gameObject.transform.GetChild(i));
                }
            }
            if (x.gameObject.transform.childCount == 0) // when a body part currently doesn't have a Object it checks if there is a Item in that slot
            {
                var Slot = InvSlots.Find(s => s.inventorySlot != null &&
                s.inventorySlot.ItemStored != null &&
                s.inventorySlot.ItemStored.playerBodyPartType == x.playerBodyPartTypes);

                if (Slot != null && Slot.inventorySlot != null && Slot.inventorySlot.ItemStored != null)
                {
                    _ = InvPlayerEquip.instance.EquipItem(Slot.inventorySlot.ItemStored);
                }
            }
        }
    }




    public async Task RemoveHandItem(int Amount)
    {
        HandSlot.ItemCount += Amount;
        HandSlot.ItemStored = null;
        await ReloadAllItemInfos();
    }

    public void LoadData(SaveManager manager)
    {

        for (int i = 0; i < manager.dataSOs.inventorDataSO.InventoryDatas.Count; i++)
        {
            SlotSaveData SavedSlot = manager.dataSOs.inventorDataSO.InventoryDatas[i];
            var foundInfo = InvSlots.Find(InventorySlot => InventorySlot.inventorySlot.SlotNumber == SavedSlot.slotNumber);
            if (foundInfo != null)
            {
                InventorySlot slot = foundInfo.inventorySlot;
                if (slot != null)
                {
                    ItemData StoredItem = ItemDatas.Find(ItemData => ItemData.name == SavedSlot.ItemName);
                    if (StoredItem != null)
                    {
                        slot.ItemCount = SavedSlot.itemCount;
                        slot.ItemStored = StoredItem;
                        slot.itemType = StoredItem.itemType;
                    }
                }
            }
        }
        ReloadAllItemInfos();
    }

    public void SaveData(SaveManager manager)
    {
        List<SlotSaveData> saveDatas = new List<SlotSaveData>();
        saveDatas.Clear();
        for (int i = 0; i < InvSlots.Count; i++)
        {
            SlotSaveData X = new SlotSaveData();
            X.itemCount = InvSlots[i].inventorySlot.ItemCount;
            if (InvSlots[i].inventorySlot.ItemStored != null)
            {
                X.ItemName = InvSlots[i].inventorySlot.ItemStored.ItemNameText;
            }
            else
            {
                X.ItemName = "null";
            }
            X.slotName = InvSlots[i].inventorySlot.SlotName;
            X.slotNumber = InvSlots[i].inventorySlot.SlotNumber;

            saveDatas.Add(X);
        }
        SaveManager.instance.dataSOs.inventorDataSO.SaveFromInventory(saveDatas);
    }
}



[System.Serializable]
public class ItemSlotSaveData
{
    public int slotIndex;
    public string itemID;
    public int ItemAmount;
}

public enum ItemType
{
    AllItems,
    Head,
    Chest,
    Leg,
    Feet
}
