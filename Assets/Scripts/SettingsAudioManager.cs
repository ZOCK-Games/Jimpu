using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsAudioManager : MonoBehaviour, IDataPersitence
{
    private Transform Parent;
    private float Music;
    private float Effects;
    private float Environment;
    private float Global;

    void Start()
    {
        Parent = this.transform;
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            Slider currentSlider = Parent.transform.GetChild(i).transform.GetChild(2).GetComponent<Slider>();

            if (currentSlider != null)
            {
                Slider tempSlider = currentSlider;

                tempSlider.onValueChanged.AddListener((float x) =>
                {
                    SaveSliderValue(x, tempSlider);
                });

                EventTrigger trigger = tempSlider.gameObject.GetComponent<EventTrigger>();
                if (trigger == null) trigger = tempSlider.gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerUp;

                entry.callback.AddListener((data) =>
                {
                    SaveManager.instance.Save();
                });
                trigger.triggers.Add(entry);
            }
        }


    }
    public void SaveSliderValue(float value, Slider slider)
    {
        switch (slider.name)
        {
            case "Music":
                Music = value;
                break;
            case "Effects":
                Effects = value;
                break;
            case "Environment":
                Environment = value;
                break;
            case "Global":
                Global = value;
                break;
        }
    }

    public void LoadData(SaveManager manager)
    {
        Music = manager.userSettingsSO.MusicValue;
        Effects = manager.userSettingsSO.EffectsValue;
        Environment = manager.userSettingsSO.EnvironmentValue;
        Global = manager.userSettingsSO.GlobalValue;
    }

    public void SaveData(SaveManager manager)
    {
        manager.userSettingsSO.MusicValue = Music;
        manager.userSettingsSO.EffectsValue = Effects;
        manager.userSettingsSO.EnvironmentValue = Environment;
        manager.userSettingsSO.GlobalValue = Global;
    }
}
