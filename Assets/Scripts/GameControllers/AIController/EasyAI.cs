using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using AI;

namespace AI
{
  public class EasyAI : MonoBehaviour
  {
    private System.Random rand;
    private GameLogicController gameLogicController;
    private short moveFrom;
    private short moveTo;
    private short removeFrom;

    void Start() {
      rand = new System.Random();
      gameLogicController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
    }

    public List<short> getAIMove(string phase) {
      List<short> move = new List<short> { -1, -1, -1 };
      if (phase == Phases.PLACEMENT)
        placementPhase(ref move);
      else if (phase == Phases.MOVEMENT)
        movementPhase(ref move, Phases.MOVEMENT);
      else
        movementPhase(ref move, Phases.FLYING);

      if (isInMill(move[1], Tags.AI_TAG))
        removePiece(ref move);

      return (move);
    }
    
    // Choose a slot to place an orb //
    private void placementPhase(ref List<short> move) {
      // check if we can make a mill
      moveTo = findMill(Tags.AI_TAG);
      // If there is no mill to make, check if we can block a mill
      if (moveTo == -1)
        moveTo = findMill(Tags.HUMAN_TAG);
      // Otherwise, remove a piece at random
      if (moveTo == -1) {
        moveTo = (short)rand.Next(0, 24);
        while (gameLogicController.runeList[moveTo].tag != Tags.EMPTY)
          moveTo = (short)rand.Next(0, 24);
      }

      move[1] = moveTo;
    }

    // Move a piece to an empty slot
    private void movementPhase(ref List<short> move, string phase) {
      List<short[]> potentialMills;
      //short moveTo;
      short[] newMove = new short[2] { -1, -1 };

      /* Check if there are any places on the board with two pieces owned
       * by the AI and one empty slot
       */
      potentialMills = findPotentialMills(Tags.AI_TAG);
      // If there are any, check if the AI can make any mills
      foreach (short[] mill in potentialMills) {
        if (phase == Phases.MOVEMENT)
          newMove[0] = findAdjacentOrb(mill, Tags.AI_TAG);
        else { // flying
          List<short> movableOrbs = getMovableOrbs(Tags.AI_TAG);
          newMove[0] = movableOrbs[(short)rand.Next(0, movableOrbs.Count)];
        }
        if (newMove[0] != -1 && !mill.Contains(newMove[0])) {
          newMove[1] = findEmptySlotInMill(mill);
          break;
        }
      }
      // If no mill can be made, see if we can block one
      if (newMove[0] == -1 || newMove[1] == -1) {
        potentialMills = findPotentialMills(Tags.HUMAN_TAG);
        foreach (short[] mill in potentialMills) {
          if (phase == Phases.MOVEMENT)
            newMove[0] = findAdjacentOrb(mill, Tags.AI_TAG);
          else { // flying
            List<short> movableOrbs = getMovableOrbs(Tags.AI_TAG);
            newMove[0] = movableOrbs[(short)rand.Next(0, movableOrbs.Count)];
          }
          if (newMove[0] != -1 && !mill.Contains(newMove[0])) {
            newMove[1] = findEmptySlotInMill(mill);
            break;
          }
        }
      }
      // No mills to make or block. Pick randomly
      if (newMove[0] == -1 || newMove[1] == -1) {
        // Piece to move
        List<short> movableAIOrbs = getMovableOrbs(Tags.AI_TAG);
        newMove[0] = movableAIOrbs[(short)rand.Next(0, movableAIOrbs.Count)];
        // Where to move it to
        List<short> potentialMoves = placesToMove(newMove[0]);
        newMove[1] = potentialMoves[(short)rand.Next(0, potentialMoves.Count)];
      }

      move[0] = newMove[0];
      move[1] = newMove[1];
    }

    private short findEmptySlotInMill(short[] mill) {
      short emptySlot = -1;
      foreach (short slot in mill)
        if (gameLogicController.runeList[slot].tag == Tags.EMPTY) {
          emptySlot = slot;
          break;
        }
      return (emptySlot);
    }

