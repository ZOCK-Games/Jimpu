using UnityEngine;
[CreateAssetMenu(fileName = "SkinElement", menuName = "SkinElement")]

public class SkinElement : ScriptableObject
{
    public string Name;
    public Sprite sprite;
    public SkinType skinType;
    public string ID;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(ID))
        {
            ID = System.Guid.NewGuid().ToString();
        }
    }
}