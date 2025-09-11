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
    public Color Color1;
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
        SkyObj.GetComponent<SpriteRenderer>().color = SkyColor;

        int h = timeManager.Hour;
        int m = timeManager.Minute;


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
