using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeLightColor : MonoBehaviour
{
    public TimeManager timeManager;
    
    public float Light;
    [SerializeField] private List<Light2D> Lights;
    public GameObject SkyObj;
    public GameObject SkyObjNight;
    public Color Color1;
    public float Sky;
    public float SkyNight;
    void Start()
    {
    }

    void Update()
    {
        LightColor();
    }

    public void LightColor()
    {
        Color SkyColor = SkyObj.GetComponent<SpriteRenderer>().color;
        SkyColor.a = Light;
        Sky = SkyColor.a;
        SkyObj.GetComponent<SpriteRenderer>().color = SkyColor;

        int h = timeManager.Hour;
        int m = timeManager.Minute;

        if (h >= 20 || h <= 6)
        {
            Color SkyColorNight = SkyObjNight.GetComponent<SpriteRenderer>().color;
            SkyColorNight.a = Light = 1 - Light;
            SkyNight = SkyColorNight.a;
            SkyObjNight.GetComponent<SpriteRenderer>().color = SkyColorNight;
        }
        else
        {
            Color SkyColorNight = SkyObjNight.GetComponent<SpriteRenderer>().color;
            SkyColorNight.a = Light = 0;
            SkyNight = SkyColorNight.a;
            SkyObjNight.GetComponent<SpriteRenderer>().color = SkyColorNight;
        }


        float LightAmount = (((h + m / 60f) / 24f) * 2) - 0.1f;
        Light = LightAmount;


        if (Light >= 1)
        {
            Light = (1 - (LightAmount / 2)) - 0.1f;
        }
                if (h >= 22 )
        {
            Light = 0;
        }
        if (h <= 4)
        {
            Light = 0;
        }
        for (int i = 0; i < Lights.Count; i++)
        {
            Lights[i].intensity = Light;
        }
        


    }
}
