using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    public static event Action<ItemData> OnAttackTurn;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Attack.performed += ctx => _ = AttackProcess();
    }
    void OnDisable()
    {
        inputActions.Player.Attack.performed -= ctx => _ = AttackProcess();
        inputActions.Player.Disable();
    }

    public async Task AttackProcess()
    {
        Debug.Log("Started Attack MAnager");

        Vector2 Direction = GetMovementDirection();
        Debug.Log(Direction);

        ItemData item = null;
        PlayerBodyPart playerBodyPart = null;

        if (Direction == Vector2.right)
        {
            playerBodyPart = playerControl.instance.playerBodyParts.playerBodyParts.Find(x => x.playerBodyPartTypes == PlayerBodyPartTypes.RightArm);
        }
        else if (Direction == Vector2.left)
        {
            playerBodyPart = playerControl.instance.playerBodyParts.playerBodyParts.Find(x => x.playerBodyPartTypes == PlayerBodyPartTypes.LeftArm);
        }

        if (playerBodyPart.gameObject.transform.childCount < 1) return; // when there is no item as a parent 

        try
        {
            Debug.Log(playerBodyPart.gameObject.name);

            item = playerBodyPart.gameObject.GetComponentInChildren<ItemHolder>().ItemReference;

            Debug.Log($"Attack Started on: {item.ItemNameText}");

            OnAttackTurn.Invoke(item);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }



    }

    private Vector2 GetMovementDirection()
    {
        float Direction = inputActions.Player.Move.ReadValue<Vector2>().x;
        if (Direction > 0) // Player is going Right
        {
            return Vector2.right;
        }
        else // Player is going Left
        {
            return Vector2.left;
        }
    }


}