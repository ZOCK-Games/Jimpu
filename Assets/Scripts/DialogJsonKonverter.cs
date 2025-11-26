using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DialogData", menuName = "Game Data/Dialog Asset")]
public class CurrentDialogData : ScriptableObject
{
    public string dialog_id;
    public string dialog_text;
    public bool dialog_ended;
    public string action;
    public string action_to_do;
    public float action_to_do_time;
    public string next_id_done;
    public string next_id_not_done;
    public void LoadFromData(DialogJsonData data)
    {
        this.dialog_id = data.dialog_id;
        this.dialog_text = data.dialog_text;
        this.dialog_ended = data.dialog_ended;
        this.action = data.action;
        this.action_to_do = data.action_to_do;
        this.action_to_do_time = data.action_to_do_time;
        this.next_id_done = data.next_id_done;
        this.next_id_not_done = data.next_id_not_done;  

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
