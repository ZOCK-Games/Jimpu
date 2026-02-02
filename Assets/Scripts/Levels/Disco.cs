using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Disco : MonoBehaviour
{
    [SerializeField] private GameObject LightsParent;
    private Color[] LightColor = new Color[]
    {
        Color.purple,
        Color.white,
        Color.yellow,
        Color.skyBlue,
        Color.softRed
    };
    void Start()
    {
        StartCoroutine(ColorChanging());
    }
    IEnumerator ColorChanging()
    {
        for (int i = 0; i < LightsParent.transform.childCount; i++)
        {
            Light2D DiscoLight = LightsParent.transform.GetChild(i).gameObject.GetComponent<Light2D>();
            int colorInt = Random.Range(0, LightColor.Length);
            DiscoLight.color = LightColor[colorInt];
            float intensityBevore = DiscoLight.intensity;
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
