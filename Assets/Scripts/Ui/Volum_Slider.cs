using UnityEngine;
using UnityEngine.UI;

public class Volum_Slider : MonoBehaviour
 
{
    public Slider Slider1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Slider1.SetValueWithoutNotify(2);
        
    }
}
