using System.Collections;
using UnityEngine;

public class PlayerColisionDetector : MonoBehaviour
{
    [SerializeField] private PlayerControll playerControll;
    [SerializeField] private float DamageCountdown;
    private bool IsDamagebel;
    void Start()
    {
        IsDamagebel = true;
    }
    
    IEnumerator IsDamagebelCountdown()
    {
        IsDamagebel = false;
        yield return new WaitForSeconds(DamageCountdown);
        IsDamagebel = true;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet" && IsDamagebel)
        {
            playerControll.PlayerHealth -= 1;
            StartCoroutine(IsDamagebelCountdown());
        }
    }
}
