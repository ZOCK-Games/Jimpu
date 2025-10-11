using System.Collections;
using UnityEngine;

public class SpeedItemScribt : MonoBehaviour
{
    [SerializeField] private float Adding;
    [SerializeField] private ItemData SpeedItemData;
    [SerializeField] private PlayerControll PlayerScribt;
    [SerializeField] private GameObject SpeedObjekt;
    [SerializeField] private Inventory inventory;
    private bool PowerAktive;
    private float MoveBevoreR;
    private float MoveBevoreL;

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
            MoveBevoreR = PlayerScribt.move_speed_R;
            MoveBevoreL = PlayerScribt.move_speed_L;

            PlayerScribt.move_speed_R += Adding;
            PlayerScribt.move_speed_L += Adding;
            Debug.Log("Power Used Power is aktive");
            StartCoroutine(Waiting());
        }


    }
    public void ResetStats()
    {
        PlayerScribt.move_speed_R = MoveBevoreR;
        PlayerScribt.move_speed_L = MoveBevoreL;

        PowerAktive = false;
        inventory.Clear();
        Debug.Log("Power Used Power is Disabled & reset");
        PlayerScribt.Jump_speed = 350; // to prevent it from making it to low soe how..
        SpeedObjekt.SetActive(false);
    }
            
        
    
    public IEnumerator Waiting()
    {
        Debug.Log("Wayt Stardet 5 sec to reset");
        yield return new WaitForSeconds(5);
        Debug.Log("Wayt_Aktive end");
        ResetStats();
    }
    
}
