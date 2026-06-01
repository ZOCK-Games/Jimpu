using UnityEngine;
[CreateAssetMenu(fileName = "SkinElement", menuName = "SkinElement")]
[System.Serializable]
public class SkinElement : ScriptableObject
{
    public string Name;
    public Sprite sprite;
    public SkinType skinType;
    public string ID;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(ID))
        {
            ID = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(ID))
        {
            ID = System.Guid.NewGuid().ToString();
        }
    }
}

