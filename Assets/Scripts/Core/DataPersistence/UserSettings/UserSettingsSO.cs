using UnityEngine;

[CreateAssetMenu(fileName = "UserSettingsSO", menuName = "Data/UserSettingsSO")]
public class UserSettingsSO : ScriptableObject
{
    #region SoundSettings
    /// <summary>
    /// Sound Settings
    /// </summary>
    public float MusicValue;
    /// <summary>
    /// Sound Settings
    /// </summary>
    public float EffectsValue;
    /// <summary>
    /// Sound Settings
    /// </summary>
    public float EnvironmentValue;
    /// <summary>
    /// Sound Settings
    /// </summary>
    public float GlobalValue = 100;
    #endregion 

    public bool ShowFPS = false;
    public string Device;

    /// <summary>
    /// Languages: DE,FR,ENG
    /// </summary>
    public string Language;
    public bool DevMode;
}
