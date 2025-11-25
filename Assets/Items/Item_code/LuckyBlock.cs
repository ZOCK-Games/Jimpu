using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    public GameObject LuckyBlockI;
    public GameObject chest;
    public PlayerControll playerControll;
    public GameObject ChestContainer;
    public Inventory inventory;
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
        if (inputActions.Player.Interact.WasCompletedThisFrame() && LuckyBlockI.activeSelf)
        {
            SpawnChest();
        }

    }
    public void SpawnChest()
    {
        GameObject chest_p = Instantiate(chest);
        chest_p.name = "ChestLukyBlock";
        chest_p.transform.parent = ChestContainer.transform;
        Vector3 pos = playerControll.transform.position;
        pos.y = playerControll.Player.transform.position.y + 1.5f;
        chest_p.transform.position = pos;
        LuckyBlockI.SetActive(false);
        inventory.Clear();
    }
}
