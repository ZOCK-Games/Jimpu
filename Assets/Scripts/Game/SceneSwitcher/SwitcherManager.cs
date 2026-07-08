using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SwitcherManager
{
    private static String CurrentID;
    public static async Task MoveToScenePoint(string SceneName, string ID, float Delay = 0)
    {
        CurrentID = ID;
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);

        operation.allowSceneActivation = false;

        int delayTime = (int)(Delay * 1000);
        await Task.Delay(delayTime);

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        OnSceneLoaded();

    }

    private static void OnSceneLoaded()
    {
        Debug.LogWarning("Onnnnnnnnnnnnnnnnn SCene loaded");
        Switcher switcher = UnityEngine.Object.FindObjectsByType<Switcher>().FirstOrDefault(x => x.SwitcherID == CurrentID);

        if (switcher != null && playerControl.instance != null)
        {
            playerControl.instance.gameObject.transform.position = switcher.PlayerTeleportPosition.position;
        }

        else
        {
            Debug.LogError($"The Requested Switcher Point ({CurrentID}) was not found");
        }
    }
}
