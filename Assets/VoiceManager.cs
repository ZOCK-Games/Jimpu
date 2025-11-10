using System.Collections;
using TMPro;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    [Range(0f, 10f)]
    [SerializeField] private float Mode;
    [SerializeField] private Animator VoiceAnimator;
    [SerializeField] private TextMeshProUGUI TextField;
    [SerializeField] private PlayerControll playerControll;

    void Start()
    {
    }
    IEnumerator WalkLeftWaiting()
    {
        VoiceAnimator.Play("Hello");
        VoiceAnimator.SetTrigger("InAll");
        TextField.text = "Hello Player pleas Walk to the left! :)";
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.anyKey);
        yield return new WaitForSeconds(0.5f);
        TextField.text = "If you haven't heard me i SAID PLEAS MOVE LEFT NOW!";
    }
    void Update()
    {
        if (playerControll.rb.linearVelocityX > 0.3f)
        {
            StopCoroutine(WalkLeftWaiting());
            TextField.text = "I knew you can do it (=";
            VoiceAnimator.SetBool("Hello", false);
        }
    }


}
