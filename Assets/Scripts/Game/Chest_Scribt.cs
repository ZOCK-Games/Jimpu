using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest_Scribt : MonoBehaviour
{
    public Animator chestaniamtion;
    public Collider2D Chest_Colider;
    public Collider2D PlayerCollider;
    public GameObject ChestGameObjekt;
    public SpriteRenderer ItemsToChangeSprite;
    public GameObject ItemsToChange;
    public List<ItemData> ItemsImages;
    public List<GameObject> ItemsHand1;
    private int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chestaniamtion.speed = 0f;
    }

    // Update is called once per frame
    void OnTriggerEnter2D()
    {
        if (PlayerCollider.IsTouching(Chest_Colider) && !ItemsHand1[index].activeSelf)
        {
            chestaniamtion.speed = 1f;
            StartCoroutine(WaytToDestroy());
            Debug.Log("Berürt");
        }
        else if (PlayerCollider.IsTouching(Chest_Colider) && ItemsHand1[index].activeSelf)
        {
            Debug.LogWarning("Player HAtt schon das item" + ItemsHand1[index]);
        }
    }
    private IEnumerator WaytToDestroy()
    {
        int index = Random.Range(0, ItemsImages.Count);
        ItemsToChangeSprite.sprite = ItemsImages[index].ItemImagePrev1;
        ItemsHand1[index].SetActive(true);
        yield return new WaitForSeconds(0.75f);
        ChestGameObjekt.SetActive(false);
        ItemsToChangeSprite.sprite = null;
    }
}
