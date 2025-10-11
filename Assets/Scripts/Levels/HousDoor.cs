using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HousDoor : MonoBehaviour
{
    public Animator HousAnimator;
    public Animator PlayerAnimator;
    public GameObject SpeachBubbel;
    public BoxCollider2D HousTriggerZone;
    public GameObject PlayerObj;
    [Header("Animator Trigger Names")]
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
            SpeachBubbel.SetActive(true);
            if (Input.GetKey(KeyCode.E) && CanOpenDoor == true)
            {
                HousAnimator.Play("OpenDoor");
                PlayerAnimator.Play("PlayerGoIn");
                CanOpenDoor = false;
                StartCoroutine(Wayt());

            }
        }
        else
        {
            SpeachBubbel.SetActive(false);
        }
    }
    public IEnumerator Wayt()
    {
        yield return new WaitForSeconds(1.14f);
        CanOpenDoor = true;
        SpeachBubbel.SetActive(false);
        SceneManager.LoadScene(GoToScene);
    }
}
