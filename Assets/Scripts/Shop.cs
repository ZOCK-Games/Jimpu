using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<Button> Items_to_select;
    public List<ItemData> Items; //Items in the shop
    public List<Image> ItemImagePreview;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemPrice;
    public TextMeshProUGUI ItemInfos;
    public TextMeshProUGUI ShopMassage;
    public Button BuyButton;
    public int AktuelItem;
    public GameData gameData;
    public Inventory Inventory;
    public int Price;
    public GameObject ShopGameObjekt; // The shop game object that is activated when the player opens the shop
    private int i;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Items.Count; i++)
        Items_to_select[i].onClick.AddListener(Selected_item);
        Price = 0;
        BuyButton.onClick.AddListener(Buy_Button);

        AktuelItem = -1;
        ShopGameObjekt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Selected_item()                  // This function is called when the Player selects an item in the shop
    {
        ShopMassage.text = "You have selected an item!";
        ItemImagePreview[i].sprite = Items[i].ItemImagePrev1;
        ItemName.SetText(Items[i].ItemNameText);
        Price = Items[i].ItemPriceInt;
        ItemPrice.SetText(Price + "$");
        ItemInfos.SetText(Items[i].ItemInfosText);
        AktuelItem = i;
    }
    public void Buy_Button() // This function is called when the Player clicks the buy button in the shop
    {
        if (gameData.CoinValue >= Price)
        {
            Debug.Log("Das Aktuelle Item Ist: " + AktuelItem);

            ShopMassage.text = "You have sucesfully bought an item!";
            Inventory.CurrentItemIndex = AktuelItem;

        }
        else // If the Player does not have enough money to buy the item or an error occurs this message will be displayed
        {
            ShopMassage.text = "You don't have enough money! or an error occured!";
        }


    }

}