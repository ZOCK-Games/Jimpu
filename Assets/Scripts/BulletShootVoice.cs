using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BulletShootVoice : MonoBehaviour
{
    public List<GameObject> Bullets;
    public Transform BulletContainer;
    public bool ShootAllsided;
    public PlayerControll playerControll;
    public Transform ShotingPositionShooter;
    public GameObject ParticleShoot;
    public GameObject ParticleContainer;
    public bool ShootBullet;
    void Start()
    {
        StartCoroutine(Shoot(playerControll.Player.transform, Bullets[0], ShotingPositionShooter, 2, 2, 0, true));

    }
    void Update()
    {
        if (ShootBullet)
        {
            ShootBullet = false;
            StartCoroutine(Shoot(playerControll.Player.transform, Bullets[0], ShotingPositionShooter, 2, 2, 0, false));
        }

        if (ShootAllsided)
        {
            ShootAllsided = false;
            Transform transformPos = playerControll.Player.transform;
        }
    }
    public void ShotToRadius(Transform transformPos, GameObject BulletType, Transform ShotingPosition, float BulletSpeed, int BulletCount, float BulletShotCountdown, bool DuplicateOnWallHit)
    {
        List<Vector3> Positions = new List<Vector3>{
            ShotingPosition.up * UnityEngine.Random.Range(1,3),
            ShotingPosition.right * UnityEngine.Random.Range(1,3),
            -ShotingPosition.up * UnityEngine.Random.Range(1,3),
            -ShotingPosition.right * UnityEngine.Random.Range(1,3),
        };
        ;
        for (int i = 0; i < Positions.Count; i++)
        {
            transformPos = ShotingPositionShooter;
            transformPos.position = Positions[i];
            Debug.Log("StartetShotion to " + transformPos);
            StartCoroutine(Shoot(transformPos, BulletType, ShotingPosition, BulletSpeed, BulletCount, BulletShotCountdown, DuplicateOnWallHit));
        }
    }


    // Update is called once per frame
    public IEnumerator Shoot(Transform Target, GameObject Bullet, Transform ShotingPosition, float BulletSpeed, int BulletCount, float BulletShotCountdown, bool DuplicateOnWallHit)
    {
        for (int i = 0; i < BulletCount; i++)
        {
            Vector3 TargetPos = Target.position;
            Vector3 ShotingPos = ShotingPosition.position;

            float duration = Vector3.Distance(ShotingPos, TargetPos) / BulletSpeed;
            float elapsedTime = 0f;
            GameObject BulletObj = Instantiate(Bullet);

            BulletObj.transform.SetParent(BulletContainer);
            BulletObj.transform.position = ShotingPos;
            BulletObj.gameObject.tag = "Bullet";

            GameObject Particle = Instantiate(ParticleShoot);
            Particle.transform.position = BulletObj.transform.position;
            Particle.transform.SetParent(ParticleContainer.transform);
            Particle.name = "Shoot:" + ParticleContainer.transform.childCount;
            Particle.GetComponent<ParticleSystem>().Play();

            BulletObj.GetComponent<BulletInfo>().bulletShootVoice = gameObject.GetComponent<BulletShootVoice>();
            if (DuplicateOnWallHit)
            {
                BulletObj.GetComponent<BulletInfo>().DuplicateOnWallHit = true;
            }
            else
            {
                BulletObj.GetComponent<BulletInfo>().DuplicateOnWallHit = false;
            }
            while (elapsedTime < duration && BulletObj != null)
            {
                if (BulletObj != null && elapsedTime > 0.1f)
                {
                    BulletObj.transform.position = TargetPos;
                    Destroy(BulletObj, 5f);
                }
                BulletObj.transform.Rotate(Vector3.up, (elapsedTime * 30) * Time.deltaTime, Space.World);
                BulletObj.transform.Rotate(Vector3.left, (elapsedTime * 30) * Time.deltaTime, Space.World);
                BulletObj.transform.position = Vector3.Lerp(ShotingPos, TargetPos, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if (Particle != null)
            {
                Destroy(Particle);
            }


            yield return new WaitForSeconds(BulletShotCountdown);
        }
    }

}
