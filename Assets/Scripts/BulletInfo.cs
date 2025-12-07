using Unity.VisualScripting;
using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    public bool DuplicateOnWallHit;
    public BulletShootVoice bulletShootVoice;
    void Start()
    {
        Destroy(gameObject, 5);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("Destroying OBj");
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            if (DuplicateOnWallHit)
            {
                StartCoroutine(bulletShootVoice.Shoot(gameObject.transform, bulletShootVoice.Bullets[0], bulletShootVoice.ShotingPositionShooter, 2, 1, 0, false));
            }
            Destroy(gameObject, 0.5f);
        }
    }
}
