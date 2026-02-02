using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CreditsManager : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Animator EndAnimation;
    void Start()
    {
        button.onClick.AddListener(() => StartCoroutine(EndButton()));
    }
    IEnumerator EndButton()
    {
        EndAnimation.Play("tv");
        yield return null;
        AnimatorClipInfo[] Info = EndAnimation.GetCurrentAnimatorClipInfo(0);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Intro");
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(Info[0].clip.length -0.3f);
        operation.allowSceneActivation = true;
    }
}
