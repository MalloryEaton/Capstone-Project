using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using AI;

namespace AI
{
  public class MinimaxAI : MonoBehaviour
  {
    private const int MAX_SCORE = 1000000;
    private short startingDepth;
    private string difficultyLevel;

    //private int numberOfMoves = 0;
    //private int movesThatRemove = 0;

    /* +-----------------------+
     * | Interfacing Functions |
     * +-----------------------+
     */
    // Returns a List of an AI move
    public List<short> getAIMove(ref Board gameBoard, string difficulty) {
      if (difficulty == Difficulties.MEDIUM) {
        startingDepth = 1;
        difficultyLevel = Difficulties.MEDIUM;
      }
      else if (difficulty == Difficulties.HARD) {
        startingDepth = 1;
        difficultyLevel = Difficulties.HARD;
      }
      Move move;
      string phase = gameBoard.getPhase(Tags.AI_TAG);

      if (phase == Phases.PLACEMENT)
        move = makePlacement(ref gameBoard);
      else
        move = makeMovement(ref gameBoard);

      return (move.list());
    }
    // Decide where to place an orb
    private Move makePlacement(ref Board gameBoard) {
      List<Move> moves = generateMoves(ref gameBoard, Tags.AI_TAG,
                                       (short)(startingDepth - 1));
      int alpha = int.MinValue;
      int beta = int.MaxValue;

      foreach (Move m in moves) {
        applyMove(m, Tags.AI_TAG, ref gameBoard, Phases.PLACEMENT);
        m.score += alphaBeta(Tags.AI_TAG, ref gameBoard,
                             (short)(startingDepth - 1),
                             ref alpha, ref beta, m);
        undoMove(m, Tags.AI_TAG, ref gameBoard, Phases.PLACEMENT);
      }

      // Sort the moves by their score
      moves = moves.OrderByDescending(o => o.score).ToList();

      // Randomly pick from the best moves
      List<Move> bestMoves = new List<Move> { };
      int bestScore = moves[0].score;
      foreach (Move m in moves) {
        int nextScore = m.score;
        if (nextScore == bestScore)
          bestMoves.Add(m);
        else
          break;
      }

      System.Random rand = new System.Random();
      return (moves[rand.Next(0, bestMoves.Count)]);
    }
    // Decide where to move an orb from/to
    private Move makeMovement(ref Board gameBoard) {
      List<Move> moves = generateMoves(ref gameBoard, Tags.AI_TAG,
                                       (short)(startingDepth - 1));
      int alpha = int.MinValue;
      int beta = int.MaxValue;

      foreach (Move m in moves) {
        applyMove(m, Tags.AI_TAG, ref gameBoard, Phases.MOVEMENT);
        m.score += alphaBeta(Tags.AI_TAG, ref gameBoard,
                             (short)(startingDepth - 1), ref alpha, ref beta,
                             m);
        undoMove(m, Tags.AI_TAG, ref gameBoard, Phases.MOVEMENT);
      }

      // Sort the moves by their score
      moves = moves.OrderByDescending(o => o.score).ToList();

      // Randomly pick from the best moves
      List<Move> bestMoves = new List<Move> { };
      int bestScore = moves[0].score;
      foreach (Move m in moves) {
        int nextScore = m.score;
        if (nextScore == bestScore)
          bestMoves.Add(m);
        else
          break;
      }

      System.Random rand = new System.Random();
      return (moves[rand.Next(0, bestMoves.Count)]);
    }

