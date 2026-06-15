using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToScene : MonoBehaviour
{
    public bool OnlyScript;
    public string SceneToGoTo;
    public Button Button;
    public bool BlockInput;
    private InputSystem_Actions inputActions;
    void OnEnable()
    {
        if (OnlyScript) return;
        if (Button == null)
        {
            return;
        }
        inputActions = new InputSystem_Actions();
        inputActions.UI.Enable();
        if (!BlockInput)
        {
            inputActions.UI.Close.performed += ctx => GoToSceneAsync(SceneToGoTo);
        }
    }
    void OnDisable()
    {
        if (OnlyScript) return;
        if (Button == null)
        {
            return;
        }
        inputActions.UI.Disable();

    }
    void Start()
    {
        if (OnlyScript) return;
        if (Button == null)
        {
            return;
        }
        Button.onClick.AddListener(() => GoToSceneAsync(SceneToGoTo));
    }
    public void GoToSceneAsync(string SceneName)
    {
        _ = GoToSceneAsyncExecute(SceneName);
    }
    private async Task GoToSceneAsyncExecute(string SceneName, bool PlayAnimation = true)
    {
        await SceneManager.LoadSceneAsync("LoadScene", LoadSceneMode.Additive);
        var async = SceneManager.LoadSceneAsync(SceneName);

        async.allowSceneActivation = false;

        await Task.Delay(2000);
        async.allowSceneActivation = true;


        while (!async.isDone)
            await Task.Yield();

        await SceneManager.UnloadSceneAsync("LoadScene");
    }
}
