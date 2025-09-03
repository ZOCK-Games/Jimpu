using System.Collections;
using Mono.Cecil.Cil;
using UnityEngine;

public class SpeedItemScribt : MonoBehaviour
{
    public Inventory InvScribt;
    public float Adding;
    public ItemData SpeedItemData;
    public PlayerControll PlayerScribt;
    public GameObject SpeedObjekt;
    public Inventory inventory;
    private bool PowerAktive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && SpeedObjekt.activeSelf && !PowerAktive)
        {
            PowerAktive = true;
            PlayerScribt.move_speed_R += Adding;
            PlayerScribt.move_speed_L += Adding;
            Debug.Log("Power Used Power is aktive");
            StartCoroutine(Wayt());
            inventory.ClearCurentItem = true;
        }


    }
    public void ResetStats()
    {
        PlayerScribt.move_speed_R -= Adding;
        PlayerScribt.move_speed_L -= Adding;
        PowerAktive = false;
        Debug.Log("Power Used Power is Disabled & reset");
        SpeedObjekt.SetActive(false);
    }
            
        
    
    public IEnumerator Wayt()
    {
        Debug.Log("Wayt Stardet 5 sec to reset");
            yield return new WaitForSeconds(5);
            Debug.Log("Wayt_Aktive end");
            ResetStats();
    }
    
}