    /* +------------------------------------------------+
     * | Minimax Search w/ Alpha-beta Pruning Functions |
     * +------------------------------------------------+
     */
    private int alphaBeta(string playerTag, ref Board gameBoard, short depth,
                          ref int alpha, ref int beta, Move previousMove) {
      List<Move> childMoves;
      string phase = gameBoard.getPhase(playerTag);
      short winner = 0;

      // Check for a winner
      calculateWinner(ref gameBoard, ref winner, ref depth);

      // Depth reached
      if (depth == 0) {
        int score = 0;
        score = evaluateBoardstate(ref gameBoard, phase, previousMove, depth);

        return (score);
      }
      // Game over
      else if (winner != 0)
        if (winner == 1) // AI won
          return (MAX_SCORE);
        else // Human won
          return (-MAX_SCORE);
      // Determine the current turn, then return -maxScore or maxScore
      else {
        string opponent;
        if (playerTag == Tags.AI_TAG)
          opponent = Tags.HUMAN_TAG;
        else
          opponent = Tags.AI_TAG;

        childMoves = generateMoves(ref gameBoard, opponent, depth);

        foreach (Move m in childMoves) {
          applyMove(m, playerTag, ref gameBoard, phase);

          // Maximizing player
          if (playerTag == Tags.AI_TAG) {
            alpha = Math.Max(alpha, alphaBeta(Tags.HUMAN_TAG, ref gameBoard,
                                            (short)(depth - 1), ref alpha,
                                            ref beta, m));
            // Check for cutoff
            if (beta <= alpha) {
              undoMove(m, playerTag, ref gameBoard, phase);
              break;
            }
          }
          // Minimizing player
          else {
            beta = Math.Min(beta, alphaBeta(Tags.AI_TAG, ref gameBoard,
                                           (short)(depth - 1), ref alpha,
                                           ref beta, m));
            //Check for cutoff
            if (beta <= alpha) {
              undoMove(m, playerTag, ref gameBoard, phase);
              break;
            }
          }
          undoMove(m, playerTag, ref gameBoard, phase);
        }
        if (playerTag == Tags.AI_TAG)
          return (alpha);
        else
          return (beta);
      }
    }

    // Get all available moves for the given player
    private List<Move> generateMoves(ref Board gameBoard, string playerTag, short currentDepth) {
      List<Move> moves = new List<Move> { };
      string phase = gameBoard.getPhase(playerTag);

      // Each empty slot is an available move
      if (phase == Phases.PLACEMENT)
        for (short i = 0; i < gameBoard.BOARD_SIZE; i++) {
          Move m = new Move();
          if (gameBoard.board[i] == Tags.EMPTY) {
            m.moveTo = i;
            // Place piece
            gameBoard.board[i] = playerTag;
            moves = checkPotentialRemovals(ref gameBoard, playerTag, moves, m);
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
              if (gameBoard.board[adj[j]] == Tags.EMPTY) {
                m.moveTo = adj[j];
                // Make move
                gameBoard.board[i] = Tags.EMPTY;
                gameBoard.board[adj[j]] = playerTag;
                moves = checkPotentialRemovals(ref gameBoard, playerTag, moves, m);
                // Undo that move
                gameBoard.board[i] = playerTag;
                gameBoard.board[adj[j]] = Tags.EMPTY;
              }
            }
          }
      }
      // Each empty slot is an available move for each piece
      else if (phase == Phases.FLYING) {
        List<short> emptySlots = new List<short>();
        List<short> playerOrbs = new List<short>();

        playerOrbs = getSlots(ref gameBoard, playerTag);
        emptySlots = getSlots(ref gameBoard, Tags.EMPTY);

        foreach (short orb in playerOrbs) {
          foreach (short slot in emptySlots) {
            Move m = new Move();
            // Since we're moving from here, we can set this as empty
            gameBoard.board[orb] = Tags.EMPTY;
            m.moveFrom = orb;

            gameBoard.board[slot] = playerTag;
            m.moveTo = slot;
            moves = checkPotentialRemovals(ref gameBoard, playerTag, moves, m);
            gameBoard.board[slot] = Tags.EMPTY;
          }
          gameBoard.board[orb] = playerTag;
        }
      }

      if (currentDepth > 3) {
        foreach (Move m in moves) {
          if (phase == Phases.PLACEMENT)
            gameBoard.placePiece(m.moveTo, playerTag);
          else
            gameBoard.movePiece(m.moveFrom, m.moveTo);
          if (m.removeFrom != -1)
            gameBoard.removePiece(m.removeFrom);

          m.score = evaluateBoardstate(ref gameBoard, phase, m,
                                       (short)(currentDepth - 1));

          if (phase == Phases.PLACEMENT)
            gameBoard.removePiece(m.moveTo);
          else
            gameBoard.movePiece(m.moveTo, m.moveFrom);
          if (m.removeFrom != -1)
            gameBoard.placePiece(m.removeFrom, playerTag);
        }
        if (playerTag == Tags.AI_TAG)
          moves = moves.OrderByDescending(o => o.score).ToList();
        else
          moves = moves.OrderBy(o => o.score).ToList();
      }

