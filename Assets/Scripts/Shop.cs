using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<Button> Items_to_select;
    public List<ItemData> Items;
    //Items When UI SHop Is active
    public List<Image> ItemImagePreview;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemPrice;
    public TextMeshProUGUI ItemInfos;
    public TextMeshProUGUI ErrorMessage;
    public TextMeshProUGUI BuyMessage;
    public Button BuyButton;
    public int AktuelItem;
    public PlayerStats PlayerStatsScribt;
    public Inventory Inventory;
    public int Price;
    public GameObject ShopGameObjekt;
    private int i;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Items.Count; i++)
            Items_to_select[i].onClick.AddListener(Selected_item);
        ErrorMessage.enabled = false;
        BuyMessage.enabled = false;

        Price = 0;
        BuyButton.onClick.AddListener(Buy_Button);

        AktuelItem = -1;
        ShopGameObjekt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Selected_item()
    {
        ItemImagePreview[i].sprite = Items[i].ItemImagePrev1;
        ItemName.SetText(Items[i].ItemNameText);
        Price = Items[i].ItemPriceInt;
        ItemPrice.SetText(Price + "$");
        ItemInfos.SetText(Items[i].ItemInfosText);
        AktuelItem = i;
    }
    public void Buy_Button()
    {
        if (PlayerStatsScribt.PlayerMoney >= Price)
        {
            Debug.Log(AktuelItem);

            BuyMessage.enabled = true;
            Inventory.Item[AktuelItem].SetActive(true);
            


            Inventory.Item_Counter[AktuelItem] += 1;
        }
        else
        {
            ErrorMessage.enabled = true;
        }


    }

}