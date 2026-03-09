using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class ParticleInfo
{
    public GameObject particle;
    public string name;
}

/// <summary>
/// Can Be used to spawn Particles 
/// On specific position and destroys them 
/// after a specific time
/// </summary>
public class ParticelManager : MonoBehaviour
{
    public static ParticelManager instance { get; set; }
    public GameObject ParticleParent;
    public List<ParticleInfo> particleInfos;
    [Tooltip("If not set the manager uses the current particleInfos")]
    public string ParticleResourcesPath = "Particle";
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
            particleInfos = new List<ParticleInfo>();
            List<GameObject> Particels = Resources.LoadAll<GameObject>("Particles").ToList();
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
        if (particleInfos.Count < 0)
        {
            GameObject ParticleObject = particleInfos.Find(particleInfos => particleInfos.name == ParticleName).particle;
            GameObject gameObject = Instantiate(ParticleObject);

            if (gameObject != null)
            {
                gameObject.name = ParticleName;
                gameObject.transform.position = Position;
                gameObject.transform.SetParent(ParticleParent.transform);
                Destroy(gameObject, DeleteTime);
            }
            else
            {
                Debug.LogError("There is no Particle GameObject");
            }
        }
    }
}