using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeLightColor : MonoBehaviour
{
    public TimeManager timeManager;
    public float Light;
    [SerializeField] List<Light2D> Lights;

    void Update()
    {

        int h = timeManager.Hour;
        int m = timeManager.Minute;


        float LightAmount = ((h + m / 60f) / 24f) *2;
        Light = LightAmount;

        if (Light >= 1)
        {
            Light = 1 - (LightAmount / 2);
        }

        for (int i = 0; i < Lights.Count; i++)
        {
            Lights[i].intensity = Light;
        }
    }
}
