using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SunMovingStart : MonoBehaviour
{
    public GameObject SunObject;
    public TimeManager timeManager;
    public SpriteRenderer SkySprite;
    public List<Light2D> Lights2d;
    public float LightMultiply;
    private float TimeUser;
    public float Hour;
    public float Minute;
    public GameObject PointA;
    public GameObject PointB;
    public Gradient SunGradient;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TimeUser = ((Minute / 60f) + Hour) / 24f;
        SunObject.transform.position = Vector3.Slerp(PointA.transform.position, PointB.transform.position, TimeUser);

        float smoothIntensity = Mathf.Sin((TimeUser * 2f * Mathf.PI) - Mathf.PI / 2f);
        smoothIntensity = (smoothIntensity + 1f) / 2f;
        for (int i = 0; i < Lights2d.Count; i++)
        {
            Lights2d[i].intensity = smoothIntensity * LightMultiply;
        }

        Color c = SunGradient.Evaluate(TimeUser);
        SkySprite.color = c;
        Hour = timeManager.Hour;
        Minute = timeManager.Minute;
    }
    void OnDrawGizmos()
    {
        Vector3 lastPoint = PointA.transform.position;
        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;
            Vector3 nextPoint = Vector3.Slerp(PointA.transform.position, PointB.transform.position, t);

            Gizmos.DrawLine(lastPoint, nextPoint);
            lastPoint = nextPoint;
        }
    }
}
