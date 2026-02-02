using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    private GameObject LuckyBlockObj;
    private PlayerControll playerControll;
    private Inventory inventory;
    private ChestManager chestManager;
    private InputSystem_Actions inputActions;

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
        chestManager = GetComponentInParent<ChestManager>();
        inventory = GetComponentInParent<Inventory>();
        playerControll = GetComponentInParent<PlayerControll>();
        LuckyBlockObj = this.gameObject;
    }
    void Update()
    {
        if (inputActions.Player.Interact.WasPerformedThisFrame() && LuckyBlockObj.activeSelf)
        {
            SpawnChest();
        }

    }
    public void SpawnChest()
    {
        Vector3 Position = new Vector3(playerControll.Player.transform.position.x, playerControll.Player.transform.position.y + 1.5f, 0);
        chestManager.AddChest(Position);
        LuckyBlockObj.SetActive(false);
        inventory.RemoveHandItem(-1);
    }
}
