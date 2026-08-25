using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogSystem;

public class SpaceCrashManager : MonoBehaviour
{
    [SerializeField] private float Time = 9;
    [SerializeField] private string NextScene;
    [SerializeField] private string AudioName;
    [SerializeField] private DialogFile dialogFile;
    [SerializeField] private DialogElement dialogElement;
    void Start()
    {
        PlayAudio();
        DialogueGraphManager.PlayNode(dialogFile, dialogElement);
    }
    async Task PlayAudio()
    {
        AudioManager.instance.PlayAudio(AudioName, transform,Vector2.zero, 0, true, 4);
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
