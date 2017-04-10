using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using AI;

namespace AI
{
  public class MediumAI : MonoBehaviour
  {
    /* +----------------------------------------------------------------+
     * | Minimax w/ alpha-beta pruning                                  |
     * +----------------------------------------------------------------+
     */
    private const int MAX_SCORE = 1000000;

    private string currentPlayer;
    private string opponentPlayer;

    private int alphaBeta(string playerTag, Board gameBoard,
                          string phase, int depth, int alpha, int beta) {
      List<Move> childMoves;

      // Depth reached
      if (depth == 0)
        return (evaluateBoardstate(gameBoard, phase));
      // Game over
      else if (isGameOver(gameBoard))
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
    private int evaluateBoardstate(Board gameBoard, string phase) {
      int score = 0;
      int numberOfAIMills = 0;
      int numberOfHumanMills = 0;
      int numberOfAITwoPieces = 0;
      int numberOfHumanTwoPieces = 0;

      for (short i = 0; i < 16; i++) {
        short[] mill = Configurations.MILLS[i];
        short playerPieces = 0;
        short emptySlots = 0;
        short opponentPieces = 0;

        foreach (short slot in mill) {
          if (gameBoard.board[mill[slot]] == Tags.AI_TAG)
            playerPieces += 1;
          else if (gameBoard.board[mill[slot]] == Tags.EMPTY)
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
          if (thisTag == Tags.AI_TAG)
            score += 2;
          else if (thisTag != Tags.EMPTY)
            score -= 2;
        }
        else if (i == 1 || i == 9 || i == 14 || i == 22 || i == 7 || i == 11 ||
                 i == 12 || i == 16)
          if (thisTag == Tags.AI_TAG)
            score += 1;
          else if (thisTag != Tags.EMPTY)
            score -= 1;
      }

      int coefficient;

      // Account for number of mills
      if (phase == Phases.PLACEMENT)
        coefficient = 80;
      else if (phase == Phases.MOVEMENT)
        coefficient = 120;
      else
        coefficient = 180;
      score += coefficient * numberOfAIMills;
      score -= coefficient * numberOfHumanMills;

      // Account for number of total pieces
      if (phase == Phases.PLACEMENT)
        coefficient = 10;
      else if (phase == Phases.MOVEMENT)
        coefficient = 8;
      else
        coefficient = 6;
      score += coefficient * gameBoard.getNumPieces(Tags.AI_TAG);
      score -= coefficient * gameBoard.getNumPieces(Tags.PLAYER_TAG);

      // Account for total number of 2- and 1-spot free configurations
      if (phase == Phases.PLACEMENT)
        coefficient = 12;
      else
        coefficient = 10;
      score += coefficient * numberOfAITwoPieces;
      score -= coefficient * numberOfHumanMills;

      if (phase == Phases.PLACEMENT)
        coefficient = 10;
      else
        coefficient = 25;

      return (score);
    }
    // Check if the game is over
    private bool isGameOver(Board gameBoard) {
      bool isOver = false;

      return (isOver);
    }
    // Get all available moves for the given player
    private List<Move> generateMoves(Board gameBoard,
                                     string playerTag, string phase) {
      List<Move> moves = new List<Move> { };
      short slot;
      short adjacentSlot;

      if (phase == Phases.PLACEMENT)
        for (short i = 0; i < gameBoard.BOARD_SIZE; i++) {
          Move m = new Move();
          slot = i;
          if (gameBoard.board[i] == Tags.EMPTY) {
            gameBoard.board[i] = playerTag;
            m.moveTo = i;
            checkMove(gameBoard, playerTag, ref moves, m);
            gameBoard.board[i] = Tags.EMPTY;
          }
        }
      else if (phase == Phases.MOVEMENT) {

      }
      return (moves);
    }
    private void checkMove(Board gameBoard, string playerTag,
                           ref List<Move> moves, Move m) {

    }
    // Apply the move to the gameBoard
    private void applyMove(Move m, string playerTag,
                           ref Board gameBoard, string phase) {
      // If we're in the placement phase
      if (phase == Phases.PLACEMENT)
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
      if (phase == Phases.PLACEMENT)
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
      foreach (short[] mill in Configurations.MILLS)
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
}