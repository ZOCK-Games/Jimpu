using System.IO;
using System.Linq;
using System.Text.Json;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    public string SwitcherID;
    public Transform PlayerTeleportPosition;

    void Start()
    {
        if (PlayerTeleportPosition == null)
        {
            PlayerTeleportPosition = transform;
        }
    }
}
