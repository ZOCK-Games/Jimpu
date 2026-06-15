using Unity.Cinemachine;
using UnityEngine;
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
        SceneInfoManager.OnSceneChanged += CheckScene;
    }
    void OnDisable()
    {
        SceneInfoManager.OnSceneChanged -= CheckScene;
    }

    void CheckScene(SceneSettings sceneSetting)
    {
        if (sceneSetting.tag != SceneTags.Game) return;
        CameraTarget = GameObject.FindWithTag("Player");
        if (CameraTarget != null)
        {
            SetTarget(CameraTarget);
        }
    }
    public void SetTarget(GameObject target)
    {
        cinemachineCamera.Follow = target.transform;
        cinemachineCamera.LookAt = target.transform;
    }
}
