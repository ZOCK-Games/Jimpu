using Unity.VectorGraphics;
using UnityEngine;

public class SwitcherTrigger : MonoBehaviour
{
    public string SwitcherPointID;
    public string scene;

    public void Invoke()
    {
        _ = SwitcherManager.MoveToScenePoint(scene, SwitcherPointID, 0.5f);
    }
}