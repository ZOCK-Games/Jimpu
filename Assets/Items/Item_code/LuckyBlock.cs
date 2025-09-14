using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    public GameObject LuckyBlockI;
    public GameObject chest;
    public PlayerControll playerControll;
    public Inventory inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && LuckyBlockI.activeSelf)
        {
            SpawnChest();
        }

    }
    public void SpawnChest()
    {
        GameObject chest_p = Instantiate(chest);
        chest_p.name = "ChestLukyBlock";
        Vector3 pos = playerControll.transform.position;
        pos.y = playerControll.Player.transform.position.y + 1.5f;
        chest_p.transform.position = pos;
        LuckyBlockI.SetActive(false);
        Debug.Log("Spawned Chest for Player at: " + pos);
        inventory.ClearCurentItem = true;
    }
}
