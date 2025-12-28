using System.Collections;
using UnityEngine;

public class DeviceManger : MonoBehaviour, IDataPersitence
{
    public string Device;
    public string DesktopOS;
    public string SimpleDevice;
    [Header("Device Buttons")]
    public GameObject MoveRButton;
    public GameObject MoveLButton;
    public GameObject MoveUPButton;
    public void Start()
    {
        Device = Application.platform.ToString();
        SimpleDevice = GetSimpleDeviceType();
        if (Device == string.Empty)
        {
            Debug.LogError("Something is wrong with your Device Pleas Contact owner!!! Your Device Is: " + Device);
            Application.OpenURL("https://github.com/ZOCK-Games/Jimpu");
        }
        if (SimpleDevice == "PC")
        {
            DesktopOS = GetDesktopOS();
        }
        StartCoroutine(Wayt());
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

        DataPersitenceManger.Instance.LoadGame();
    }
    public static string GetDesktopOS()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";

            case RuntimePlatform.LinuxPlayer:
            case RuntimePlatform.LinuxEditor:
                return "Linux";

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                return "macOS";

            default:
                return "Other";
        }
    }
    IEnumerator Wayt()
    {
        yield return new WaitForFixedUpdate();
        if (SimpleDevice == "PC")
        {
            MoveLButton.SetActive(false);
            MoveRButton.SetActive(false);
            MoveUPButton.SetActive(false);
            Debug.Log("Current Device is" + SimpleDevice + "Handy movement Disabled");
        }
        if (SimpleDevice == "Console")
        {
            MoveLButton.SetActive(false);
            MoveRButton.SetActive(false);
            MoveUPButton.SetActive(false);
            Debug.Log("Current Device is" + SimpleDevice + "Handy movement Disabled");
        }
        if (SimpleDevice == "Server")
        {
            MoveLButton.SetActive(false);
            MoveRButton.SetActive(false);
            MoveUPButton.SetActive(false);
            Debug.Log("Current Device is" + SimpleDevice + "Handy movement Disabled");
        }
        if (SimpleDevice == "Handy")
        {
            MoveLButton.SetActive(true);
            MoveRButton.SetActive(true);
            MoveUPButton.SetActive(true);
            Debug.Log("Current Device is" + SimpleDevice + "Handy movement enabled");
        }
        if (SimpleDevice == "Unknown")
        {
            MoveLButton.SetActive(false);
            MoveRButton.SetActive(false);
            MoveUPButton.SetActive(false);
            Debug.Log("Current Device is" + SimpleDevice + "Handy movement disabled");

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
