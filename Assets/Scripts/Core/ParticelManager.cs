using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParticleInfo
{
    public GameObject particle;
    public string name;
}


public class ParticelManager : MonoBehaviour
{
    public static ParticelManager instance {get; set;}
    public GameObject ParticleParent;
    public List<ParticleInfo> particleInfos;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        GameObject PM = new GameObject("ParticleManager");
        PM.AddComponent<ParticelManager>();
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        if (ParticleParent == null)
        {
            GameObject Parent = new GameObject("ParticleParent");
            ParticleParent = Parent;
        }
    }

    public void SpawnParticle(Vector3 Position, string ParticleName, float DeleteTime)
    {
        GameObject gameObject = Instantiate(particleInfos.Find(particleInfos => particleInfos.name == ParticleName).particle);

        if (gameObject != null)
        {
            gameObject.name = ParticleName;
            gameObject.transform.position = Position;
            Destroy(gameObject, DeleteTime);
        }
    }
}