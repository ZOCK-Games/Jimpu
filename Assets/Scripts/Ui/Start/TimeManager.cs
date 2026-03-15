using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class TimeManager : MonoBehaviour
{
    [SerializeField] public string timeData;
    public int Hour;
    public int Minute;

    void OnEnable()
    {
        CheckTime();
    }


    public void CheckTime()
    {
        Debug.Log("Started Checking time");
        timeData = DateTime.Now.TimeOfDay.ToString(@"hh\:mm");
        Hour = DateTime.Now.Hour;
        Minute = DateTime.Now.Minute;
        Debug.Log("New Time:" + timeData);
        StartCoroutine(TimeChecker());
    }
    IEnumerator TimeChecker()
    {
        yield return new WaitForSeconds(60);
       CheckTime();
    }
}
