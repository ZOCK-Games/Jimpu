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
