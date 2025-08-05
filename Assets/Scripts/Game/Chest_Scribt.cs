using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestScribt : MonoBehaviour
{
    public Collider2D PlayerCollider;
    public GameObject ChestGameObjekt;
    public SpriteRenderer ItemsToChangeSprite;
    public GameObject ItemsToChange;
    public Inventory inventory;
    public bool FreeSlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChestGameObjekt.GetComponent<Animation>().Stop("Chest Open");
        FreeSlot = false;

    }

    // Update is called once per frame
    public void Update()
    {
        for (int i = 0; i < inventory.Item.Count; i++)
            if (!inventory.Item[i].activeSelf)
            {
                FreeSlot = true;
            }

        if (PlayerCollider.IsTouching(ChestGameObjekt.GetComponent<BoxCollider2D>()) && FreeSlot == true)
        {
            ChestInteraction();
            Animator anim = ChestGameObjekt.GetComponent<Animator>();
            anim.SetTrigger("Chest Open");
            StartCoroutine(WaytToDestroy());
            Debug.Log("Player Berührt Chest: " + ChestGameObjekt.name);
        }

        else if (PlayerCollider.IsTouching(ChestGameObjekt.GetComponent<BoxCollider2D>()))
        {
            Debug.LogWarning("Player Hatt schon ein item ausgerüstet");
            ChestInteraction();
        }

    }
    private IEnumerator WaytToDestroy()
    {
        int CurrentItemInt = Random.Range(0, inventory.itemData.Count);
        Debug.Log(CurrentItemInt);
        ItemsToChangeSprite.sprite = inventory.itemData[CurrentItemInt].ItemImagePrev1;
        yield return new WaitForSeconds(0.75f);
        inventory.Item[CurrentItemInt].SetActive(true);
        ChestGameObjekt.SetActive(false);
    }
    private IEnumerator ChestInteraction()
    {
        Debug.Log("Started Interaction Aniamtion");
        ChestGameObjekt.GetComponent<Animator>().Play("TriggerChestInteract");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Stop Interaction Aniamtion");
    }
}
