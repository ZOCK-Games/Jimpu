using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSetting", menuName = "Game Data/Setting")]
public class sceneSetting : ScriptableObject
{
    public List<SceneSettings> sceneSettings = new List<SceneSettings>();
    #if UNITY_EDITOR
    void Reset()
    {
        sceneSettings.Clear();
        foreach (var buildScene in EditorBuildSettings.scenes)
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path); 

            if (sceneAsset != null)
            {
                sceneSettings.Add(new SceneSettings
                {
                    tag = SceneTags.none,
                    sceneName = sceneAsset.name
                });
            }
        }
    }
    #endif
}