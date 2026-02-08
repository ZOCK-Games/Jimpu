using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public List<AudioClip> audioClips = new List<AudioClip>();

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
                GameObject Source = new GameObject($"Source: {AudioName}");
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

    void Update()
    {

    }
}
