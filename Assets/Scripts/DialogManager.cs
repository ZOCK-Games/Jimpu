using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public string DialogFile;
    public CurrentDialogData currentDialogData;
    public TextMeshPro TextDisplay;
    public ActionSystem actionSystem;
    private bool WaitingDone;
    public List<string> QueuedDialogs;
    private int DialogPriority;
    void Start()
    {
        QueuedDialogs = new List<string>(QueuedDialogs);
        if (currentDialogData != null)
        {
            TextDisplay.text = currentDialogData.dialog_text;
        }
        WaitingDone = false;
    }
    public void AddQueueDialog(string Path)
    {
        if (Resources.Load(Path) != null)
        {
            QueuedDialogs.Add(Path);
        }
    }
    public void LoadMultipleDialog(float Time)
    {
        LoadMultipleDialogTask(Time);
    }
    public async Task LoadMultipleDialogTask(float Time)
    {
        List<string> CurentPaths = new List<string>(QueuedDialogs);
        QueuedDialogs.Clear();
        for (int i = 0; i < CurentPaths.Count; i++)
        {
            LoadDialog(CurentPaths[i]);
            WaitingDone = false;
            int DelayTime = (int)(Time * 1000);
            await Task.Delay(DelayTime);
        }
        TextDisplay.text = null;
        DialogPriority = -99;
        DialogFile = null;
    }


    public void LoadDialog(string Path)
    {

        TextAsset jsonTextAsset = Resources.Load<TextAsset>(Path);
        DialogFile = jsonTextAsset.text;
        if (jsonTextAsset != null)
        {
            CovertJson(jsonTextAsset);
            DialogFile = jsonTextAsset.name;
            if (DialogPriority <= currentDialogData.dialog_priority || DialogFile == null)
            {
                    DialogPriority = currentDialogData.dialog_priority;
                    TextDisplay.text = currentDialogData.dialog_text;
            }
        }
    }


    public void CovertJson(TextAsset jsonFile)
    {
        if (jsonFile == null) return;

        string jsonText = jsonFile.text;

        string wrappedJson = "{\"dialogs\":" + jsonText + "}";

        DialogJsonData wrapper = JsonUtility.FromJson<DialogJsonData>(wrappedJson);

        if (wrapper == null || wrapper.dialogs == null || wrapper.dialogs.Length == 0)
        {
            Debug.LogError("Fehler beim Deserialisieren. Überprüfe JSON- und Wrapper-Struktur.");
            return;
        }
        DialogJsonData firstDialog = wrapper.dialogs[0];
        currentDialogData.LoadFromData(firstDialog);

        Debug.Log($"Dialog '{currentDialogData.dialog_id}' erfolgreich geladen. Zeit: {currentDialogData.action_to_do_time}");
    }
    public List<TextAsset> FindPrefabsByName(string folderPath, string searchTerm)
    {
        List<TextAsset> foundObjects = new List<TextAsset>();
        TextAsset[] allTextAsset = Resources.LoadAll<TextAsset>(folderPath);

        foreach (TextAsset TextAssets in allTextAsset)
        {
            if (TextAssets.name.IndexOf(searchTerm, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                foundObjects.Add(TextAssets);
            }
        }
        return foundObjects;
    }
}

[Serializable]
public class DialogJsonData
{
    public DialogJsonData[] dialogs;
    public string dialog_id;
    public bool dialog_ended;
    public string dialog_text;
    public string action;
    public string action_to_do;
    public float action_to_do_time;
    public string next_id_done;
    public string next_id_not_done;
    public int dialog_priority;
}
