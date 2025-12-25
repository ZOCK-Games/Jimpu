using System.Collections;
using UnityEngine;

public class SpeedItemScribt : MonoBehaviour
{
    [SerializeField] private float Adding = 2;
    private PlayerControll PlayerScribt;
    public GameObject SpeedObjekt;
    public Inventory inventory;
    private bool PowerAktive;
    private float MoveBevore;
    private InputSystem_Actions inputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    void Start()
    {
        inventory = GetComponentInParent<ItemInfoManager>().inventory;
        PlayerScribt = GetComponentInParent<ItemInfoManager>().playerControll;
        SpeedObjekt = this.gameObject;
        PowerAktive = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Interact.WasPerformedThisFrame() && SpeedObjekt.activeSelf && !PowerAktive)
        {
            PowerAktive = true;
            MoveBevore = PlayerScribt.MoveSpeed;

            PlayerScribt.MoveSpeed += Adding;
            Debug.Log("Power Used Power is aktive");
            StartCoroutine(Waiting());
        }


    }
    public void ResetStats()
    {
        PlayerScribt.MoveSpeed = MoveBevore;
        PowerAktive = false;
        inventory.RemoveItem(inventory.HandSlot.ItemStored, -10, null);
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
