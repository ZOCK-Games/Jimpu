using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    public GameObject LuckyBlockI;
    public GameObject chest;
    public PlayerControll playerControll;
    public Inventory inventory;
    public ChestManager chestManager;
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
    void Update()
    {
        if (inputActions.Player.Interact.WasPerformedThisFrame() && LuckyBlockI.activeSelf)
        {
            SpawnChest();
        }

    }
    public void SpawnChest()
    {
        Vector3 Position = new Vector3(playerControll.Player.transform.position.x, playerControll.Player.transform.position.y + 1.5f, 0);
        chestManager.AddChest(Position);
        LuckyBlockI.SetActive(false);
        inventory.Clear();
    }
}
