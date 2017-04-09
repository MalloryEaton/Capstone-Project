/* This file is part of Runic by Ensorcelled Studios
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyAIController : MonoBehaviour
{
  /* +----------------------------------------------------------------+
   * | AI CONSTANTS                                                   |
   * +----------------------------------------------------------------+
   */
  private const string EMPTY = "Empty";
  private const string AI_TAG = "Opponent";
  private const string PLAYER_TAG = "Player";
  private const string PLACEMENT = "placement";
  private const string MOVEMENT = "movement";
  private const string FLYING = "flying";
  // For minimax
  private const int MAX_SCORE = 1000000;

  // Represents a move
  class Move
  {
    public short moveFrom = -1;
    public short moveTo = -1;
    public short removeFrom = -1;
  }

  // Represent the game board
  class Board
  {
    public short BOARD_SIZE = 24;

    public string[] board;
    private short numAIPieces;
    private short numPlayerPieces;

    public Board() {
      board = new string[24];
      for (short i = 0; i < BOARD_SIZE; i++)
        board[i] = EMPTY;

      numAIPieces = 0;
      numPlayerPieces = 0;
    }

    // Place a piece on the board
    public short placePiece(int slot, string player) {
      board[slot] = player;
      return (incPieces(player));
    }
    // Move a piece from one slot to another
    public void movePiece(int moveFrom, int moveTo) {
      string player = board[moveFrom];
      board[moveFrom] = EMPTY;
      board[moveTo] = player;
    }
    // Remove a piece from a given slot
    public short removePiece(int slot) {
      short pieces = decPieces(board[slot]);
      board[slot] = EMPTY;

      return (pieces);
    }
    // Return the number of pieces for the given players
    public short getNumPieces(string player) {
      short num = -1;
      if (player == AI_TAG)
        num = numAIPieces;
      else if (player == PLAYER_TAG)
        num = numPlayerPieces;

      return (num);
    }
    /* Increment the number of pieces for the given player
     * Returns the total number of pieces for said player
     */
    private short incPieces(string player) {
      short result = -1;
      if (player == AI_TAG) {
        numAIPieces += 1;
        result = numAIPieces;
      }
      else if (player == PLAYER_TAG) {
        numPlayerPieces += 1;
        result = numPlayerPieces;
      }
      return (result);
    }
    // Same as above, except it decrementns the values
    private short decPieces(string player) {
      short result = -1;
      if (player == AI_TAG) {
        numAIPieces -= 1;
        result = numAIPieces;
      }
      else if (player == PLAYER_TAG) {
        numPlayerPieces -= 1;
        result = numPlayerPieces;
      }
      return (result);
    }
  }

  /* +----------------------------------------------------------------+
   * | AI VARIABLES                                                   |
   * +----------------------------------------------------------------+
   */
  private GameLogicController gameLogicController;
  private System.Random rand;
  private short moveFrom;
  private short moveTo;
  private short removeFrom;
  private short[][] mills;
  private short[][] adjacentSlots;

  // for alpha-beta
  private string currentPlayer;
  private string opponentPlayer;

  /* +----------------------------------------------------------------+
   * | AI FUNCTIONS                                                   |
   * +----------------------------------------------------------------+
   */
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

    mills = new short[16][]{
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

    if (phase == PLACEMENT)
      placementPhase(ref move);
    else if (phase == MOVEMENT)
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
    List<short> potentialMills;
    short[] newMove = new short[2] { -1, -1 };

    /* Check if there are any places on the board with two pieces owned
     * by the AI and one empty slot
     */
    potentialMills = findPotentialMills(AI_TAG);
    // If there are any, check if the AI can make any mills
    foreach (short slot in potentialMills)
      if ((newMove[0] = findAdjacentOrb(slot, AI_TAG)) != -1) {
        newMove[1] = slot;
        break;
      }
    // If no mill can be made, see if we can block one
    if (newMove[0] == -1 || newMove[1] == -1) {
      potentialMills = findPotentialMills(PLAYER_TAG);
      foreach (short slot in potentialMills)
        if ((newMove[0] = findAdjacentOrb(slot, AI_TAG)) != -1) {
          newMove[1] = slot;
          break;
        }
    }        
    // No mills to make or block. Pick randomly
    if (newMove[0] == -1 || newMove[1] == -1) {
      // Piece to move
      List<short> movableAIOrbs = getMovableOrbs(AI_TAG);
      newMove[0] = movableAIOrbs[(short)rand.Next(0, movableAIOrbs.Count)];
      // Where to move it to
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

    myOrbs = getSlots(tag);
    for (short i = 0; i < myOrbs.Count; i++)
      // if you can fly, any of your orbs can move
      if (myOrbs.Count == 3)
        movableOrbs = myOrbs;
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
    emptySlots = getSlots(EMPTY);

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
   /*private short[] findMills(List<short> movableOrbs) {
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
               *
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
  */

  /* Returns all empty slots that would make a mill if an orb owned by
   * 'tag' were placed there. Returns an empty list if there are none
   */
  private List<short> findPotentialMills(string tag) {
    List<short> slotsThatMakeMill = new List<short> { };
    foreach (short[] mill in mills) {
      short pieces = 0;
      short emptySlots = 0;
      short emptySlot = -1;
      foreach (short slot in mill)
        if (gameLogicController.runeList[slot].tag == tag)
          pieces += 1;
        else if (gameLogicController.runeList[slot].tag == EMPTY) {
          emptySlots += 1;
          emptySlot = slot;
        }
      if (pieces == 2 && emptySlots == 1)
        slotsThatMakeMill.Add(emptySlot);
    }

    return (slotsThatMakeMill);
  }

  // Find an orb owned by 'tag' that's adjacent to slot 'slot'
  private short findAdjacentOrb(short slot, string tag) {
    foreach (short adjacentSlot in adjacentSlots[slot])
      if (gameLogicController.runeList[adjacentSlot].tag == tag)
        return (adjacentSlot);
    // None found
    return (-1);
  }

  // Check if a given slot is part of a mill
  private bool isInMill(short orb, string tag) {
    short numberOfSameColorOrbs;

    foreach (short[] mill in mills)
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
    foreach (short potentialMove in adjacentSlots[slot])
      if (gameLogicController.runeList[potentialMove].tag == EMPTY)
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

  /* +----------------------------------------------------------------+
   * | Minimax w/ alpha-beta pruning                                  |
   * +----------------------------------------------------------------+
   */
  private int alphaBeta(string playerTag, Board gameBoard,
                        string phase, int depth, int alpha, int beta) {
    List<Move> childMoves;

    // Depth reached
    if (depth == 0)
      return (evaluate(gameBoard, phase));
    // Game over
    else if (gameOver(gameBoard))
      return (0);
    else if ((childMoves = generateMoves(gameBoard, playerTag, phase)).Count == 0)
      // If playerTag is the same as the player we're evaluating for
      if (playerTag == currentPlayer)
        return (-MAX_SCORE);
      else
        return (MAX_SCORE);
    // determine the current turn, then return -maxScore or maxScore
    else {
      foreach (Move m in childMoves) {
        applyMove(m, playerTag, ref gameBoard, phase);

        // Maximizing player
        if (playerTag == currentPlayer) {
          alpha = getMax(alpha,
                         alphaBeta(opponentPlayer, gameBoard, phase,
                                   depth - 1, alpha, beta));
          // Check for cutoff
          if (beta <= alpha) {
            undoMove(m, playerTag, gameBoard, phase);
            break;
          }
        }
        // Minimizing player
        else {
          beta = getMin(alpha,
                        alphaBeta(playerTag, gameBoard, phase,
                                  depth - 1, alpha, beta));
          //Check for cutoff
          if (beta <= alpha) {
            undoMove(m, playerTag, gameBoard, phase);
            break;
          }
        }
        undoMove(m, playerTag, gameBoard, phase);
      }
      if (playerTag == currentPlayer)
        return (alpha);
      else
        return (beta);
    }
  }
  private int evaluate(Board gameBoard, string phase) {
    int score = 0;
    int numberOfAIMills = 0;
    int numberOfHumanMills = 0;
    int numberOfAITwoPieces = 0;
    int numberOfHumanTwoPieces = 0;

    for (short i = 0; i < 16; i++) {
      short[] mill = mills[i];
      short playerPieces = 0;
      short emptySlots = 0;
      short opponentPieces = 0;

      foreach (short slot in mill) {
        if (gameBoard.board[mill[slot]] == AI_TAG)
          playerPieces += 1;
        else if (gameBoard.board[mill[slot]] == EMPTY)
          emptySlots += 1;
        else
          opponentPieces += 1;
      }

      if (playerPieces == 3)
        numberOfAIMills += 1;
      else if (playerPieces == 2 && emptySlots == 1)
        numberOfAITwoPieces += 1;
      else if (playerPieces == 1 && emptySlots == 2)
        score += 1;
      else if (opponentPieces == 3)
        numberOfHumanMills += 1;
      else if (opponentPieces == 2 && emptySlots == 1)
        numberOfHumanTwoPieces += 1;
      else if (opponentPieces == 1 && emptySlots == 2)
        score -= 1;

      string thisTag = gameBoard.board[i];
      if (i == 4 || i == 10 || i == 13 || i == 19) {
        if (thisTag == AI_TAG)
          score += 2;
        else if (thisTag != EMPTY)
          score -= 2;
      }
      else if (i == 1 || i == 9 || i == 14 || i == 22 || i == 7 || i == 11 ||
               i == 12 || i == 16)
        if (thisTag == AI_TAG)
          score += 1;
        else if (thisTag != EMPTY)
          score -= 1;
    }

    int coefficient;

    // Account for number of mills
    if (phase == PLACEMENT)
      coefficient = 80;
    else if (phase == MOVEMENT)
      coefficient = 120;
    else
      coefficient = 180;
    score += coefficient * numberOfAIMills;
    score -= coefficient * numberOfHumanMills;

    // Account for number of total pieces
    if (phase == PLACEMENT)
      coefficient = 10;
    else if (phase == MOVEMENT)
      coefficient = 8;
    else
      coefficient = 6;
    score += coefficient * gameBoard.getNumPieces(AI_TAG);
    score -= coefficient * gameBoard.getNumPieces(PLAYER_TAG);

    // Account for total number of 2- and 1-spot free configurations
    if (phase == PLACEMENT)
      coefficient = 12;
    else
      coefficient = 10;
    score += coefficient * numberOfAITwoPieces;
    score -= coefficient * numberOfHumanMills;

    if (phase == PLACEMENT)
      coefficient = 10;
    else
      coefficient = 25;

    return (score);
  }
  // Check if the game is over
  private bool gameOver(Board gameBoard) {
    bool isOver = false;

    return (isOver);
  }
  // Get all available moves for the given player
  private List<Move> generateMoves(Board gameBoard,
                                   string playerTag, string phase) {
    List<Move> moves = new List<Move> { };
    short slot;
    short adjacentSlot;

    if (phase == PLACEMENT) {
      for (short i = 0; i < gameBoard.BOARD_SIZE; i++) {

      }
    }
    return (moves);
  }
  // Apply the move to the gameBoard
  private void applyMove(Move m, string playerTag,
                         ref Board gameBoard, string phase) {
    // If we're in the placement phase
    if (phase == PLACEMENT)
      gameBoard.placePiece(m.moveTo, playerTag);
    // In the movement phase, we also have to clear the moveFrom slot
    else
      gameBoard.movePiece(m.moveFrom, m.moveTo);

    // If a piece was removed
    if (m.removeFrom == -1)
      gameBoard.removePiece(m.removeFrom);
  }
  // Undo the given move
  private void undoMove(Move m, string playerTag,
                        Board gameBoard, string phase) {
    // If a piece was placed by the move, remove it
    if (phase == PLACEMENT)
      gameBoard.removePiece(m.moveTo);
    // If a piece was moved, move it back
    else
      gameBoard.movePiece(m.moveTo, m.moveFrom);

    // If a piece was removed, put it back
    if (m.removeFrom != -1)
      gameBoard.placePiece(m.removeFrom, playerTag);
  }
  // Check if a new mill has been made with the
  private bool madeNewMill(Board gameBoard, short slot) {
    string player = gameBoard.board[slot];

    /* For each mill that the slot could potentially be a part of,
     * check if all the slots contain the same color pieces
     */
    foreach (short[] mill in mills)
      if (mill.Contains(slot))
        if (new[] { gameBoard.board[mill[0]],
                    gameBoard.board[mill[1]],
                    gameBoard.board[mill[2]] }.All(x => x == player))
          // If so, we've found a mill
          return (true);
    // No mills were found
    return (false);
  }

  // return the greater of two numbers
  private static int getMax(int first, int second) {
    int result;
    if (first > second)
      result = first;
    else
      result = second;

    return (result);
  }
  // Return the lesser of two numbers
  private static int getMin(int first, int second) {
    int result;
    if (first < second)
      result = first;
    else
      result = second;

    return (result);
  }
}
