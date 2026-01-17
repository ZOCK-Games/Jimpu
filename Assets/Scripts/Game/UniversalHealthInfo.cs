using TMPro;
using UnityEngine;

public class UniversalHealthInfo : MonoBehaviour
{
    [SerializeField] private float _internalHealth; 

    public float Health 
    {
        get => _internalHealth;
        set
        {
            if (_internalHealth == value || HealthText == null) return;

            _internalHealth = value;
            HealthText.text = _internalHealth.ToString("0");
        }
    }
    public TextMeshPro HealthText;
    public bool IsDeath => Health < 0;


}
