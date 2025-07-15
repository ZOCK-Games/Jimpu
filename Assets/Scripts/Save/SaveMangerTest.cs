using System;
using UnityEngine;
using System.IO;

public class SaveManger : MonoBehaviour
{
    [Serializable]
    public class PlayerData
    {
        public int level = 1;
    }
    public void Save()
    {
        PlayerData data = new PlayerData();
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "playerdata.json";
        File.WriteAllText(path, json);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Save();
        }
    }

}