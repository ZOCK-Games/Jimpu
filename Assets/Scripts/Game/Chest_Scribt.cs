using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestScribt : MonoBehaviour
{
    public Collider2D PlayerCollider;
    public GameObject ChestContainer;
    public GameObject ItemDisplayContainer;
    public Inventory inventory;
    public List<GameObject> Chests = new List<GameObject>();
    public bool FreeSlot;
    public GameObject ItemPrefab;
    private int CurrentChest;

    public delegate void CheckChest();
    public event CheckChest OnChestContainerChanged;
    void Start()
    {
        StartCoroutine(CheckChestContainer());
    }

    // Update is called once per frame
    public void Update()
    {
        for (int i = 0; i < inventory.Item.Count; i++)
            if (!inventory.Item[i].activeSelf)
            {
                FreeSlot = true;
            }
            else
            {
                FreeSlot = false;
            }
        for (int i = 0; i < Chests.Count; i++)

        {
            if (PlayerCollider.IsTouching(Chests[i].GetComponent<BoxCollider2D>()) && FreeSlot == true)
            {
                FreeSlot = false;
                CurrentChest = i;
                StartCoroutine(WaytToDestroy());
            }

            else if (PlayerCollider.IsTouching(Chests[i].GetComponent<BoxCollider2D>()))
            {

                Chests[i].GetComponent<Animator>().SetTrigger("Chest Interact");
            }
        }
    }

    IEnumerator CheckChestContainer()
    {
        Chests.Clear();
        for (int i = 0; i < ChestContainer.transform.childCount; i++)
        {
            Chests.Add(ChestContainer.transform.GetChild(i).gameObject);
        }
        yield return new WaitUntil(() => ChestContainer.transform.childCount != Chests.Count);
        StartCoroutine(CheckChestContainer());
    }


    private IEnumerator WaytToDestroy()
    {
        int ChestSave = CurrentChest;
        GameObject ItemDisplay = Instantiate(ItemPrefab);
        ItemDisplay.transform.parent = ItemDisplayContainer.transform;
        Vector3 ChestPosition = Chests[ChestSave].transform.position;
        ItemDisplay.transform.position = new Vector3(ChestPosition.x, ChestPosition.y + 0.8f, 0);
        for (int i = 0; i < 20; i++)
        {
            int DisplayItem = Random.Range(0, inventory.itemData.Count);
            yield return new WaitForSeconds(0.2f);
            ItemDisplay.GetComponent<SpriteRenderer>().sprite = inventory.itemData[DisplayItem].ItemImagePrev1;
        }
        int CurrentItemInt = Random.Range(0, inventory.itemData.Count);
        Debug.Log(CurrentItemInt);
        Chests[ChestSave].GetComponent<Animator>().SetTrigger("Chest Open");
        ItemDisplay.GetComponent<SpriteRenderer>().sprite = inventory.itemData[CurrentItemInt].ItemImagePrev1;
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < inventory.Item.Count; i++)
        {
            inventory.Item[i].SetActive(false);
        }
        inventory.Item[CurrentItemInt].SetActive(true);
        for (int i = 0; i < ItemDisplayContainer.transform.childCount; i++)
        {
            Destroy(ItemDisplayContainer.transform.GetChild(i).gameObject);
        }
        Destroy(Chests[ChestSave]);
        StartCoroutine(CheckChestContainer());
    }
}
