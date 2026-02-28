using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.Collections;

public class CheatShootUI : MonoBehaviour
{
    public Button SingelShoot;
    public Button MultiShoot;
    public GameObject SettingsUI;
    public GameObject SelectUI;
    public BulletShootVoice bulletShootVoice;
    public Button CloseShotSettings;
    public Button CloseShotSelect;
    [Header("ShootSettings")]
    public Button ShootBullet;

    public TMP_Dropdown ShootingOBj;
    public TMP_InputField BulletSpeed;
    public TMP_InputField BulletsCount;
    public TMP_Dropdown Duplicate;
    public TMP_InputField TargetPosX;
    public TMP_InputField TargetPsoY;
    public TMP_Dropdown Target;

    public TMP_InputField ShootingPosX;
    public TMP_InputField ShootingPosY;
    public TMP_Dropdown Shooting;
    void Start()
    {
        ShootingOBj.ClearOptions();
        List<String> Objekts = new List<String>();
        for (int i = 0; i < bulletShootVoice.Bullets.Count; i++)
        {
            Objekts.Add(bulletShootVoice.Bullets[i].name);
        }
        ShootingOBj.AddOptions(Objekts);
        SettingsUI.SetActive(false);
        SingelShoot.onClick.AddListener(() => { SettingsUI.SetActive(true); StartCoroutine(ShootSettings());});
        MultiShoot.onClick.AddListener(() => {SettingsUI.SetActive(true); StartCoroutine(MultiShooShoot());});
        CloseShotSettings.onClick.AddListener(() => SettingsUI.SetActive(false));
        CloseShotSelect.onClick.AddListener(() => SelectUI.SetActive(false));
    }

    // Update is called once per frame
    IEnumerator MultiShooShoot()
    {
        {
            bool Shoot = false;
            while (!Shoot)
            {
                ShootBullet.onClick.AddListener(() => Shoot = true);
                yield return null;
            }
            Debug.Log("Checking Shooting..");
            /// Finding Target
            Transform ShootTarget = null;
            if (Target.value == 0)
            {
                ShootTarget = bulletShootVoice.playerControll.Player.transform; //Player Pos
            }
            else if (Target.value == 1)
            {
                ShootTarget = bulletShootVoice.ShotingPositionShooter;  //Narrator Pos
            }
            else
            {
                ShootTarget.position = new Vector3(float.Parse(TargetPosX.text), float.Parse(TargetPsoY.text), 0); // Input Pos
            }

            /// Getting Bullet Object
            /// 
            GameObject BulletObj = bulletShootVoice.Bullets[ShootingOBj.value];

            /// Getting ShootingPosition
            /// 
            Transform ShootingPosition = null;
            if (Target.value == 0)
            {
                ShootingPosition = bulletShootVoice.playerControll.Player.transform; //Player Pos
            }
            else if (Target.value == 1)
            {
                ShootingPosition = bulletShootVoice.ShotingPositionShooter;  //Narrator Pos
            }
            else
            {
                ShootingPosition.position = new Vector3(float.Parse(ShootingPosX.text), float.Parse(ShootingPosY.text), 0); // Input Pos
            }
            /// Getting Bullet Speed
            /// 
            float Speed = float.Parse(BulletSpeed.text);

            /// Getting Bullet Count
            /// 
            int Count = int.Parse(BulletsCount.text);

            /// Checking if It should Duplicate
            bool IsDuplicating = false;
            if (Duplicate.value == 1)
            {
                IsDuplicating = true;
            }
            else
            {
                IsDuplicating = false;
            }
            bulletShootVoice.ShotToRadius(ShootTarget, BulletObj, ShootingPosition, Speed, Count, 0, IsDuplicating);
            Debug.Log("Shooting..");
        }
    }

    IEnumerator ShootSettings()
    {
        bool Shoot = false;
        while (!Shoot)
        {
            ShootBullet.onClick.AddListener(() => Shoot = true);
            yield return null;
        }
        Debug.Log("Checking Shooting..");
        /// Finding Target
        Transform ShootTarget = null;
        if (Target.value == 0)
        {
            ShootTarget = bulletShootVoice.playerControll.Player.transform; //Player Pos
        }
        else if (Target.value == 1)
        {
            ShootTarget = bulletShootVoice.ShotingPositionShooter;  //Narrator Pos
        }
        else
        {
            ShootTarget.position = new Vector3(float.Parse(TargetPosX.text), float.Parse(TargetPsoY.text), 0); // Input Pos
        }

        /// Getting Bullet Object
        /// 
        GameObject BulletObj = bulletShootVoice.Bullets[ShootingOBj.value];

        /// Getting ShootingPosition
        /// 
        Transform ShootingPosition = null;
        if (Target.value == 0)
        {
            ShootingPosition = bulletShootVoice.playerControll.Player.transform; //Player Pos
        }
        else if (Target.value == 1)
        {
            ShootingPosition = bulletShootVoice.ShotingPositionShooter;  //Narrator Pos
        }
        else
        {
            ShootingPosition.position = new Vector3(float.Parse(ShootingPosX.text), float.Parse(ShootingPosY.text), 0); // Input Pos
        }
        /// Getting Bullet Speed
        /// 
        float Speed = float.Parse(BulletSpeed.text);

        /// Getting Bullet Count
        /// 
        int Count = int.Parse(BulletsCount.text);

        /// Checking if It should Duplicate
        bool IsDuplicating = false;
        if (Duplicate.value == 1)
        {
            IsDuplicating = true;
        }
        else
        {
            IsDuplicating = false;
        }
        StartCoroutine(bulletShootVoice.Shoot(ShootTarget, BulletObj, ShootingPosition, Speed, Count, 0, IsDuplicating));
        Debug.Log("Shooting..");
    }
}
