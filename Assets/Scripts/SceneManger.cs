using UnityEngine;
using System.Collections;
public class SceneManger : MonoBehaviour
{
    [SerializeField] private bool UseSavedPlayerPosition;
    [SerializeField] private bool SavePlayerPosition;
    [SerializeField] private Vector2 PlayerSpawnPos;
   [SerializeField] private PlayerControll playerControll;
    void awake()
    {
    }
}
