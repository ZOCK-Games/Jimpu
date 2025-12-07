using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public string DialogFile;
    public CurrentDialogData currentDialogData;
    public TextMeshPro TextDisplay;
    public ActionSystem actionSystem;

    void Start()
    {
        if (currentDialogData != null)
        {
            TextDisplay.text = currentDialogData.dialog_text;
        }
    }

    public void LoadDialog(string Path)
    {

        TextAsset jsonTextAsset = Resources.Load<TextAsset>(Path);
        DialogFile = jsonTextAsset.text;
        if (jsonTextAsset != null)
        {
            CovertJson(jsonTextAsset);
        }
        TextDisplay.text = currentDialogData.dialog_text;
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
}