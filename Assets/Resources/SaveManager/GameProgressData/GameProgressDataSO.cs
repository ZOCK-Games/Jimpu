using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameProgress", menuName = "Data/GameProgress")]
public class GameProgressDataSO : ScriptableObject
{
    public List<GameProgressData> gameProgressDatas = new List<GameProgressData>();
}