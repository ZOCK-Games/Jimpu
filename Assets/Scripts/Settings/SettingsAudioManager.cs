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
        Music = manager.dataSOs.userSettingsSO.MusicValue;
        Effects = manager.dataSOs.userSettingsSO.EffectsValue;
        Environment = manager.dataSOs.userSettingsSO.EnvironmentValue;
        Global = manager.dataSOs.userSettingsSO.GlobalValue;
    }

    public void SaveData(SaveManager manager)
    {
        manager.dataSOs.userSettingsSO.MusicValue = Music;
        manager.dataSOs.userSettingsSO.EffectsValue = Effects;
        manager.dataSOs.userSettingsSO.EnvironmentValue = Environment;
        manager.dataSOs.userSettingsSO.GlobalValue = Global;
    }
}
