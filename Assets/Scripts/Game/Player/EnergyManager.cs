using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public int EnergyAmount;
    public Slider EnergySlider;
    private bool Filling;
    void Update()
    {
        if (EnergyAmount <= 90 && !Filling)
        {
            StartCoroutine(AutoFill());
        }
        EnergySlider.value = EnergyAmount;
    }
    IEnumerator AutoFill()
    {
        Filling = true;
        while (EnergyAmount <= 90)
        {
            EnergyAmount += 1;
            yield return new WaitForSeconds(0.25f);
        }
        Filling = false;
    }

    public IEnumerator RemoveEnergy(int Energy)
    {
        int AddedEnergy = 0;
        while (AddedEnergy >= Energy)
        {
            EnergyAmount -= 1;
            AddedEnergy -= 1;
            yield return null;
        }
    }
    public IEnumerator AddEnergy(int Energy)
    {
        int AddedEnergy = 0;
        while (AddedEnergy <= Energy)
        {
            EnergyAmount += 1;
            AddedEnergy += 1;
            yield return null;
        }
    }
}
