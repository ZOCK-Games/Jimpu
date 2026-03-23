using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceCrashManager : MonoBehaviour
{
    [SerializeField] private float Time = 9;
    [SerializeField] private string NextScene;
    [SerializeField] private string AudioName;
    void Start()
    {
        PlayAudio();
    }
    async Task PlayAudio()
    {
        await Task.Delay((int)(0.3f * 1000));
        AudioManager.instance.PlayAudio(AudioName, transform, true, 4);
        int DelayTime = (int)(Time * 1000);
        AsyncOperation operation = SceneManager.LoadSceneAsync(NextScene);
        operation.allowSceneActivation = false;
        
        await Task.Delay(DelayTime);
        while(operation.progress < 0.9f)
        {
            await Task.Delay(50);
        }

        operation.allowSceneActivation = true;
    }
}
