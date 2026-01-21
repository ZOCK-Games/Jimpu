using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SocialPlatforms;

public class LangugeSetting : MonoBehaviour, IDataPersitence
{
    public TMP_Dropdown dropdown;
    public string localeCode;
    void Start()
    {
        SaveManager.instance.Load();
        dropdown.onValueChanged.AddListener((index) => { StartCoroutine(CheckLanguage(index)); });
        if (localeCode == null)
        {
            localeCode = LocalizationSettings.SelectedLocale.Identifier.Code;
        }
        LoadLocalCode(localeCode);
    }
    public void LoadLocalCode(string code)
    {
        switch (localeCode)
        {
            case "en":
                dropdown.value = 0;
                break;
            case "de":
                dropdown.value = 1;
                break;
            case "fr":
                dropdown.value = 2;
                break;
        }
        CheckLanguage(dropdown.value);
    }
    IEnumerator CheckLanguage(int LanguageNumber)
    {
        localeCode = "en";
        switch (LanguageNumber)
        {
            case 0:
                localeCode = "en";
                break;
            case 1:
                localeCode = "de";
                break;
            case 2:
                localeCode = "fr";
                break;
        }
        yield return LocalizationSettings.InitializationOperation;
        Locale desiredLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        if (desiredLocale != null)
        {
            LocalizationSettings.SelectedLocale = desiredLocale;
            Debug.Log($"New Language: {desiredLocale.name}");
            SaveManager.instance.Save();
        }
        else
        {
            Debug.Log($"Error Selecting new Language");
        }

    }

    public void LoadData(SaveManager manager)
    {
        localeCode = manager.userSettingsSO.Language;
    }

    public void SaveData(SaveManager manager)
    {
        if (localeCode != null)
        {
            manager.userSettingsSO.Language = localeCode;
        }
    }
}