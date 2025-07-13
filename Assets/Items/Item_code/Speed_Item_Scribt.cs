using System.Collections;
using UnityEngine;

public class Speed_Item_Scribt : MonoBehaviour
{
    public Inventory InvScribt;
    public ItemData SpeedItemData;
    public Player_move Player_scribt;
    public bool Power_Aktive;
    private bool Wayt_Aktive;
    private bool Wayt_Inaktive;
    private bool reset_stats;
    public GameObject PowerAnimationObjekt;
    public GameObject SpeedObjekt;
    public SpriteRenderer PowerAnimationSpriteRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Wayt_Aktive = false;
        Wayt_Inaktive = false;
        reset_stats = false;
        PowerAnimationObjekt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && Power_Aktive == false && Wayt_Inaktive == false)
        {
            PowerAnimationObjekt.SetActive(true);
            SpeedObjekt.SetActive(true);
            Player_scribt.move_speed_R = 7;
            Player_scribt.move_speed_L = 14;
            Player_scribt.Jump_speed += 70f;
            Debug.Log("Power Used Power is aktive");
            Power_Aktive = true;
            Wayt_Aktive = true;
            StartCoroutine(Wayt());
        }

        if (reset_stats == true)
        {
            PowerAnimationObjekt.SetActive(false);
            Player_scribt.move_speed_R = 5;
            Player_scribt.move_speed_L = 10;
            Player_scribt.Jump_speed = 55;
            SpeedObjekt.SetActive(false);
            Debug.Log("Power Used Power is Disabled & reset");
            Power_Aktive = false;
            Wayt_Aktive = false;
            reset_stats = false;
            Wayt_Inaktive = true;
            StartCoroutine(Wayt());
        }
        else if (Power_Aktive == false)
        {
            PowerAnimationObjekt.SetActive(false);
        }
    }
    public IEnumerator Wayt()
    {
        if (Wayt_Aktive == true)
        {
            yield return new WaitForSeconds(5);
            Wayt_Aktive = false;
            PowerAnimationObjekt.SetActive(false);
            PowerAnimationSpriteRenderer.color = Color.gray;
            Debug.Log("Wayt_Aktive end");
            reset_stats = true;

        }
        if (Wayt_Inaktive == true)
        {
            yield return new WaitForSeconds(10);
            PowerAnimationSpriteRenderer.color = Color.white;
            Wayt_Inaktive = false;
            Debug.Log("Wayt_Inaktive end");

        }

    }
    
}
