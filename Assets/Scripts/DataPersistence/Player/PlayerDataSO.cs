using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Data/Player")]
public class PlayerDataSO : ScriptableObject
{
    public Vector3 PlayerPosition;
    public int Health;
    public int SkinIndex;
    public string colorHex;
}
