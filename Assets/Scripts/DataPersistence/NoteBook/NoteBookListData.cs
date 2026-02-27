using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteBookListSO", menuName = "Data/NoteBookListSO")]
public class NoteBookListSO : ScriptableObject
{
    public List<NoteBookData> noteBookDatas = new List<NoteBookData>();
}
