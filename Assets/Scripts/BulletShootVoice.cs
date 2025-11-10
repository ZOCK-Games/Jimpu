using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BulletShootVoice : MonoBehaviour
{
    public GameObject Bullet;
    public Transform BulletContainer;
    void Start()
    {
        StartCoroutine(Shoot(new Vector3(1000, 0, 0), Bullet, new Vector3(0, 0, 0), 10f));
    }

    // Update is called once per frame
    IEnumerator Shoot(Vector3 Target, GameObject Bullet, Vector3 ShotingPosition, float BulletSpeed)
    {
        yield return new WaitForSeconds(4f);
        ///
        /// This Part moves the Bullet and rotates it
        float duration = Vector3.Distance(ShotingPosition, Target) / BulletSpeed;
        float elapsedTime = 0f;
        Debug.Log($"Duration: {duration}");
        GameObject BulletObj = Instantiate(Bullet);
        BulletObj.transform.SetParent(BulletContainer);
        BulletObj.transform.position = ShotingPosition;
        while (elapsedTime < duration && Bullet)
        {
            BulletObj.transform.Rotate(Vector3.up, (elapsedTime * 30) * Time.deltaTime, Space.World);
            BulletObj.transform.Rotate(Vector3.left, (elapsedTime * 30) * Time.deltaTime, Space.World);
            BulletObj.transform.position = Vector3.Lerp(ShotingPosition, Target, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (Bullet)
        {
            BulletObj.transform.position = Target;
            Destroy(BulletObj, 20f);
        }
    }

}
