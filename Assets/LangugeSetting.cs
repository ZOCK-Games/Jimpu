using System;
using System.Collections;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class LangugeSetting : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        dropdown.onValueChanged.AddListener((index) => { StartCoroutine(CheckLanguage(index)); });

        string localeCode = LocalizationSettings.SelectedLocale.Identifier.Code;
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
    }

    // Update is called once per frame
    IEnumerator CheckLanguage(int LanguageNumber)
    {
        string localeCode = "en";
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
        }
        else
        {
            Debug.Log($"Error Selecting new Language");
        }

    }
}
