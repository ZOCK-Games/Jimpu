using System;
using System.Collections;
using System.Linq;
using System.Xml;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TutorialManager : MonoBehaviour, IDataPersitence
{
    public Animator PlayerAnimator;
    public Animator DinoAnimator;
    public TextMeshProUGUI DinoText;
    public bool TutorialHasPlayed;
    private string CurentText;
    private int Dialog;
    void Start()
    {
        StartCoroutine(Tutorial());
        TutorialHasPlayed = false;
        PlayerAnimator.SetTrigger("Hello");


    }

    // Update is called once per frame
    void Update()
    {
        switch (Dialog)
        {
            case 1:
                CurentText = "Welcome Player";
                StartCoroutine(Tutorial());
                break;
        }

    }
    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(0.5f);
        DinoAnimator.SetTrigger("Talk");
        DinoText = CurentText.
        TutorialHasPlayed = true;


    }

    public void LoadGame(GameData data)
    {
        this.TutorialHasPlayed = data.TutorialHasPlayed;
    }

    public void SaveGame(ref GameData data)
    {
        data.TutorialHasPlayed = this.TutorialHasPlayed;
    }
}
