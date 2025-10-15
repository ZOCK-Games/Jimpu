using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class EnemyAttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    public float sichtRadius = 10f;
    public float loadTime = 1f;
    public float decisionTime = 3f;
    public float travelDuration = 3f;
    [SerializeField] private PlayerControll playerControll;
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private GameObject EnemyContainer;
    [SerializeField] private GameObject GridTilemaps;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject BulletContainer;
    [SerializeField] private int MaxBullets;
    public List<Tilemap> tilemaps;
    void Start()
    {
        for (int i = 0; i < GridTilemaps.transform.childCount; i++)
        {
            var Tielamp = GridTilemaps.transform.GetChild(i);
            if (Tielamp.gameObject.tag == "Ground")
            {
                tilemaps.Add(Tielamp.GetComponent<Tilemap>());
            }
        }
    }

    void Update()
    {
        ////////////////////////////
        /// Checks if an Jimpu Wants to shoot
        ////////////////////////////

        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {
            EnemyInfo enemyInfo = EnemyContainer.transform.GetChild(i).GetComponent<EnemyInfo>();
            if (enemyInfo.WantsToAttack && !enemyInfo.IsAttacking && BulletContainer.transform.childCount < MaxBullets)
            {
                enemyInfo.WantsToAttack = false;
                StartCoroutine(ShotBullet(enemyInfo, enemyInfo.Target));
            }

        }

        ////////////////////////////
        ///  Checks if the Bullet hits a wall or the player
        ////////////////////////////

        for (int i = 0; i < BulletContainer.transform.childCount; i++)
        {
            GameObject Bullet = BulletContainer.transform.GetChild(i).gameObject;
            EnemyInfo enemyInfo = Bullet.GetComponent<EnemyInfo>();


            for (int t = 0; t < tilemaps.Count; t++)
            {
                if (Bullet.GetComponent<CircleCollider2D>().IsTouching(tilemaps[t].GetComponent<TilemapCollider2D>()))
                {
                    StartCoroutine(BulletHit(Bullet, enemyInfo));
                }

            }
            if (playerControll.playerCollider.IsTouching(Bullet.GetComponent<CircleCollider2D>()) && Bullet != null && enemyInfo != null)
            {
                StartCoroutine(BulletHit(Bullet, enemyInfo));
            }
        }
    }
    public IEnumerator ShotBullet(EnemyInfo enemyInfo, Transform Target)
    {
        enemyInfo.WantsToAttack = false;
        enemyInfo.IsCharging = false;
        if (enemyInfo.IsAttacking || enemyInfo.EnemyPosition == null) yield break;
        enemyInfo.IsAttacking = true;

        Debug.Log("Executing ShotBullet");

        GameObject Bullet = Instantiate(BulletPrefab);
        Bullet.transform.SetParent(BulletContainer.transform);
        Bullet.GetComponent<BulletObj>().enemyInfo = enemyInfo;
        Bullet.name = Bullet + BulletContainer.transform.childCount.ToString();
        Bullet.transform.position = enemyInfo.EnemyPosition;
        Bullet.GetComponent<Animator>().SetBool("Attack", true);
        Vector3 direction = (Target.position - Bullet.transform.position).normalized;
        float speed = 1f;
        float elapsedTime = 0;
        while (elapsedTime < travelDuration)
        {
            Bullet.transform.position += direction * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(BulletHit(Bullet, enemyInfo));

    }
    public IEnumerator Charging(EnemyInfo enemyInfo)
    {
        if (!enemyInfo.IsCharging)
        {
            Debug.Log("Charging");
            enemyInfo.WantsToAttack = false;
            enemyInfo.IsAttacking = false;
            enemyInfo.IsCharging = true;
            enemyInfo.JimpuAnimator.SetBool("Charge", true);
            yield return new WaitForSeconds(Random.Range(1, 5)); //the time in that the jimpu(enemy) can not shoot 
            enemyInfo.WantsToAttack = true;
        }
    }
    public IEnumerator BulletHit(GameObject Bullet, EnemyInfo enemyInfo)
    {
        enemyInfo.IsAttacking = false;
        enemyInfo.IsCharging = true;
        Debug.Log("Bullet has hit a Obj");
        Animator BulletAniamtor = Bullet.GetComponent<Animator>();
        BulletAniamtor.Play("Hit");
        BulletAniamtor.SetBool("Attack", false);
        enemyInfo.IsAttacking = false;
        yield return new WaitForSeconds(0.8f);
        enemyInfo.IsCharging = false;
        Destroy(Bullet);
        StartCoroutine(Charging(enemyInfo));
    }
}

