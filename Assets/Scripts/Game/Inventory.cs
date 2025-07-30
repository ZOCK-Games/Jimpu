using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemData> itemData;
    public List<GameObject> Item;
    public int CurrentItemIndex;
    public bool ItemIsAktive;
    public void Start()
    {

    }
    void Update()
    {
        for (int i = 0; i < Item.Count; i++)
        {
            if (Item[i].activeSelf)
            {
                Debug.Log("Item " + itemData[i].ItemNameText + " is active.");
                ItemAdd();
            }
        }
    }
    public void ItemAdd()
    {
        Item[CurrentItemIndex].SetActive(true);
        ItemIsAktive = true;
        Debug.Log("Item Added: " + itemData[CurrentItemIndex].ItemNameText);

    }
}
