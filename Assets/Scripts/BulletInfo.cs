using Unity.VisualScripting;
using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("Destroying OBj");
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            Destroy(gameObject,  0.5f);
        }
    }
}
