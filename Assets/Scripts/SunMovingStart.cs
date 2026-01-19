using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SunMovingStart : MonoBehaviour
{
    public GameObject SunObject;
    public TimeManager timeManager;
    public SpriteRenderer SkySprite;
    public Light2D light2D;
    public float TimeUser;
    public float Hour;
    public float Minute;
    public GameObject PointA;
    public GameObject PointB;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TimeUser = ((Minute / 60f) + Hour) / 24f;
        SunObject.transform.position = Vector3.Slerp(PointA.transform.position, PointB.transform.position, TimeUser);
        float UpTimeStart = 0.15f;
        float UpTineDone = 0.19f;
        if (TimeUser >= UpTimeStart && TimeUser <= UpTineDone)
        {
            float t = (TimeUser - UpTimeStart) / (UpTineDone - UpTimeStart);
            SkySprite.color = Color.Lerp(Color.red, Color.lightBlue, t);
        }

        float DownTimeStart = 0.7f;
        float DownTineDone = 0.74f;
        if (TimeUser >= DownTimeStart && TimeUser <= DownTineDone)
        {
            float t = (TimeUser - DownTimeStart) / (DownTineDone - DownTimeStart);
            SkySprite.color = Color.Lerp(Color.lightBlue, Color.red, t);
        }
        float NightTimeStart = 0.741f;
        float NightTineDone = 0.75f;
        if (TimeUser >= NightTimeStart && TimeUser <= NightTineDone)
        {
            float t = (TimeUser - NightTimeStart) / (NightTineDone - NightTimeStart);
            SkySprite.color = Color.Lerp(Color.red, Color.black, t);
        }

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
