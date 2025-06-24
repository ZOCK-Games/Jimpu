using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<Button> Items_to_select;
    public List<ItemData> Items;
    //Items When UI SHop Is active
    public Image ItemImagePreview;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemPrice;
    public TextMeshProUGUI ItemInfos;
    public TextMeshProUGUI ErrorMessage;
    public TextMeshProUGUI BuyMessage;
    public Button BuyButton;
    public int AktuelItem;
    public Coin_scribt Coin_scribt1;
    public Inventory Inventory;
    public int Price;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Items_to_select[0].onClick.AddListener(Selected_items_0);
        Items_to_select[1].onClick.AddListener(Selected_items_1);
        Items_to_select[2].onClick.AddListener(Selected_items_2);
        Items_to_select[3].onClick.AddListener(Selected_items_3);
        Items_to_select[4].onClick.AddListener(Selected_items_4);
        ErrorMessage.enabled = false;
        BuyMessage.enabled = false;

        Price = 0;
        BuyButton.onClick.AddListener(Buy_Button);

        AktuelItem = -1;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Selected_items_0()
    {
        ItemImagePreview.sprite = Items[0].ItemImagePrev1;
        ItemName.SetText(Items[0].ItemNameText);
        Price = Items[0].ItemPriceInt;
        ItemPrice.SetText(Price + "$");
        ItemInfos.SetText(Items[0].ItemInfosText);
        AktuelItem = 0;
    }
    public void Selected_items_1()
    {
        ItemImagePreview.sprite = Items[1].ItemImagePrev1;
        ItemName.SetText(Items[1].ItemNameText);
        Price = Items[1].ItemPriceInt;
        ItemPrice.SetText(Price + "$");
        ItemInfos.SetText(Items[1].ItemInfosText);
        AktuelItem = 1;
    }
    public void Selected_items_2()
    {
        ItemImagePreview.sprite = Items[2].ItemImagePrev1;
        ItemName.SetText(Items[2].ItemNameText);
        Price = Items[2].ItemPriceInt;
        ItemPrice.SetText(Price + "$");
        ItemInfos.SetText(Items[2].ItemInfosText);
        AktuelItem = 2;
    }
    public void Selected_items_3()
    {
        ItemImagePreview.sprite = Items[3].ItemImagePrev1;
        ItemName.SetText(Items[3].ItemNameText);
        Price = Items[3].ItemPriceInt;
        ItemPrice.SetText(Price + "$");
        ItemInfos.SetText(Items[3].ItemInfosText);
        AktuelItem = 3;
    }
    public void Selected_items_4()
    {
        ItemImagePreview.sprite = Items[4].ItemImagePrev1;
        ItemName.SetText(Items[4].ItemNameText);
        Price = Items[4].ItemPriceInt;
        ItemPrice.SetText(Price + "$");
        ItemInfos.SetText(Items[4].ItemInfosText);
        AktuelItem = 4;
    }
    public void Buy_Button()
    {
        if (Coin_scribt1.Coin_Count >= Price)
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