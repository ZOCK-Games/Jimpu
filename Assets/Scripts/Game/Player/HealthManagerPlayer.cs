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
    public static HealthManagerPlayer instance {get; set;}
    [SerializeField] private List<DamageTags> DamageInfo = new List<DamageTags>();
    public int PlayerHealth;
    public Action<int> HealthChanged;
    [SerializeField] private GameObject DamageParticle;
    [SerializeField] private GameObject HealParticle;
    // Update is called once per frame
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
                VibrateControllerManager.instance.VibrateController(0.5f, 0f, 2f);
                PlayerHealth += DamageInfo[i].Damage;
                HealthChanged.Invoke(PlayerHealth);
                Debug.Log("New Health for player: " + PlayerHealth);
            }
        }
    }
}
