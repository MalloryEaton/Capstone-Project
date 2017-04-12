using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this file is used on the gameboards to allow stuff to happen with clicking off runes
public class PlaneScript : MonoBehaviour
{
    private GameLogicController gameController;

    void Start()
    {
        gameController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
    }

    private void OnMouseDown()
    {
        if (gameController != null && (!gameController.waitingOnAnimation && !gameController.waitingOnOtherPlayer && gameController.gamePhase == "movementPlace"))
        {
            gameController.gamePhase = "movementPickup";
            gameController.ShowAvailableMoves();
        }
        print("plane");
    }
}
