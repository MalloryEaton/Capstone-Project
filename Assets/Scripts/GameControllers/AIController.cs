using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyAIController : MonoBehaviour
{

    /*---------------------------------------------------------------------
    || AI VARIABLES
    -----------------------------------------------------------------------*/
    private const string EMPTY = "Empty";
    private const string AI_TAG = "Opponent";
    private const string PLAYER_TAG = "Player";

    private GameLogicController gameLogicController;
    private System.Random rand;
    private short moveFrom;
    private short moveTo;
    private short removeFrom;

    /*---------------------------------------------------------------------
    || AI FUNCTIONS
    -----------------------------------------------------------------------*/

    void Start()
    {
        rand = new System.Random();
        gameLogicController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
    }

    // Gets a move from the AI
    public List<short> GetAIMove(string phase)
    {
        List<short> move = new List<short> { -1, -1, -1 };

        if (phase == "placement")
            placementPhase(ref move);
        else if (phase == "movement")
            movementPhase(ref move);

        if (gameLogicController.RuneIsInMill(move[1]))
            removePiece(ref move);

        return (move);
    }

    // Choose a slot to place an orb //
    private void placementPhase(ref List<short> move)
    {
        moveTo = (short)rand.Next(0, 24);
        while (gameLogicController.runeList[moveTo].tag != EMPTY)
            moveTo = (short)rand.Next(0, 24);

        move[1] = moveTo;
    }

    // Move a piece to an empty slot
    private void movementPhase(ref List<short> move)
    {
        List<short> orbsAvailableToMove = new List<short> { };
        orbsAvailableToMove = getMovableOrbs();

        moveFrom = (short)rand.Next(0, orbsAvailableToMove.Count);

        moveTo = (short)rand.Next(0, orbsAvailableToMove.Count);
        while (gameLogicController.runeList[moveTo].tag != EMPTY)
            moveTo = (short)rand.Next(0, orbsAvailableToMove.Count);

        move[0] = moveFrom;
        move[1] = moveTo;
    }

    // Chooses an opponent's piece to remove
    private void removePiece(ref List<short> move)
    {
        short removeFrom;
        List<short> opponentRunes = new List<short> { };

        for (short i = 0; i <= 23; i++)
            if (gameLogicController.runeList[i].tag == PLAYER_TAG)
                opponentRunes.Add(i);

        removeFrom = (short)rand.Next(0, opponentRunes.Count);
        while (!gameLogicController.RuneCanBeRemoved(removeFrom))
            removeFrom = (short)rand.Next(0, opponentRunes.Count);

        move[2] = removeFrom;
    }

    // Returns a list of the current player's movable orbs
    private List<short> getMovableOrbs()
    {
        List<short> myOrbs;
        List<short> movableOrbs = new List<short> { };

        myOrbs = gameLogicController.MakeListOfRunesForCurrentPlayer();
        for (short i = 0; i < myOrbs.Count; i++)
            // if you can fly, any of your orbs can move
            if (gameLogicController.CanFly())
            {
                if (gameLogicController.runeList[myOrbs[i]].tag == AI_TAG)
                    movableOrbs.Add(myOrbs[i]);
            }
            // Other wise, an orb is only available to move if it is adjacent to an empty slot
            else
            {
                foreach (short adjacentRune in gameLogicController.dictionaries.adjacencyDictionary[myOrbs[i]])
                    if (gameLogicController.runeList[adjacentRune].tag == EMPTY)
                    {
                        movableOrbs.Add(myOrbs[i]);
                        break;
                    }
            }
        return (movableOrbs);
    }
}