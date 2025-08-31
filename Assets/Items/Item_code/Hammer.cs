using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Hammer : MonoBehaviour
{
    public GameObject HamerObjekt;
    public GameObject EnemyContainer;
    public Animator ItemAnimator;
    public int Demage = 1;
    private bool CanAttack;
    private int currentenemy;
    public Inventory inventory;
    public GameObject ExplosionPrefab;
    public TilemapCollider2D GroundCollider;
    void Start()
    {
        CanAttack = true;
    }
    void Update()
    {
        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {     // Plays the attack hit animation when enemy is in range and the player presses E
            if (HamerObjekt.GetComponent<BoxCollider2D>().IsTouching(EnemyContainer.transform.GetChild(i).GetComponent<PolygonCollider2D>()) && CanAttack && Input.GetKey(KeyCode.E))
            {
                inventory.ClearCurentItem = true;
                Debug.Log("There was an Enemy in the radius of the hammer!");
                currentenemy = i;
                StartCoroutine(inaktive());

            }

            else if (Input.GetKey(KeyCode.E)) // Plays the attack animation but dose not hit
            {
                ItemAnimator.SetTrigger("HammerUse");
                Debug.Log("There was no Enemy in the radius of the hammer!");
            }
        }
        

    }
    public IEnumerator inaktive()
    {
        float Posy = HamerObjekt.transform.position.y;
        float Posx = HamerObjekt.transform.position.x;
        Vector3 ExplosionPos = new Vector3(Posx, Posy -0.5f, 0);
        GameObject explosion = Instantiate(ExplosionPrefab);
        if (!ExplosionPrefab.GetComponent<BoxCollider2D>().IsTouching(GroundCollider))
        {
            for (float i = ExplosionPos.y; i > ExplosionPos.y -1; i -= 0.25f)
            {
                ExplosionPos.y = i;
                if (ExplosionPrefab.GetComponent<BoxCollider2D>().IsTouching(GroundCollider))
                {
                    explosion.transform.position = ExplosionPos;
                    break;
                }
            }
        }
        explosion.GetComponent<Animator>().SetTrigger("AttackHit");
        ItemAnimator.SetTrigger("HammerHit");
        CanAttack = false;
        yield return new WaitForSeconds(0.4f);
        EnemyContainer.transform.GetChild(currentenemy).GetComponent<EnemyInfo>().EnemyHealt -= 1;
        currentenemy = -1;
        yield return new WaitForSeconds(0.6f);
        Destroy(explosion);
        CanAttack = true;
        HamerObjekt.SetActive(false);
    }

}