      return (moves);
    }
    private List<Move> checkPotentialRemovals(ref Board gameBoard, string playerTag,
                                              List<Move> moves, Move m) {
      /* If we made a mill, add a move for each possible piece that can
       * be removed
       */
      if (isInMill(ref gameBoard, m.moveTo)) {
        for (short i = 0; i < gameBoard.BOARD_SIZE; i++)
          if (gameBoard.board[i] != playerTag &&
              gameBoard.board[i] != Tags.EMPTY &&
              canBeRemoved(ref gameBoard, i)) {
            m.removeFrom = i;
            moves.Add(m);
          }
      }
      else
        moves.Add(m);

      return (moves);
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
      if (phase == Phases.PLACEMENT) {
        gameBoard.removePiece(m.moveTo);
        gameBoard.decPlacedPieces(playerTag);
      }
      // If a piece was moved, move it back
      else
        gameBoard.movePiece(m.moveTo, m.moveFrom);

      // If a piece was removed, put it back
      if (m.removeFrom != -1) {
        gameBoard.placePiece(m.removeFrom, playerTag);
        gameBoard.decPlacedPieces(playerTag);
      }
    }

    /* +--------------------------------+
    * | Heuristic Evaluation Functions |
    * +--------------------------------+
    */
    private int evaluateBoardstate(ref Board gameBoard, string phase, Move m,
                                   short depth) {
      int score = 0;
      short lastMoveMadeMill = 0;
      short lastMoveBlockedMill = 0;
      short differenceInBlockedMills = 0;
      short differenceInMills = 0;
      short differenceInBlockedPieces = 0;
      short differenceInTotalPieces = 0;
      short differenceIn2Pieces = 0;
      short differenceIn3Pieces = 0;
      short differenceInDoubleMills = 0;
      short winner = 0;
      short differenceInOpenMills = 0;
      short prioritySlotBonus = 0;

      // Get the values we need for our heuristic
      checkIfMadeMill(ref gameBoard, ref m, ref lastMoveMadeMill);
      checkIfBlockedMill(ref gameBoard, ref m, ref lastMoveMadeMill);
      calculateMillsAnd2Pieces(ref gameBoard, ref differenceInMills,
                              ref differenceIn2Pieces,
                              ref differenceInBlockedMills, ref depth);
      calculateBlockedPieces(ref gameBoard, ref differenceInBlockedPieces,
                             ref depth);
      calculate3Pieces(ref gameBoard, ref differenceIn3Pieces, ref depth);

      differenceInTotalPieces = (short)(gameBoard.getNumPieces(Tags.AI_TAG) -
                                 gameBoard.getNumPieces(Tags.HUMAN_TAG));
      calculateDoubleMills(ref gameBoard, ref differenceInDoubleMills);
      calculateWinner(ref gameBoard, ref winner, ref depth);
      calculatePrioritySlotBonus(ref gameBoard, ref prioritySlotBonus);

      // Evaluate the heuristic
      if (difficultyLevel == Difficulties.MEDIUM)
        score = evaluateMedium(phase, lastMoveMadeMill, lastMoveBlockedMill,
                               differenceInBlockedMills, differenceInMills,
                               differenceInBlockedPieces,
                               differenceInTotalPieces, differenceIn2Pieces,
                               differenceIn3Pieces, differenceInDoubleMills,
                               winner, differenceInOpenMills);

      else if (difficultyLevel == Difficulties.HARD)
        score = evaluateHard(phase, lastMoveMadeMill, lastMoveBlockedMill,
                             differenceInBlockedMills, differenceInMills,
                             differenceInBlockedPieces,
                             differenceInTotalPieces, differenceIn2Pieces,
                             differenceIn3Pieces, differenceInDoubleMills,
                             winner, differenceInOpenMills, prioritySlotBonus);

      return (score);
    }
    private int evaluateMedium(string phase,
        short lastMoveMadeMill, short lastMoveBlockedMill,
        short differenceInBlockedMills, short differenceInMills,
        short differenceInBlockedPieces, short differenceInTotalPieces,
        short differenceIn2Pieces, short differenceIn3Pieces,
        short differenceInDoubleMills, short winner,
        short differenceInOpenMills) {
      int score = 0;

            if (phase == Phases.PLACEMENT)
            {
                score += 18 * lastMoveMadeMill;
                score += 20 * lastMoveBlockedMill;
                score += 26 * differenceInMills;
                score += 1 * differenceInBlockedPieces;
                score += 9 * differenceInTotalPieces;
                score += 10 * differenceIn2Pieces;
                score += 7 * differenceIn3Pieces;
            }
            else if (phase == Phases.MOVEMENT)
            {
                score += 14 * lastMoveMadeMill;
                score += 20 * lastMoveBlockedMill;
                score += 35 * differenceInOpenMills;
                score += 35 * differenceInMills;
                score += 10 * differenceInBlockedPieces;
                score += 11 * differenceInTotalPieces;
                score += 8 * differenceInDoubleMills;
            }
            else
            {
                score += 30 * lastMoveMadeMill;
                score += 30 * lastMoveBlockedMill;
                score += 10 * differenceInMills;
                score += 10 * differenceIn2Pieces;
                score += 1 * differenceIn3Pieces;
                score += 1190 * winner;
            }

            return (score);
    }
    private int evaluateHard(string phase,
        short lastMoveMadeMill, short lastMoveBlockedMill,
        short differenceInBlockedMills, short differenceInMills,
        short differenceInBlockedPieces, short differenceInTotalPieces,
        short differenceIn2Pieces, short differenceIn3Pieces,
        short differenceInDoubleMills, short winner,
        short differenceInOpenMills, short prioritySlotBonus) {
      int score = 0;

      if (phase == Phases.PLACEMENT) {
        score += 18 * lastMoveMadeMill;
        score += 20 * lastMoveBlockedMill;
        score += 26 * differenceInMills;
        score += 1 * differenceInBlockedPieces;
        score += 9 * differenceInTotalPieces;
        score += 10 * differenceIn2Pieces;
        score += 7 * differenceIn3Pieces;
      }
      else if (phase == Phases.MOVEMENT) {
        score += 14 * lastMoveMadeMill;
        score += 20 * lastMoveBlockedMill;
        score += 35 * differenceInOpenMills;
        score += 35 * differenceInMills;
        score += 10 * differenceInBlockedPieces;
        score += 11 * differenceInTotalPieces;
        score += 8 * differenceInDoubleMills;
      }
      else {
        score += 30 * lastMoveMadeMill;
        score += 30 * lastMoveBlockedMill;
        score += 10 * differenceInMills;
        score += 10 * differenceIn2Pieces;
        score += 1 * differenceIn3Pieces;
        score += 1190 * winner;
      }

      // Bonus points for highly mobile spots
      if (phase != Phases.FLYING)
      score += prioritySlotBonus;

      return (score);
    }
    /* Calculates the difference in AI mills vs human mills and the
      * difference between the number of 2-piece configurations.
      * Positive numbers favor the AI, negatives favor the human
      */
    private void calculateMillsAnd2Pieces(ref Board gameBoard,
                                          ref short differenceInMills,
                                          ref short differenceIn2Pieces,
                                          ref short differenceInBlockedMills,
                                          ref short depth) {
      foreach (short[] mill in Configurations.MILLS) {
        short AIPieces = 0;
        short humanPieces = 0;
        short emptySlots = 0;

        foreach (short slot in mill)
          if (gameBoard.board[slot] == Tags.AI_TAG)
            AIPieces += 1;
          else if (gameBoard.board[slot] == Tags.HUMAN_TAG)
            humanPieces += 1;
          else
            emptySlots += 1;

        // Count the mills
        if (AIPieces == 3)
          differenceInMills += 1;
        else if (humanPieces == 3)
          differenceInMills -= 1;
        // Count the 2-piece configurations
        if (AIPieces == 2) {
          if (emptySlots == 1)
            differenceIn2Pieces += 1;
          else if (humanPieces == 1)
            differenceInBlockedMills += 1;
        }
        else if (humanPieces == 2)
          if (emptySlots == 1)
            differenceIn2Pieces -= 1;
          else if (AIPieces == 1)
            differenceInBlockedMills += 1;
      }
    }
    /* Calculates the difference in AI 3-piece configurations vs human
     * 3-piece configurations. Positive numbers the favor AI, negatives
     * favor the human
     */
    private void calculate3Pieces(ref Board gameBoard,
                                  ref short differenceIn3Pieces,
                                  ref short depth) {
      foreach (short[] threePiece in Configurations.THREE_PIECES) {
        short AIPieces = 0;
        short humanPieces = 0;

        foreach (short slot in threePiece)
          if (gameBoard.board[slot] == Tags.AI_TAG)
            AIPieces += 1;
          else if (gameBoard.board[slot] == Tags.HUMAN_TAG)
            humanPieces += 1;

        if (AIPieces == 3)
          differenceIn3Pieces += 1;
        else if (humanPieces == 3)
          differenceIn3Pieces -= 1;
      }
    }
    /* Calculates the difference in blocked human pieces vs blockd AI
     * pieces. Positive numbers favor the human, negatives the AI
     */
    private void calculateBlockedPieces(ref Board gameBoard,
                                        ref short differenceInBlockedPieces,
                                        ref short depth) {
      for (int i = 0; i < gameBoard.BOARD_SIZE; i++)
        if (gameBoard.board[i] != Tags.EMPTY) {
          short availableMoves = 0;
          foreach (short slot in Configurations.ADJACENT_SLOTS[i])
            if (gameBoard.board[slot] == Tags.EMPTY) {
              availableMoves += 1;
              break;
            }
          if (availableMoves == 0)
            if (gameBoard.board[i] == Tags.AI_TAG)
              differenceInBlockedPieces -= 1;
            else
              differenceInBlockedPieces += 1;
        }
    }
    /* Determine if the game is over and who the winner is. 1 if the AI
     * won, -1 if the human won, 0 if the game isn't over.
     */
    private void calculateWinner(ref Board gameBoard, ref short winner,
                                 ref short depth) {
      if (gameBoard.getPhase(Tags.AI_TAG) != Phases.PLACEMENT &&
          gameBoard.getPhase(Tags.HUMAN_TAG) != Phases.PLACEMENT)
        if (gameBoard.getNumPieces(Tags.HUMAN_TAG) < GameSettings.LOSE_AT)
          winner = 1;
        else if (gameBoard.getNumPieces(Tags.AI_TAG) == GameSettings.LOSE_AT)
          winner = -1;
        else if (generateMoves(ref gameBoard, Tags.HUMAN_TAG, depth).Count == 0)
          winner = 1;
        else if (generateMoves(ref gameBoard, Tags.AI_TAG, depth).Count == 0)
          winner = -1;
        else
          winner = 0;
      else
        winner = 0;
    }
    /* Calculate the difference in AI double mills vs human double
     * mills. Positive numbers favor the AI, negatives the human.
     */
    private void calculateDoubleMills(ref Board gameBoard,
                                      ref short differenceInDoubleMills) {
      foreach (short[] dm in Configurations.DOUBLE_MILLS) {
        short AIPieces = 0;
        short humanPieces = 0;
        for (int i = 0; i < 5; i++)
          if (gameBoard.board[dm[i]] == Tags.AI_TAG)
            AIPieces += 1;
          else if (gameBoard.board[dm[i]] == Tags.HUMAN_TAG)
            humanPieces += 1;
        if (AIPieces == 5 && gameBoard.board[dm[5]] == Tags.EMPTY)
          differenceInDoubleMills += 1;
        if (humanPieces == 5 && gameBoard.board[dm[5]] == Tags.EMPTY)
          differenceInDoubleMills -= 1;
      }
    }
    /* Checks if the last move made a mill.  1 if the AI made a mill. 
     * -1 if the human made a mill. 0 if no mills were made.
     */
    private void checkIfMadeMill(ref Board gameBoard, ref Move m,
                                 ref short madeMill) {
      if (m.removeFrom != -1)
        if (gameBoard.board[m.moveTo] == Tags.AI_TAG)
          madeMill = 1;
        else
          madeMill = -1;
      else
        madeMill = 0;
    }
    /* Calculate the difference in "open" mills (2/3 slots filled, an
     * adjacent piece, and no adjacent opponent piece).  Positive
     * numbers favor the AI, negatives favor the human.
     */
    private void calculateDifferenceInOpenMills(ref Board gameBoard,
                                                ref short differenceInOpenMills) {
      foreach (short[] mill in Configurations.MILLS) {
        int AIPieces = 0;
        int humanPieces = 0;
        int emptySlots = 0; // Total number
        int emptySlot = 0;  // The specific slot

        foreach (short slot in mill)
          if (gameBoard.board[slot] == Tags.AI_TAG)
            AIPieces += 1;
          else if (gameBoard.board[slot] == Tags.HUMAN_TAG)
            humanPieces += 1;
          else {
            emptySlots += 1;
            emptySlot = slot;
          }

        if (emptySlots == 1 && (AIPieces == 2 || humanPieces == 2)) {
          int adjAIPieces = 0;
          int adjHumanPieces = 0;
          foreach (short adj in Configurations.ADJACENT_SLOTS[emptySlots])
            if (gameBoard.board[adj] == Tags.AI_TAG)
              adjAIPieces += 1;
            else if (gameBoard.board[adj] == Tags.HUMAN_TAG)
              adjHumanPieces += 1;

          if (AIPieces == 2 && adjHumanPieces == 0 && adjAIPieces >= 1)
            differenceInOpenMills += 1;
          else if (humanPieces == 2 && adjAIPieces == 0 && adjHumanPieces >= 1)
            differenceInOpenMills -= 1;
        }
      }
    }

