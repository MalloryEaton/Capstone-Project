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
  private short[][] mills;
  private short[][] adjacentSlots;

  /*---------------------------------------------------------------------
  || AI FUNCTIONS
  -----------------------------------------------------------------------*/

  void Start() {
    rand = new System.Random();
    gameLogicController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;

    adjacentSlots = new short[24][]{
      new short[2]{ 1, 9 }, new short[3]{ 0, 2, 4 }, new short[2]{ 1, 14 },
      new short[2]{ 4, 10 }, new short[4]{ 1, 3, 5, 7 }, new short[2]{ 4, 13 },
      new short[2]{ 7, 11 }, new short[3]{ 4, 6, 8 }, new short[2]{ 7, 12 },
      new short[3]{ 0, 10, 21 }, new short[4]{ 3, 9, 11, 18 },
      new short[3]{ 6, 10, 15 }, new short[3]{ 8, 13, 17 },
      new short[4]{ 5, 12, 14, 20 }, new short[3]{ 2, 13, 23 },
      new short[2]{ 11, 16 }, new short[3]{ 15, 17, 19 }, new short[2]{ 12, 16 },
      new short[2]{ 10, 19 }, new short[4]{ 16, 18, 20, 22 }, new short[2]{ 13, 19 },
      new short[2]{ 9, 22 }, new short[3]{ 19, 21, 23 }, new short[2]{ 14, 22 } };

    mills = new short[][]{
      new short[3]{ 0, 1, 2 }, new short[3]{ 0, 9, 21 }, new short[3]{ 1, 4, 7 },
      new short[3]{ 2, 14, 23 }, new short[3]{ 3, 4, 5 }, new short[3]{ 3, 10, 18 },
      new short[3]{ 5, 13, 20 }, new short[3]{ 6, 7, 8 }, new short[3]{ 6, 11, 15 },
      new short[3]{ 8, 12, 17 }, new short[3]{ 9, 10, 11 },
      new short[3]{ 12, 13, 14 }, new short[3]{ 15, 16, 17 },
      new short[3]{ 16, 19, 22 }, new short[3]{ 18, 19, 20 },
      new short[3]{ 21, 22, 23 } };
  }

  // Gets a move from the AI
  public List<short> GetAIMove(string phase) {
    List<short> move = new List<short> { -1, -1, -1 };

    if (phase == "placement")
      placementPhase(ref move);
    else if (phase == "movement")
      movementPhase(ref move);

    if (isInMill(move[1], AI_TAG))
      removePiece(ref move);

    return (move);
  }

  // Choose a slot to place an orb //
  private void placementPhase(ref List<short> move) {
    // check if we can make a mill
    moveTo = findMill(AI_TAG);
    // If there is no mill to make, check if we can block a mill
    if (moveTo == -1)
      moveTo = findMill(PLAYER_TAG);
    // Otherwise, remove a piece at random
    if (moveTo == -1) {
      moveTo = (short)rand.Next(0, 24);
      while (gameLogicController.runeList[moveTo].tag != EMPTY)
        moveTo = (short)rand.Next(0, 24);
    }

    move[1] = moveTo;
  }

  // Move a piece to an empty slot
  private void movementPhase(ref List<short> move) {
    List<short> orbsAvailableToMove = new List<short> { };
    short[] newMove;

    orbsAvailableToMove = getMovableOrbs(AI_TAG);

    // Check if we can make a mill
    newMove = findMill(orbsAvailableToMove);
    // If not, check if we can block a mill
    if (newMove[0] == -1 || newMove[1] == -1) {
      List<short> movableOpponentOrbs = new List<short> { };
      movableOpponentOrbs = getMovableOrbs(PLAYER_TAG);
      newMove = findMill(movableOpponentOrbs);
    }
    // No mills to make or block. Pick randomly
    if (newMove[0] == -1 || newMove[1] == -1) {
      // Piece to move
      newMove[0] = orbsAvailableToMove[(short)rand.Next(0, orbsAvailableToMove.Count)];
      List<short> potentialMoves = placesToMove(newMove[0]);
      newMove[1] = potentialMoves[(short)rand.Next(0, potentialMoves.Count)];
    }

    move[0] = newMove[0];
    move[1] = newMove[1];
  }

  // Chooses an opponent's piece to remove
  private void removePiece(ref List<short> move) {
    short removeFrom;
    List<short> opponentRunes = new List<short> { };

    for (short i = 0; i <= 23; i++)
      if (gameLogicController.runeList[i].tag == PLAYER_TAG)
        opponentRunes.Add(i);

    removeFrom = (short)rand.Next(0, opponentRunes.Count);
    while (!canBeRemoved(opponentRunes[removeFrom]))
      removeFrom = (short)rand.Next(0, opponentRunes.Count);

    move[2] = opponentRunes[removeFrom];
  }

  // Returns a list of the current player's movable orbs
  private List<short> getMovableOrbs(string tag) {
    List<short> myOrbs;
    List<short> movableOrbs = new List<short> { };

    myOrbs = gameLogicController.MakeListOfRunesForCurrentPlayer();
    for (short i = 0; i < myOrbs.Count; i++)
      // if you can fly, any of your orbs can move
      if (gameLogicController.CanFly()) {
        if (gameLogicController.runeList[myOrbs[i]].tag == tag)
          movableOrbs.Add(myOrbs[i]);
      }
      // Otherwise, an orb is only available to move if it is adjacent to an empty slot
      else {
        foreach (short adjacentRune in gameLogicController.dictionaries.adjacencyDictionary[myOrbs[i]])
          if (gameLogicController.runeList[adjacentRune].tag == EMPTY) {
            movableOrbs.Add(myOrbs[i]);
            break;
          }
      }

    return (movableOrbs);
  }

  // Find a potential mill in the placement phase
  private short findMill(string tag) {
    List<short> emptySlots;
    emptySlots = getEmptySlots();

    /* For each possible mill, if two pieces are the AI and the
    * third is empty, return the empty slot
    */
    foreach (short[] mill in mills) {
      short AIPieces;
      short emptySlot;

      AIPieces = 0;
      emptySlot = -1;

      // Count the number of AI pieces and mark the empty slot
      foreach (short slot in mill)
        if (gameLogicController.runeList[slot].tag == tag)
          AIPieces += 1;
        else if (gameLogicController.runeList[slot].tag == EMPTY)
          emptySlot = slot;

      /* If exactly two of the pieces are the AI and there is 1 empty
       * slot, return that empty slot
       */
      if (AIPieces == 2 && emptySlot != -1)
        return (emptySlot);
    }
    // If no mills can be made, return -1
    return (-1);
  }

  /* Find a potential mill in the movement/flying phase
   * On success, returns short[2] { slotToMoveFrom, slotToMoveTo }
   * On failure, returns short[2] { -1, -1 }
   */
  private short[] findMill(List<short> movableOrbs) {
    string currentTag;
    short currentPieces;

    currentTag = gameLogicController.runeList[movableOrbs[0]].tag;

    // Find available slots to move to
    foreach (short orb in movableOrbs) {
      foreach (short adjacentSlot in adjacentSlots[orb])
        if (gameLogicController.runeList[adjacentSlot].tag == EMPTY)
          // An available move. Check for possible mills
          foreach (short[] mill in mills)
            if (mill.Contains(adjacentSlot) && !mill.Contains(orb)) {
              /* If the mill contains the adjacent slot and does not
               * contain the original sloto, check if moving there will
               * make a mill
               */
              currentPieces = 0;
              foreach (short slot in mill)
                if (gameLogicController.runeList[slot].tag == currentTag)
                  currentPieces += 1;

              if (currentPieces == 2)
                return (new short[] { orb, adjacentSlot });
            }
    }
    return (new short[] { -1, -1 });
  }

  private List<short> getEmptySlots() {
    List<short> emptySlots = new List<short> { };

    for (short i = 0; i <= 23; i++)
      if (gameLogicController.runeList[i].tag == EMPTY)
        emptySlots.Add(i);

    return (emptySlots);
  }

  private bool isInMill(short orb, string tag) {
    short numberOfSameColorOrbs;

    foreach (short[] mill in mills)
      if (mill.Contains(orb)) {
        numberOfSameColorOrbs = 0;
        foreach (short slot in mill)
          if (slot != orb && gameLogicController.runeList[slot].tag == tag)
            numberOfSameColorOrbs += 1;

        if (numberOfSameColorOrbs == 2)
          return (true);
      }
    return (false);
  }

  private bool canBeRemoved(short orb) {
    if (gameLogicController.AllRunesAreInMills() ||
      !isInMill(orb, gameLogicController.runeList[orb].tag))
      return (true);
    else
      return (false);
  }

  private List<short> placesToMove(short slot) {
    List<short> emptyMoves = new List<short> { };
    foreach (short potentialMove in adjacentSlots[slot])
      if (gameLogicController.runeList[potentialMove].tag == EMPTY)
        emptyMoves.Add(potentialMove);
    return (emptyMoves);
  }
}
