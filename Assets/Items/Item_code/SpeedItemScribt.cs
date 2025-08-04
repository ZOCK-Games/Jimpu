using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedItemScribt : MonoBehaviour
{
    public Inventory InvScribt;
    public ItemData SpeedItemData;
    public PlayerControll PlayerScribt;
    public GameObject SpeedObjekt;
    public Inventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpeedObjekt.GetComponent<Animation>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && SpeedObjekt.activeSelf)
        {
            SpeedObjekt.GetComponent<Animation>().Play();
            SpeedObjekt.SetActive(true);
            PlayerScribt.move_speed_R = 7;
            PlayerScribt.move_speed_L = 14;
            PlayerScribt.Jump_speed += 70f;
            Debug.Log("Power Used Power is aktive");
            StartCoroutine(Wayt());
            inventory.ClearCurentItem = true;
        }


    }
    public void ResetStats()
    {
        SpeedObjekt.GetComponent<Animation>().Stop();
        PlayerScribt.move_speed_R = 3.5f;
        PlayerScribt.move_speed_L = 3.5f;
        PlayerScribt.Jump_speed = 30;
        SpeedObjekt.SetActive(false);
        Debug.Log("Power Used Power is Disabled & reset");
        SpeedObjekt.GetComponent<Animation>().Stop();
    }
            
        
    
    public IEnumerator Wayt()
    {
        Debug.Log("Wayt Stardet 5 sec to reset");
            yield return new WaitForSeconds(5);
            SpeedObjekt.GetComponent<Animation>().Stop();
            Debug.Log("Wayt_Aktive end");
            ResetStats();
    }
    
}
