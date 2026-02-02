using UnityEngine;

[CreateAssetMenu(fileName = "Skins", menuName = "Scriptable Objects/Skins")]
public class Skins : ScriptableObject
{
    public Sprite Head;
    public Sprite Body;
    public Sprite LeftArm;
    public Sprite RightArm;
    public Sprite LeftLeg;
    public Sprite RightLeg;
    public string SkinName;
    
}
