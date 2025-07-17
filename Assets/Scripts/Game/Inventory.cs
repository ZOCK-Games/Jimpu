using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemData> itemData;
    public List<GameObject> InventoryPanel;
    public Button SellButton;
    public Button EquipButton;
    public bool ItemIsAktive;
    public void Start()
    {
        Button InvButton = InventoryPanel[s].GetComponent<Button>();
        InvButton.onClick.AddListener(InvButtonClick);

    }
    void ItemAdd()
    {
        SpriteRenderer sr = InventoryPanel[s].GetComponent<SpriteRenderer>();
        sr.sprite = itemData[d].ItemImagePrev1;
        InventoryPanel[s].name = itemData[d].ItemNameText;
    }
    void InvButtonClick()
    {
        for (int i = 0; i < 10; i++)
            if (InventoryPanel[s].name == itemData[i].ItemNameText)
            {
                if (SellButton.IsActive())
                {
                    Debug.Log("Item Verkauft: " + InventoryPanel[s].name);
                    Sell();
                }
                if (EquipButton.IsActive() && ItemIsAktive == false)
                {
                    Equip();
                }

            }
    }
    void Sell()
    {

    }
    void Equip()
    {

    }

}
