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
    [SerializeField] private BulletShootVoice bulletShootVoice;
    [Header("AttackSettings")]
    public Transform ShootingPosition;
    public float MaxBulletSpeed;
    public float MinBulletSpeed;
    public float MaxBulletShotCountdown;
    public float MinBulletShotCountdown;

    void Start()
    {
        StartCoroutine(WalkLeftWaiting());
    }
    IEnumerator WalkLeftWaiting()
    {
        VoiceAnimator.Play("Hello");
        VoiceAnimator.SetTrigger("InAll");
        TextField.text = "Hello Player pleas Walk to the left! :)";
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.anyKey);
        yield return new WaitForSeconds(0.5f);
        Mode -= 1;
        TextField.text = "If you haven't heard me i SAID PLEAS MOVE LEFT NOW!";
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.anyKey);
        yield return new WaitForSeconds(0.5f);
        TextField.text = "Thats enough You wont make it out alive!";

        StartCoroutine(bulletShootVoice.Shoot(
        playerControll.Player.transform,
        bulletShootVoice.Bullets[0],
        ShootingPosition,
        Random.Range(MinBulletSpeed,
        MaxBulletSpeed),
        10,
        Random.Range(MaxBulletShotCountdown,MinBulletShotCountdown)));

        playerControll.rb.linearVelocityX = 0.31f;

    }
    void Update()
    {
        if (playerControll.rb.linearVelocityX > 0.6f)
        {
            StopCoroutine(WalkLeftWaiting());
            TextField.text = "I knew you can do it (=";
            VoiceAnimator.SetBool("Hello", false);
            VoiceAnimator.SetTrigger("AllOut");
        }

    }

}
