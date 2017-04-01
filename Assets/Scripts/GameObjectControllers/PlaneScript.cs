using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    private GameLogicController gameController;

    void Start()
    {
        gameController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
    }

    private void OnMouseDown()
    {
        if (!gameController.waitingOnAnimation && !gameController.waitingOnOtherPlayer && gameController.gamePhase == "movementPlace")
        {
            gameController.gamePhase = "movementPickup";
            gameController.ShowAvailableMoves();
        }
        print("plane");
    }
}
