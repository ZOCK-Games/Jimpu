using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JimpuListData", menuName = "Data/JimpuListData")]
public class JimpuListData : ScriptableObject
{
    public List<JimpuDataSO> jimpuDatas = new List<JimpuDataSO>();
}
