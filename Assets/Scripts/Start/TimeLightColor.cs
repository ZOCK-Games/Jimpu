using TMPro;
using UnityEngine;

public class TimeLightColor : MonoBehaviour
{
    public TimeManager timeManager;
    public float Light;

    void Update()
    {
        int h = timeManager.Hour;
        int m = timeManager.Minute;


        Light = ((h + m / 60f) / 24f ) *2 ;
        
        if (Light >= 1)
        {
            Light = Light - ((h + m / 60f) / 24f );
        }
    }
}
