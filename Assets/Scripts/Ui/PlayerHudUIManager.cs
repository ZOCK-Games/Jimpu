using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
/// <summary>
/// This is a Manager for the player hud UI
/// it can be aces via the instance,
/// 
/// </summary>
public class PlayerHudUIManager : MonoBehaviour
{
    public static PlayerHudUIManager instance { get; set; }
    public PlayerHudUIScriptList playerHudUIScriptList;
    [Space(1)]
    public Transform HearthContainer;
    public GameObject HeartPrefab;
    [Space(1)]
    public Slider energySlider;
    private bool subscribed;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        subscribed = false;
        SceneInfoManager.OnSceneChanged += x => CheckScene(x);
        toggle(false);

    }
    void CheckScene(SceneSettings scene)
    {
        if (scene == null) return;
        if (scene.tag == SceneTags.Game)
        {
            CheckHealth(HealthManagerPlayer.instance.PlayerHealth);
            CheckEnergy(EnergyManager.instance.EnergyAmount);
            if (!subscribed)
            {
                HealthManagerPlayer.instance.HealthChanged += CheckHealth;
                EnergyManager.instance.EnergyChanged += CheckEnergy;
            }
            subscribed = true;

            toggle(true);
        }
        else if (subscribed)
        {
            HealthManagerPlayer.instance.HealthChanged -= CheckHealth;
            EnergyManager.instance.EnergyChanged -= CheckEnergy;
            subscribed = false;

            toggle(false);
        }
    }

    void toggle(bool status)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(status);
        }
    }

    void CheckHealth(int PlayerHealth)
    {
        ////////////////////////////////////////////////////////
        ///             Heart System                         ///
        ////////////////////////////////////////////////////////

        if (HearthContainer.transform.childCount != PlayerHealth)
        {
            foreach (Transform child in HearthContainer.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < PlayerHealth; i++)
            {
                GameObject HeartObj = Instantiate(HeartPrefab, HearthContainer.transform);
            }
        }
    }
    void CheckEnergy(int PlayerEnergy)
    {
        energySlider.value = PlayerEnergy;
    }
}
[System.Serializable]
public class PlayerHudUIScriptList
{
    public NoteBookManager noteBookManager;
    public Inventory inventory;
    public CheatCode cheatCode;
}