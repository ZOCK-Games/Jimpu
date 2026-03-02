using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ParticleInfo
{
    public GameObject particle;
    public string name;
}


public class ParticelManager : MonoBehaviour
{
    public static ParticelManager instance { get; set; }
    public GameObject ParticleParent;
    public List<ParticleInfo> particleInfos;
    [Tooltip("If not set the manager uses the current particleInfos")]
    public string ParticleResourcesPath;
    /// <summary>
    /// Can Only Be used with ParticleResourcesPath
    /// </summary>

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
    void Start()
    {
        if (ParticleResourcesPath != null)
        {
            particleInfos.Clear();
            List<GameObject> Particels = Resources.LoadAll<GameObject>(ParticleResourcesPath).ToList();
            for (int i = 0; i < Particels.Count; i++)
            {
                if (Particels[i].GetComponent<ParticleSystem>())
                {
                    ParticleInfo info = new ParticleInfo
                    {
                        name = Particels[i].name,
                        particle = Particels[i]
                    };
                    particleInfos.Add(info);
                }
            }
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