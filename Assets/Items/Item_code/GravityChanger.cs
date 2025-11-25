using System.Collections;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    public PlayerControll PlayerScribt;
    public GameObject GravityChangingObjekt;
    public GameObject PlayerGameObjekt;
    public bool IsAktive1;
    public bool reset;
    public bool BlockUse;
    public Inventory inventory;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsAktive1 = false;
        reset = false;
        BlockUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GravityChangingObjekt.activeSelf && IsAktive1 == false && inputActions.Player.Interact.WasCompletedThisFrame() && BlockUse == false)
        {
            PlayerGameObjekt.GetComponent<Rigidbody2D>().gravityScale = -0.25f;
            PlayerScribt.PlayerRotation = 180;
            PlayerScribt.Jump_speed = -55;
            StartCoroutine(IsAktive());
        }
        if (reset == true && Input.GetKey(KeyCode.E))
        {
            PlayerScribt.PlayerRotation = 0f;
            PlayerGameObjekt.GetComponent<Rigidbody2D>().gravityScale = 1f;
            PlayerScribt.Jump_speed = 55;
            StartCoroutine(GoDown());
            Debug.Log("Reset");
        }
    }
    public IEnumerator IsAktive()
    {
        yield return new WaitForSeconds(0.3f);
        reset = true;
        Debug.Log("IsAktive = true");
        IsAktive1 = true;
    }
    public IEnumerator GoDown()
    {
        yield return new WaitForSeconds(0.1f);
        IsAktive1 = false;
        reset = false;
        Debug.Log("IsAktive = false");
        StartCoroutine(Wayt());

    }
    public IEnumerator Wayt()
    {
        BlockUse = true;
        Debug.Log("Wayt start");
        yield return new WaitForSeconds(2);
        BlockUse = false;
        inventory.Clear();
        GravityChangingObjekt.SetActive(false);
    }
}
