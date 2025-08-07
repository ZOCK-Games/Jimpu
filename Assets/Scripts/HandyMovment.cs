using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;  // Wichtig!

public class HandyMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public String ButtonName;
    public PlayerControll playerControll;

    // Wird aufgerufen, wenn der Button gedrückt wird
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ButtonName == "MoveR")
            playerControll.MovePlayerR = true;

        if (ButtonName == "MoveL")
            playerControll.MovePlayerL = true;
        if (ButtonName == "MoveUP")
            playerControll.MovePlayerUP = true; 
    }

    // Wird aufgerufen, wenn der Button losgelassen wird
    public void OnPointerUp(PointerEventData eventData)
    {
        if (ButtonName == "MoveR")
            playerControll.MovePlayerR = false;

        if (ButtonName == "MoveL")
            playerControll.MovePlayerL = false; 
        if (ButtonName == "MoveUP")
            playerControll.MovePlayerUP = false; 
    }
    
}
