using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public GameObject HamerObjekt;
    public BoxCollider2D HamerCollider;
    public Animator HammerAnimation;
    public List<BoxCollider2D> Enemy;
    public EnemyScript enemy_Scribt;
    public int Demage = 1;
    private int i;
    private bool CanAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HammerAnimation.speed = 0;
        CanAttack = true;
    }

    // Update is called once per frame
    void OnTriggerEnter2D()
    {
        if (HamerCollider.IsTouching(Enemy[0]) && CanAttack == true && Input.GetKey(KeyCode.E))
        {
            enemy_Scribt.EnemyHealt -= Demage;
            StartCoroutine(inaktive());
        }
        else
        {
        }

    }
    public IEnumerator inaktive()
    {
        HammerAnimation.speed = 2;
        CanAttack = false;
        yield return new WaitForSeconds(0.4f);
        HamerObjekt.SetActive(false);
        HammerAnimation.speed = 0;
    }

}
