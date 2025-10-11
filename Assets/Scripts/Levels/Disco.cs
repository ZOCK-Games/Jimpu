using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Disco : MonoBehaviour
{
    [SerializeField] private GameObject LightsParent;
    void Start()
    {
        StartCoroutine(ColorChanging());
    }
    IEnumerator ColorChanging()
    {
        for (int i = 0; i < LightsParent.transform.childCount; i++)
        {
            Light2D DiscoLight = LightsParent.transform.GetChild(i).gameObject.GetComponent<Light2D>();
            int ColorInt = Random.Range(0, 4);
            Color LightColor = Color.purple;
            switch (ColorInt)
            {
                case 0:
                    LightColor = Color.purple;
                    break;
                case 1:
                    LightColor = Color.white;
                    break;
                case 2:
                    LightColor = Color.yellow;
                    break;
                case 3:
                    LightColor = Color.skyBlue;
                    break;
                case 4:
                    LightColor = Color.softRed;
                    break;
            }
            float intensityBevore = DiscoLight.intensity;
            DiscoLight.color = LightColor;
            if (DiscoLight.lightType != Light2D.LightType.Global)
            {
                float intensityLight = Random.Range(0.1f, 0.85f);
                DiscoLight.intensity = Mathf.Lerp(intensityBevore, intensityLight, 0.05f);
            }
        }
        yield return new WaitForSeconds(Random.Range(0.01f, 0.5f));
        StartCoroutine(ColorChanging());
    }
}
