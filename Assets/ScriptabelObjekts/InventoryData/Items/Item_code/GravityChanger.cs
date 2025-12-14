using System.Collections;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    private PlayerControll PlayerScribt;
    private GameObject GravityChangingObjekt;
    public bool reset;
    public bool BlockUse;
    private Inventory inventory;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
    void Start()
    {
        GravityChangingObjekt = this.gameObject;
        PlayerScribt = GetComponentInParent<ItemInfoManager>().playerControll;
        inventory = GetComponentInParent<ItemInfoManager>().inventory;
        reset = false;
        BlockUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        float DefaultJump = PlayerScribt.Jump_speed;
        if (GravityChangingObjekt.activeSelf && inputActions.Player.Interact.WasPerformedThisFrame() && !BlockUse)
        {
            PlayerScribt.rb.gravityScale = -0.25f;
            PlayerScribt.PlayerRotation = 180;
            PlayerScribt.Jump_speed = -DefaultJump / 3;
            StartCoroutine(IsAktive());
        }
        if (reset == true && inputActions.Player.Interact.WasPerformedThisFrame())
        {
            PlayerScribt.PlayerRotation = 0f;
            PlayerScribt.rb.gravityScale = 1f;
            PlayerScribt.Jump_speed = 350;
            StartCoroutine(GoDown());
            Debug.Log("Reset");
        }
    }
    public IEnumerator IsAktive()
    {
        yield return new WaitForSeconds(0.3f);
        reset = true;
        Debug.Log("IsAktive = true");
    }
    public IEnumerator GoDown()
    {
        yield return new WaitForSeconds(0.1f);
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
        inventory.RemoveHandItem(-1);
        GravityChangingObjekt.SetActive(false);
    }
}
