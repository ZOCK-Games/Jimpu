using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;
[Tooltip("Searches for a target with the tag: CameraTarget")]
public class CameraManager : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    public static CameraManager instance { get; set; }
    private GameObject CameraTarget;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        cinemachineCamera = this.gameObject.GetComponent<CinemachineCamera>();
        if (cinemachineCamera == null) cinemachineCamera = this.gameObject.GetComponentInChildren<CinemachineCamera>();
        SceneInfoManager.OnSceneChanged += CheckScene;
    }
    void OnDisable()
    {
        SceneInfoManager.OnSceneChanged -= CheckScene;
    }

    void CheckScene(SceneSettings sceneSetting)
    {
        Debug.Log("Scene Changed: " + sceneSetting.sceneName);
        var CameraTargets = GameObject.FindGameObjectsWithTag("CameraManagerTarget").ToList();
        if (CameraTarget != null && sceneSetting.tag == SceneTags.Game)
        {
            CameraTarget = CameraTargets.Find(x => x.GetComponentInParent<playerControl>());
            cinemachineCamera.transform.position = CameraTarget.transform.position;
            cinemachineCamera.Follow = CameraTarget.transform;
            cinemachineCamera.LookAt = CameraTarget.transform;
        }
        else
        {
            CameraTarget = CameraTargets.Find(x => !x.GetComponentInParent<playerControl>());
            cinemachineCamera.transform.position = CameraTarget.transform.position;
            cinemachineCamera.Follow = CameraTarget.transform;
            cinemachineCamera.LookAt = CameraTarget.transform;
            transform.position = CameraTarget.transform.position;
        }

    }

}