    /* Checks if the last move blocked a mill.  1 if the AI blocked a
     * mill.  -1 if the human blocked a mill. 0 if no mills were blocked.
     */
    private void checkIfBlockedMill(ref Board gameBoard, ref Move m, ref short blockedMill) {
      bool didBlockMill = false;
      string currentPlayer = gameBoard.board[m.moveTo];
      string opponent;

      if (currentPlayer == Tags.AI_TAG)
        opponent = Tags.HUMAN_TAG;
      else
        opponent = Tags.AI_TAG;

      foreach (short[] mill in Configurations.MILLS)
        if (mill.Contains(m.moveTo)) {
          short currentPlayerPieces = 0;
          short opponentPieces = 0;

          foreach (short slot in mill)
            if (gameBoard.board[slot] == currentPlayer)
              currentPlayerPieces += 1;
            else if (gameBoard.board[slot] == opponent)
              opponentPieces += 1;

          if (currentPlayerPieces == 1 && opponentPieces == 2) {
            didBlockMill = true;
            break;
          }
        }
      if (didBlockMill)
        if (currentPlayer == Tags.AI_TAG)
          blockedMill = 1;
        else
          blockedMill = -1;
      else
        blockedMill = 0;
    }

    private void calculatePrioritySlotBonus(ref Board gameBoard,
                                            ref short prioritySlotBonus) {

      for (short i = 0; i < gameBoard.BOARD_SIZE; i++) {
        if (Configurations.PLUS2.Contains(i)) {
          if (gameBoard.board[i] == Tags.AI_TAG)
            prioritySlotBonus += 2;
          else if (gameBoard.board[i] == Tags.HUMAN_TAG)
            prioritySlotBonus -= 2;
        }
        else if (Configurations.PLUS1.Contains(i))
          if (gameBoard.board[i] == Tags.AI_TAG)
            prioritySlotBonus += 1;
          else if (gameBoard.board[i] == Tags.HUMAN_TAG)
            prioritySlotBonus -= 1;
      }
    }

    /* +------------------+
     * | Helper Functions |
     * +------------------+
     */
    // Check if a new mill has been made with the
    private bool isInMill(ref Board gameBoard, short slot) {
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
    private bool canBeRemoved(ref Board gameBoard, short orb) {
      if (!isInMill(ref gameBoard, orb) ||
        allInMill(ref gameBoard, gameBoard.board[orb]))
        return (true);
      else
        return (false);
    }
    // Checks if all orbs of a given player are in a mill
    private bool allInMill(ref Board gameBoard, string playerTag) {
      for (short i = 0; i < gameBoard.BOARD_SIZE; i++)
        if (gameBoard.board[i] == playerTag)
          if (!isInMill(ref gameBoard, i))
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

    // Return a list of all the slots containing a given tag
    private List<short> getSlots(ref Board gameBoard, string tag) {
      List<short> slots = new List<short> { };

      for (short slot = 0; slot < gameBoard.BOARD_SIZE; slot++)
        if (gameBoard.board[slot] == tag)
          slots.Add(slot);

      return (slots);
    }
  }
}
