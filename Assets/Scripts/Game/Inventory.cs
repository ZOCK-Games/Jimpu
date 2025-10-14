using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IDataPersitence
{
    public List<ItemData> itemData;
    public List<GameObject> Item;
    public string CurentItem;
    public int CurentItemData;

    public void Start()
    {
        for (int i = 0; i < Item.Count; i++)
            if (Item[i].name.ToString() == CurentItem)
            {
                CurentItemData = i;
                Item[CurentItemData].SetActive(true);
                CurentItem = Item[CurentItemData].name;
            }
            else
            {
                Item[i].SetActive(false);

            }
    }
    void Update()
    {
        for (int i = 0; i < Item.Count; i++)
            if (Item[i].activeSelf)
            {
                CurentItem = Item[i].name;
            }


    }
    public void Clear()
    {
            CurentItem = null;
            for (int i = 0; i < Item.Count; i++)
            {
                Item[i].SetActive(false);
            }
            DataPersitenceManger.Instance.SaveGame();

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

