using UnityEngine;

public class LuckyBlock : MonoBehaviour
{
    public GameObject LuckyBlockI;
    public GameObject chest;
    public GameObject PlayerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && LuckyBlockI.activeSelf)
        {
            GameObject chest_p = Instantiate(chest);
            chest_p.name = "ChestLukyBlock";
            chest_p.transform.position = PlayerTransform.transform.position;
            Vector3 pos = chest_p.transform.position;
            pos.y = -3.5f;
            chest_p.transform.position = pos;
            LuckyBlockI.SetActive(false);
        }
        
    }
}
