using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorDataSO", menuName = "Data/InventorDataSO")]
public class InventorDataSO : ScriptableObject
{
    public List<SlotSaveData> InventoryDatas = new List<SlotSaveData>();

    public void SaveFromInventory(List<SlotSaveData> slots)
    {
        InventoryDatas.Clear();
        foreach (var slot in slots)
        {
            InventoryDatas.Add(slot);
        }
    }
}