using UnityEngine;
using UnityEngine.Playables;

public class PlayerFallingAnimationManager : MonoBehaviour, IDataPersitence
{
    private PlayableDirector PlayerFallingDownTimeline;
    private bool state;

    public void LoadData(SaveManager manager)
    {
        PlayerFallingDownTimeline = GetComponent<PlayableDirector>();
        state = false;

        var data = manager.dataSOs.gameProgressDataSO.gameProgressDatas.Find(x => x.Name == "PlayerFallingAnimationManager");

        if (data != null)
        {
            state = data.State;
        }


        PlayerFallingDownTimeline.enabled = !state;
    }

    public void SaveData(SaveManager manager)
    {
        var data = manager.dataSOs.gameProgressDataSO.gameProgressDatas.Find(x => x.Name == "PlayerFallingAnimationManager");

        if (data == null && PlayerFallingDownTimeline != null)
        {
            GameProgressData gameProgressData = new GameProgressData
            {
                Name = "PlayerFallingAnimationManager",
                State = true
            };
            manager.dataSOs.gameProgressDataSO.gameProgressDatas.Add(gameProgressData);

        }
    }
}
