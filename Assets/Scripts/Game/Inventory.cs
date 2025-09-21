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
    public bool ClearCurentItem;

    public void Start()
    {
        for (int i = 0; i < Item.Count; i++)
            if (Item[i].name.ToString() == CurentItem)
            {
                CurentItemData = i;
                Item[CurentItemData].SetActive(true);
                CurentItem = Item[CurentItemData].name;
                Debug.Log("Aktueles Item " + CurentItem + ".  the int is: " + CurentItemData + "The Game Obj is: " + Item[CurentItemData]);
            }
            else
            {
                Item[i].SetActive(false);
                Debug.Log("Kein Item Mit Gleichen namen gefunden Aktueles Item " + CurentItem + ". And the int is: " + CurentItemData);

            }
    }
    void Update()
    {
        if (ClearCurentItem == true)
        {
            Debug.Log("Curent Item Is Cleard item: " + CurentItem);
            CurentItem = null;
            for (int i = 0; i < Item.Count; i++)
            {
                Item[i].SetActive(false);
            }
            ClearCurentItem = false;
            DataPersitenceManger.Instance.SaveGame();
        }
        for (int i = 0; i < Item.Count; i++)
            if (Item[i].activeSelf)
            {
                CurentItem = Item[i].name;
            }


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

