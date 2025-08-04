using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public GameObject HamerObjekt;
    public List<GameObject> Enemy;
    public EnemyScript enemy_Scribt;
    public int Demage = 1;
    private int i;
    private bool CanAttack;
    public Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HamerObjekt.GetComponent<Animation>().Stop();
        CanAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Enemy.Count; i++)
        if (HamerObjekt.GetComponent<BoxCollider2D>().IsTouching(Enemy[i].GetComponent<BoxCollider2D>()) && CanAttack == true && Input.GetKey(KeyCode.E))
        {
            inventory.ClearCurentItem = true;
            enemy_Scribt.EnemyHealt -= Demage;
            StartCoroutine(inaktive());
        }
        else
        {
        }

    }
    public IEnumerator inaktive()
    {
        HamerObjekt.GetComponent<Animation>().Play();
        CanAttack = false;
        yield return new WaitForSeconds(0.4f);
        HamerObjekt.SetActive(false);
        HamerObjekt.GetComponent<Animation>().Stop();
    }

}
