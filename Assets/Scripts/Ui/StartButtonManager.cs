using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// Creates the GameObject for the game 
/// (Player, Player Hud etc)
/// </summary>
public class StartButtonManager : MonoBehaviour
{
    public Button StartButton;
    public string StartSceneName;
    [SerializeField] private GameObject TransitionUi;

    private void Awake()
    {
        TransitionUi.SetActive(false);
        StartButton.onClick.AddListener(() =>
            {
                StartCoroutine(ExecutingStart());
            }
            );
    }
    void Start()
    {
        StartSceneName = SceneInfoManager.instance.LastGameScene;
    }

    public IEnumerator ExecutingStart() //is executed when StartButton is being clicked
    {
        TransitionUi.SetActive(true);
        Animator animator = TransitionUi.GetComponent<Animator>();
        Debug.Log("Start Button Clicked Load Scene: " + StartSceneName);
        AsyncOperation operation = SceneManager.LoadSceneAsync(StartSceneName);
        operation.allowSceneActivation = false;

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float clipLenght = clipInfo[0].clip.length;
        if (clipInfo.Length == 0)
        {
            Debug.LogWarning("No animation clip found!");
            TransitionUi.SetActive(false);
            operation.allowSceneActivation = true;
            yield break;

        }
        else
        {
            yield return new WaitForSeconds(clipLenght);
            CreateObjects();
            operation.allowSceneActivation = true;
        }
    }

    private void CreateObjects()
    {

    }
}
