using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int PlayerMoney;
    public int PlayerHealt;
    public Vector2 PlayerPosition;
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = Player.transform.position;


    }
    void OnApplicationQuit()
    {
        
    }
}
