using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    public GameObject LuckyBlockI;
    public GameObject chest;
    public GameObject PlayerTransform;
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
            chest_p.transform.position = PlayerTransform.transform.position;
            Vector3 pos = chest_p.transform.position;
            pos.y = +1.5f;
            chest_p.transform.position = pos;
            LuckyBlockI.SetActive(false);
            inventory.ClearCurentItem = true;
    }
}
