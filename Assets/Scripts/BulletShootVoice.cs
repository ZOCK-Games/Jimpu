using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BulletShootVoice : MonoBehaviour
{
    public List<GameObject> Bullets;
    public Transform BulletContainer;
    void Start()
    {
    }

    // Update is called once per frame
    public IEnumerator Shoot(Transform Target, GameObject Bullet, Transform ShotingPosition, float BulletSpeed, int BulletCount, float BulletShotCountdown)
    {
        Debug.Log($"Summoning {BulletCount} Bullets");
        for (int i = 0; i < BulletCount; i++)
        {
            Vector3 TargetPos = Target.position;
            Vector3 ShotingPos = ShotingPosition.position;

            float duration = Vector3.Distance(ShotingPos, TargetPos) / BulletSpeed;
            float elapsedTime = 0f;
            Debug.Log($"Duration: {duration}");
            GameObject BulletObj = Instantiate(Bullet);
            BulletObj.transform.SetParent(BulletContainer);
            BulletObj.transform.position = ShotingPos;
            while (elapsedTime < duration && BulletObj != null)
            {
                BulletObj.transform.Rotate(Vector3.up, (elapsedTime * 30) * Time.deltaTime, Space.World);
                BulletObj.transform.Rotate(Vector3.left, (elapsedTime * 30) * Time.deltaTime, Space.World);
                BulletObj.transform.position = Vector3.Lerp(ShotingPos, TargetPos, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if (BulletObj != null)
            {
                BulletObj.transform.position = TargetPos;
                Destroy(BulletObj, 20f);
            }
            yield return new WaitForSeconds(BulletShotCountdown);
        }
    }

}
