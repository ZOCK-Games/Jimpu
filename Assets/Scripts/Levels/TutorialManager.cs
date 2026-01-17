using System;
using System.Collections;
using System.Linq;
using System.Xml;
using TMPro;
using Unity.Cinemachine;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class TutorialManager : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Animator DinoAnimator;
    public GameObject DinoTutorial;
    public TextMeshProUGUI DinoText;
    public bool TutorialHasPlayed;
    private bool DialogIsPlaying;
    private string CurentText;
    private int Dialog;

    void Awake()
    {
        DinoTutorial.SetActive(false);
        DinoAnimator.SetBool("Talk", false);
        if (!TutorialHasPlayed)
        {
            PlayerAnimator.enabled = true;
            DinoTutorial.SetActive(true);
            PlayerAnimator.SetTrigger("Hello");
            DialogIsPlaying = true;
            Dialog = 1;
            GetDialog();
            StartCoroutine(Tutorial());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialHasPlayed)
        {
            DinoAnimator.SetBool("Talk", false);
            DinoTutorial.SetActive(false);
            DialogIsPlaying = false;
            PlayerAnimator.enabled = false;
        }
        
        if (Input.anyKeyDown && DialogIsPlaying && !TutorialHasPlayed)
        {
            StartCoroutine(Silence());
        }         
    }
    void GetDialog()
    {
        switch (Dialog)
        {
            case 1:
                CurentText = "Welcome Player";
                StartCoroutine(Tutorial());
                break;
            case 2:
                CurentText = "This is Jimpu";
                StartCoroutine(Tutorial());
                break;
            case 3:
                CurentText = "Cool see you playing this game";
                StartCoroutine(Tutorial());
                break;
            case 4:
                CurentText = "As this is only a test Build!";
                DinoAnimator.SetTrigger("Confetti");
                StartCoroutine(Tutorial());
                break;
            case 5:
                CurentText = "I hope you have fun";
                StartCoroutine(Tutorial());
                break;
            case 6:
                CurentText = "With C you can enable cheats (:";
                StartCoroutine(Tutorial());
                break;
            case >6:
                DinoTutorial.SetActive(false);
                CurentText = "";
                DialogIsPlaying = false;
                TutorialHasPlayed = true;
                break;
            }
    }
    IEnumerator Tutorial()
    {
        DialogIsPlaying = true;
        DinoText.text = CurentText;
        yield return new WaitForSeconds(0.5f);
        DinoAnimator.SetBool("Talk" , true);
        yield return new WaitForSeconds(3f);
    }
    IEnumerator Silence()
    {
        DinoAnimator.SetBool("Talk" , false);
        DinoAnimator.SetTrigger("Silence");
        yield return new WaitForSeconds(1f);
        Dialog += 1;
        DialogIsPlaying = false;
        GetDialog();
    }


}
