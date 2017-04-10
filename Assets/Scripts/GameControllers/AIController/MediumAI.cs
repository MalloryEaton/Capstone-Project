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
    private const short DEPTH = 4;

    private string currentPlayer;
    private string opponentPlayer;
    private int numberOfMoves = 0;
    private int movesThatRemove = 0;

    public List<short> GetMediumAIMove(Board gameBoard, string phase) {
      Move move;

      if (phase == Phases.PLACEMENT)
        move = makePlacement(gameBoard);
      else
        move = makeMovement(gameBoard);

      return (move.list());
    }
    // Decide where to place an orb
    private Move makePlacement(Board gameBoard) {
      List<Move> moves = generateMoves(gameBoard, Tags.AI_TAG, Phases.PLACEMENT);
      foreach (Move m in moves) {
        applyMove(m, Tags.AI_TAG, ref gameBoard, Phases.PLACEMENT);
        m.score += alphaBeta(Tags.PLAYER_TAG, gameBoard, Phases.PLACEMENT,
                             DEPTH, int.MaxValue, int.MaxValue);
        undoMove(m, Tags.AI_TAG, ref gameBoard, Phases.PLACEMENT);
      }

      // Sort the moves by their score
      moves = moves.OrderBy(o => o.score).ToList();

      return (moves[0]);
    }

    private Move makeMovement(Board gameBoard) {
      Move m = new Move();

      return (m);
    }

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
              undoMove(m, playerTag, ref gameBoard, phase);
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
              undoMove(m, playerTag, ref gameBoard, phase);
              break;
            }
          }
          undoMove(m, playerTag, ref gameBoard, phase);
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

      // Each empty slot is an available move
      if (phase == Phases.PLACEMENT)
        for (short i = 0; i < gameBoard.BOARD_SIZE; i++) {
          Move m = new Move();
          if (gameBoard.board[i] == Tags.EMPTY) {
            m.moveTo = i;
            // Place piece
            gameBoard.board[i] = playerTag;
            checkIfMoveMakesMill(gameBoard, playerTag, ref moves, m);
            // Undo that placement
            gameBoard.board[i] = Tags.EMPTY;
          }
        }
      /* Each empty slot adjacent to an orb is an available move for
       * said orb
       */
      else if (phase == Phases.MOVEMENT) {
        for (short i = 0; i < gameBoard.BOARD_SIZE; i++)
          if (gameBoard.board[i] == playerTag) {
            short[] adj = Configurations.ADJACENT_SLOTS[i];
            for (short j = 0; j < adj.Count(); j++) {
              Move m = new Move();
              m.moveFrom = i;
              if (gameBoard.board[j] == Tags.EMPTY) {
                m.moveTo = j;
                // Make move
                gameBoard.board[i] = Tags.EMPTY;
                gameBoard.board[j] = playerTag;
                checkIfMoveMakesMill(gameBoard, playerTag, ref moves, m);
                // Undo that move
                gameBoard.board[i] = playerTag;
                gameBoard.board[j] = Tags.EMPTY;
              }
            }
          }
      }
      // Each empty slot is an available move for each piece
      else if (phase == Phases.FLYING) {
        List<short> emptySlots = new List<short>();
        List<short> playerOrbs = new List<short>();

        for (short i = 0; i < gameBoard.BOARD_SIZE; i++)
          if (gameBoard.board[i] == playerTag)
            playerOrbs.Add(i);
          else if (gameBoard.board[i] == Tags.EMPTY)
            emptySlots.Add(i);

        foreach (short orb in playerOrbs) {
          Move m = new Move();
          // Since we're moving from here, we can set this as empty
          gameBoard.board[orb] = Tags.EMPTY;
          m.moveFrom = orb;
          foreach (short slot in emptySlots) {
            gameBoard.board[slot] = playerTag;
            m.moveTo = slot;
            checkIfMoveMakesMill(gameBoard, playerTag, ref moves, m);
            gameBoard.board[slot] = Tags.EMPTY;
          }
          gameBoard.board[orb] = playerTag;
        }
      }

      numberOfMoves += moves.Count;

      return (moves);
    }
    private void checkIfMoveMakesMill(Board gameBoard, string playerTag,
                           ref List<Move> moves, Move m) {
      /* If we made a mill, add a move for each possible piece that can
       * be removed
       */
      if (isInMill(gameBoard, m.moveTo)) {
        for (short i = 0; i < gameBoard.BOARD_SIZE; i++)
          if (gameBoard.board[i] != playerTag &&
              gameBoard.board[i] != Tags.EMPTY &&
              canBeRemoved(gameBoard, i)) {
            m.removeFrom = i;
            moves.Add(m);
            movesThatRemove += 1;
          }
      }
      else
        moves.Add(m);
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
      if (m.removeFrom != -1)
        gameBoard.removePiece(m.removeFrom);
    }
    // Undo the given move
    private void undoMove(Move m, string playerTag,
                          ref Board gameBoard, string phase) {
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
    private bool isInMill(Board gameBoard, short slot) {
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
    // Checks if a given orb is eligible for removal
    private bool canBeRemoved(Board gameBoard, short orb) {
      if (!isInMill(gameBoard, orb) ||
        allInMill(gameBoard, gameBoard.board[orb]))
        return (true);
      else
        return (false);
    }
    // Checks if all orbs of a given player are in a mill
    private bool allInMill(Board gameBoard, string playerTag) {
      for (short i = 0; i < gameBoard.BOARD_SIZE; i++)
        if (gameBoard.board[i] == playerTag)
          if (!isInMill(gameBoard, i))
            return (false);
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