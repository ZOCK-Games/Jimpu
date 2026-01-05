using UnityEngine;

public class UniversalHealthInfo : MonoBehaviour
{
    public float Health;
    public bool IsDeath => Health < 0;
}
