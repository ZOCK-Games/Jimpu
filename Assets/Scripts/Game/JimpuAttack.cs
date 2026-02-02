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

        }

        ////////////////////////////
        ///  Checks if the Bullet hits a wall or the player
        ////////////////////////////

        for (int i = 0; i < BulletContainer.transform.childCount; i++)
        {
            GameObject Bullet = BulletContainer.transform.GetChild(i).gameObject;
            EnemyInfo enemyInfo = Bullet.GetComponent<EnemyInfo>();


        }
    }
    public IEnumerator ShotBullet(EnemyInfo enemyInfo, Transform Target)
    {


        Debug.Log("Executing ShotBullet");

        GameObject Bullet = Instantiate(BulletPrefab);
        Bullet.transform.SetParent(BulletContainer.transform);
        Bullet.GetComponent<BulletObj>().enemyInfo = enemyInfo;
        Bullet.name = Bullet + BulletContainer.transform.childCount.ToString();
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

    }

}

