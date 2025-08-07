using System;
using UnityEngine;

public class GetInfo : MonoBehaviour, IDataPersitence
{
    public string Device;
    public string SimpleDevice;
    public void Start()
    {
        Device = Application.platform.ToString();
        SimpleDevice = GetSimpleDeviceType();

        if (Device == string.Empty)
        {
            Debug.LogError("Something is wrong with your Device Pleas Contact owner!!! Your Device Is: " + Device);
            Application.OpenURL("https://github.com/ZOCK-Games/Jimpu");
        }
    }
        string GetSimpleDeviceType() // Used to get the Current device 
        {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                return "Handy";
            case RuntimePlatform.PS4:
            case RuntimePlatform.PS5:
            case RuntimePlatform.XboxOne:
            case RuntimePlatform.GameCoreXboxOne:
            case RuntimePlatform.GameCoreXboxSeries:
            case RuntimePlatform.Switch:
                return "Console";
            case RuntimePlatform.LinuxEditor:
            case RuntimePlatform.LinuxHeadlessSimulation:
            case RuntimePlatform.LinuxPlayer:
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                return "PC";
            case RuntimePlatform.LinuxServer:
            case RuntimePlatform.WindowsServer:
            case RuntimePlatform.OSXServer:
                return "Server";
            default:
                return "Unknown";
        }
    }


    public void SaveGame(ref GameData data)
    {
        data.Device = this.Device;
        data.SimpleDevice = this.SimpleDevice;
}

    public void LoadGame(GameData data)
    {
    }
}
