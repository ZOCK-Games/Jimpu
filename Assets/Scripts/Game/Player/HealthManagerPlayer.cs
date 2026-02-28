using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DamageTags
{
    public string TagName;
    public int Damage;
    public bool IsActive;
}
public class HealthManagerPlayer : MonoBehaviour
{
    public List<DamageTags> DamageInfo = new List<DamageTags>();
    public int PlayerHealth;
    public GameObject HeartContainer;
    public GameObject HeartPrefab;
    public GameObject DamageParticle;
    public GameObject HealParticle;
    public VibrateControllerManager vibrateController;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////////////////
        ///             Heart System                         ///
        ////////////////////////////////////////////////////////

        if (HeartContainer.transform.childCount != PlayerHealth)
        {
            foreach (Transform child in HeartContainer.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < PlayerHealth; i++)
            {
                GameObject HeartObj = Instantiate(HeartPrefab, HeartContainer.transform);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < DamageInfo.Count; i++)
        {
            if (collision.gameObject.tag == DamageInfo[i].TagName && DamageInfo[i].IsActive)
            {
                if (DamageInfo[i].Damage > 0)
                {
                    GameObject Particel = Instantiate(HealParticle);
                    Particel.transform.position = collision.gameObject.transform.position;
                    Destroy(Particel, 0.2f);
                }
                else
                {
                    GameObject Particel = Instantiate(DamageParticle);
                    Particel.transform.position = collision.gameObject.transform.position;
                    Destroy(Particel, 0.2f);
                }
                vibrateController.VibrateController(0.5f, 0f, 2f);
                PlayerHealth += DamageInfo[i].Damage;
                Debug.Log("New Health for player: " + PlayerHealth);
            }
        }
    }
}
