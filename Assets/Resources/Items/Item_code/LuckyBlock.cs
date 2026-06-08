using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    private GameObject LuckyBlockObj;
    private Inventory inventory;
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
        inventory = Inventory.instance;
        LuckyBlockObj = this.gameObject;
        inputActions.Player.Interact.performed += ctx =>
        {
            if (LuckyBlockObj.activeSelf)
            {
                SpawnChest();
            }
        };
    }

    public void SpawnChest()
    {
        Vector3 Position = new Vector3(playerControl.instance.Player.transform.position.x, playerControl.instance.Player.transform.position.y + 1.5f, 0);
        ChestManager chestManager = GameObject.FindFirstObjectByType<ChestManager>();
        chestManager.AddChest(Position);
        LuckyBlockObj.SetActive(false);
        inventory.RemoveHandItem(-1);
    }
}
