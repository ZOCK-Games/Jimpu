using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public List<AudioClip> audioClips = new List<AudioClip>();
    public List<GameObject> LoopedAudios = new List<GameObject>();
    public List<GameObject> NormalAudios = new List<GameObject>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        GameObject go = new GameObject("GlobalSoundManager");
        instance = go.AddComponent<AudioManager>();
        DontDestroyOnLoad(go);
        Debug.Log("SoundManager was created successfully!");
    }


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadAudios();
        StartCoroutine(LoadAudioFromDirectory("/home/zock/Music/Games/Jimpu"));
    }

    void LoadAudios()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audio/Game");
    }

    public IEnumerator LoadAudioFromDirectory(string Path)
    {
        string[] files = Directory.GetFiles(Path);
        foreach (string file in files)
        {
            string absolutePath = "file:///" + file.Replace("\\", "/");
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(absolutePath, AudioType.UNKNOWN);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                if (clip != null)
                {
                    clip.name = System.IO.Path.GetFileNameWithoutExtension(absolutePath);
                    audioClips.Add(clip);
                    Debug.Log($"Added {clip.name}");
                }
                else
                {
                    Debug.LogWarning("No Audio Clip Was Found");
                }

            }
            else
            {
                Debug.LogWarning("No Audio Was Found");
            }
        }
    }

    public void PlayAudio(string AudioName, Transform Position, bool IsChild = false, float Volume = 1)
    {
        AudioClip audio = audioClips.Find(audioClips => audioClips.name == AudioName);
        if (audio != null)
        {
            if (!IsChild)
            {
                AudioSource.PlayClipAtPoint(audio, Position.position);
            }
            else
            {
                GameObject Source = new GameObject($"{AudioName}");
                NormalAudios.Add(Source);
                Source.transform.SetParent(Position);
                Source.transform.localPosition = Vector3.zero;
                AudioSource Audio = Source.AddComponent<AudioSource>();
                Audio.volume = Volume;
                Audio.clip = audio;
                Audio.Play();
                Destroy(Source, audio.length);
            }
        }
    }

    public void PlayAudioLoop(string Name, Transform Position, string AudioName, float Volume = 1, bool IsChild = false)
    {
        AudioClip audio = audioClips.Find(audioClips => audioClips.name == AudioName);
        if (audio != null)
        {
            if (!IsChild)
            {
                AudioSource.PlayClipAtPoint(audio, Position.position);
            }
            else
            {
                GameObject Source = new GameObject($"{Name}");
                Source.name = Name;
                LoopedAudios.Add(Source);
                Source.transform.SetParent(Position);
                Source.transform.localPosition = Vector3.zero;
                AudioSource Audio = Source.AddComponent<AudioSource>();
                Audio.volume = Volume;
                Audio.clip = audio;
                Audio.Play();
            }
        }
    }

    public void StopAudio(string Name)
    {
        GameObject Object = LoopedAudios.Find(GameObject => GameObject.name == Name);
        if (Object != null)
        {
            Destroy(Object);
        }
        else
        {
            Object = NormalAudios.Find(GameObject => GameObject.name == Name);
            if (Object != null)
            {
                Destroy(Object);
            }
        }
    }
    public bool isPlaying(string name)
    {
        GameObject ObjLooped = LoopedAudios.Find(obj => obj != null && obj.name == name);
        if (ObjLooped != null)
        {
            return true;
        }
        else
        {
            GameObject ObjNormal = NormalAudios.Find(obj => obj != null && obj.name == name);
            if (ObjNormal != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void Update()
    {

    }
}
