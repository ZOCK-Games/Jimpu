using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance { get; set; }
    public int EnergyAmount;
    private bool Filling;
    public Action<int> EnergyChanged;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        if (EnergyAmount <= 90 && !Filling)
        {
            StartCoroutine(AutoFill());
        }
    }
    IEnumerator AutoFill()
    {
        Filling = true;
        while (EnergyAmount <= 90)
        {
            EnergyAmount += 1;
            EnergyChanged(EnergyAmount);
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
            EnergyChanged(EnergyAmount);
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
            EnergyChanged(EnergyAmount);
            yield return null;
        }
    }
}
