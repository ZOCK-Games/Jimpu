using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class TimeManager : MonoBehaviour
{
    private readonly string apiUrl = "https://worldtimeapi.org/api/ip";

    [SerializeField] public string timeData;
    public int Hour;
    public int Minute;

    void Start()
    {
        StartCoroutine(CheckTime());

    }


    IEnumerator CheckTime()
    {
        Debug.Log("Started Checking time");
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API Error: " + request.error);
                timeData = DateTime.Now.TimeOfDay.ToString(@"hh\:mm");
                Hour = DateTime.Now.Hour;
                Minute = DateTime.Now.Minute;
                Debug.Log("New Time:" + timeData);
                StartCoroutine(TimeChecker());
            }
            else
            {
                try
                {
                    TimeApiResponse response = JsonUtility.FromJson<TimeApiResponse>(request.downloadHandler.text);

                    DateTime dateTime = DateTime.Parse(response.datetime);

                    // Nur Uhrzeit (HH:mm)
                    timeData = dateTime.ToString("HH:mm");
                    Hour = int.Parse(dateTime.ToString("HH"));
                    Minute = int.Parse(dateTime.ToString("mm"));
                    Debug.Log("Current Time: " + timeData);
                    Debug.Log("New Time:" + timeData);
                    StartCoroutine(TimeChecker());
                }
                catch (Exception e)
                {
                    Debug.LogError("Parsing Error: " + e.Message);
                    timeData = DateTime.Now.TimeOfDay.ToString(@"hh\:mm");
                    Hour = DateTime.Now.Hour;
                    Minute = DateTime.Now.Minute;
                    StartCoroutine(TimeChecker());
                }
            }
        }

    }
    IEnumerator TimeChecker()
    {
        yield return new WaitForSeconds(60);
        StartCoroutine(CheckTime());
    }
    [Serializable]
    public class TimeApiResponse
    {
        public string datetime;

    }
}
