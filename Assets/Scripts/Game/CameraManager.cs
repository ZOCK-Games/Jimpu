using System.Collections.Generic;
using System.IO.Compression;
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
    public CinemachineImpulseSource cinemachineImpulseSource;
    public CameraImpulses cameraImpulses;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
        if (cinemachineCamera == null) cinemachineCamera = GetComponent<CinemachineCamera>();
        SceneInfoManager.OnSceneChanged += CheckScene;
        CheckScene(SceneInfoManager.instance.CurrentScene);
    }
    void OnDisable()
    {
        SceneInfoManager.OnSceneChanged -= CheckScene;
    }

    void CheckScene(SceneSettings sceneSetting)
    {
        Debug.Log("Scene Changed: " + sceneSetting.sceneName);
        var CameraTargets = GameObject.FindGameObjectsWithTag("CameraManagerTarget").ToList();
        if (sceneSetting.tag == SceneTags.Game)
        {
            CameraTarget = CameraTargets.Find(x => x.GetComponentInParent<playerControl>());
            GetComponentInChildren<Camera>().enabled = true;
        }
        else
        {
            CameraTarget = CameraTargets.Find(x => !x.GetComponentInParent<playerControl>());
            GetComponentInChildren<Camera>().enabled = false;
        }
        cinemachineCamera.transform.position = CameraTarget.transform.position;
        cinemachineCamera.Follow = CameraTarget.transform;
        cinemachineCamera.LookAt = CameraTarget.transform;
        transform.position = CameraTarget.transform.position;

    }

    public void PlayCameraAnimation(EntityTypes entityType, impulseSourceTypes impulseSourceType)
    {
        var impulse = cameraImpulses.Impulses.Find(x => x.entityType == entityType && x.impulseSourceType == impulseSourceType);
        if (impulse == null)
        {
            Debug.LogWarning("A script tried to play a camera impulse but it wasn't found");
            return;
        }
        impulse.CineMachineImpulseSource.GenerateImpulse();
    }

}

[System.Serializable]
public class CameraImpulses
{
    public List<ImpulseSource> Impulses = new List<ImpulseSource>();
}
[System.Serializable]

public class ImpulseSource
{
    public EntityTypes entityType;
    public CinemachineImpulseSource CineMachineImpulseSource;
    public impulseSourceTypes impulseSourceType;
}
[System.Serializable]
public enum impulseSourceTypes // The category's for each entity depending on the type of impulse
{
    Damage,
    Death,
    FallDamage,
    Heal,
}