    // Chooses an opponent's piece to remove
    private void removePiece(ref List<short> move) {
      short removeFrom;
      List<short> opponentRunes = new List<short> { };

      for (short i = 0; i <= 23; i++)
        if (gameLogicController.runeList[i].tag == Tags.HUMAN_TAG)
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

      myOrbs = getSlots(tag);
      for (short i = 0; i < myOrbs.Count; i++)
        // if you can fly, any of your orbs can move
        if (myOrbs.Count == GameSettings.FLYING_AT)
          movableOrbs = myOrbs;
        // Otherwise, an orb is only available to move if it is adjacent to an empty slot
        else {
          foreach (short adjacentRune in gameLogicController.dictionaries.adjacencyDictionary[myOrbs[i]])
            if (gameLogicController.runeList[adjacentRune].tag == Tags.EMPTY) {
              movableOrbs.Add(myOrbs[i]);
              break;
            }
        }

      return (movableOrbs);
    }

    // Find a potential mill in the placement phase
    private short findMill(string tag) {
      //List<short> emptySlots;
      //emptySlots = getSlots(Tags.EMPTY);

      /* For each possible mill, if two pieces are the AI and the
      * third is empty, return the empty slot
      */
      foreach (short[] mill in Configurations.MILLS) {
        short AIPieces;
        short emptySlot;

        AIPieces = 0;
        emptySlot = -1;

        // Count the number of AI pieces and mark the empty slot
        foreach (short slot in mill)
          if (gameLogicController.runeList[slot].tag == tag)
            AIPieces += 1;
          else if (gameLogicController.runeList[slot].tag == Tags.EMPTY)
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
    
    /* Returns all empty slots that would make a mill if an orb owned by
     * 'tag' were placed there. Returns an empty list if there are none
     */
    private List<short[]> findPotentialMills(string tag) {
      List<short[]> potentialMills = new List<short[]> { };
      foreach (short[] mill in Configurations.MILLS) {
        short pieces = 0;
        short emptySlots = 0;

        foreach (short slot in mill)
          if (gameLogicController.runeList[slot].tag == tag)
            pieces += 1;
          else if (gameLogicController.runeList[slot].tag == Tags.EMPTY)
            emptySlots += 1;
        if (pieces == 2 && emptySlots == 1)
          potentialMills.Add(mill);
      }

      return (potentialMills);
    }

    // Find an orb owned by 'tag' that's adjacent to slot 'slot'
    private short findAdjacentOrb(short[] mill, string tag) {
      short emptySlot = findEmptySlotInMill(mill);
      foreach (short adjacentSlot in Configurations.ADJACENT_SLOTS[emptySlot])
        if (!mill.Contains(adjacentSlot) &&
          gameLogicController.runeList[adjacentSlot].tag == tag)
          return (adjacentSlot);
      // None found
      return (-1);
    }

    // Check if a given slot is part of a mill
    private bool isInMill(short orb, string tag) {
      short numberOfSameColorOrbs;

      foreach (short[] mill in Configurations.MILLS)
        if (mill.Contains(orb)) {
          numberOfSameColorOrbs = 0;
          foreach (short slot in mill)
            if (slot != orb &&
                gameLogicController.runeList[slot].tag == tag)
              numberOfSameColorOrbs += 1;

          if (numberOfSameColorOrbs == 2)
            return (true);
        }
      return (false);
    }

    // Check if a given slot can be removed
    private bool canBeRemoved(short orb) {
      if (gameLogicController.AllRunesAreInMills() ||
        !isInMill(orb, gameLogicController.runeList[orb].tag))
        return (true);
      else
        return (false);
    }

    // Find all the available moves (empty adjacent slots) for an orb
    private List<short> placesToMove(short slot) {
      List<short> emptyMoves = new List<short> { };
      foreach (short potentialMove in Configurations.ADJACENT_SLOTS[slot])
        if (gameLogicController.runeList[potentialMove].tag == Tags.EMPTY)
          emptyMoves.Add(potentialMove);
      return (emptyMoves);
    }

    // Return a list of all the slots containing a given tag
    private List<short> getSlots(string tag) {
      List<short> slots = new List<short> { };

      for (short i = 0; i <= 23; i++)
        if (gameLogicController.runeList[i].tag == tag)
          slots.Add(i);
      return (slots);
    }
  }
}