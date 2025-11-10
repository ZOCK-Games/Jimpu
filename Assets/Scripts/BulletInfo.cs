using Unity.VisualScripting;
using UnityEngine;

public class BulletInfo : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("Destroying OBj");
            Destroy(gameObject);
        }
    }
}
