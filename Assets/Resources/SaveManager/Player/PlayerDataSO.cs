using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Data/Player")]
public class PlayerDataSO : ScriptableObject
{
    public Vector3 PlayerPosition;
    public int Health;
    public List<BodyPart> bodyParts;
}
[System.Serializable]
public class BodyPart
{
    public string BodyPartType;
    public string BodyElementID;
    public string ColorHex;
}