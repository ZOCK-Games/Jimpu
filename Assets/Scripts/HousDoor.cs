using System.Collections;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HousDoor : MonoBehaviour
{
    public Animator HousAnimator;
    public Animator PlayerAnimator;
    public Animator GUIAnimator;
    public Animator InfoTextAnimator;
    public BoxCollider2D HousTriggerZone;
    public GameObject PlayerObj;
    [Header("Animator Trigger Names")]

    public string PlayerGoIn;
    public string GUIDarkPanel;
    public string InfoTrigger;
    public string DoorTrigger;
    public string GoToScene;
    private bool CanOpenDoor;
    void Start()
    {
        CanOpenDoor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerObj.GetComponent<PolygonCollider2D>().IsTouching(HousTriggerZone))
        {
            InfoTextAnimator.SetTrigger(InfoTrigger);
            if (Input.GetKey(KeyCode.E) && CanOpenDoor == true)
            {
                Debug.Log("Opening Door");
                HousAnimator.SetTrigger(DoorTrigger);
                PlayerAnimator.SetTrigger(PlayerGoIn);
                GUIAnimator.SetTrigger(GUIDarkPanel);
                CanOpenDoor = false;
                StartCoroutine(Wayt());

            }
        }
    }
    public IEnumerator Wayt()
    {
        Debug.Log("Started Wayt");
        yield return new WaitForSeconds(0.6f);
        CanOpenDoor = true;
        SceneManager.LoadScene(GoToScene);
    }
}
