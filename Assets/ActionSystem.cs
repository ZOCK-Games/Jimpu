using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{
    public Rigidbody2D PlayerRB;
    public enum ActionType
    {
        Input,
        Position,
        Level,
        Item
    }
    public enum InputType
    {
        Key,
        Walk
    }
    public enum KeyType
    {
        Pressed,
        HoldDown
    }
    public enum Walk
    {
        WalkLeft,
        WalkRight,
        Jump,
        WalkSpeed,
    }
    public enum PositionType
    {
        Position,
        Transform,
        Radius
    }
    public enum Level
    {
        HighestLevel,
        LockedLevel,
        UnLockedLevel,
        InLevel,
    }

    bool CheckAction(string Action)
    {
        if (Action != null)
        {
            string[] parts = Action.Split(':');
            switch (parts[0])
            {
                case "Input":
                    {
                        ///
                        /// All Input Actions Mouse,keyboard 
                        switch (parts[1])
                        {
                            case "Key":
                                {
                                    ///
                                    /// Key Input Actions
                                    /// 
                                    switch (parts[2])
                                    {
                                        case "Presed":
                                            {
                                                KeyCode resultKey;
                                                if (Enum.TryParse(parts[3], true, out resultKey))
                                                {
                                                }
                                                return Input.GetKey(resultKey);
                                            }
                                        case "HoldDown":
                                            {
                                                KeyCode resultKey;
                                                if (Enum.TryParse(parts[3], true, out resultKey))
                                                {
                                                }
                                                return Input.GetKeyDown(resultKey);
                                            }
                                        default:
                                            return false;

                                    }
                                    break;
                                }

                            case "Walk":
                                {
                                    ///
                                    /// Walk Actions
                                    /// 
                                    switch (parts[2])
                                    {
                                        case "WalkLeft":
                                            {
                                                return PlayerRB.linearVelocityX < -0.5f;
                                            }
                                        case "WalkRight":
                                            {
                                                return PlayerRB.linearVelocityX > 0.6f;
                                            }
                                        case "Jump":
                                            {
                                                // Check if the player jumps whit new input system
                                                return false;

                                                break;
                                            }
                                        case "WalkSpeed":
                                            {
                                                float Speed;
                                                if (float.TryParse(parts[3], out Speed))
                                                    return Mathf.Abs(PlayerRB.linearVelocity.x) >= Speed;
                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                        default:
                                            return false;
                                    }

                                }
                            default:
                                return false;

                        }
                    }
                case "Position":
                    return false;
                case "Level":
                    return false;
                case "Item":
                    return false;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }
    public IEnumerator NewAction(float ActionTime, string Action)
    {
        float TimePassed = 0;
        while (TimePassed < ActionTime)
        {

            if (CheckAction(Action))
            {
                Debug.Log("Action Is done");
                yield break;
            }
            TimePassed += Time.deltaTime;
            yield return null;
        }
        Debug.Log($"Action: {Action} Failed");
    }

}
