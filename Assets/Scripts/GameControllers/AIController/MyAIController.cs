/* This file is part of Runic by Ensorcelled Studios
 * AI by Bryan Cuneo
 * 
 * Heurstics evaluations based on Petcu & Holban, 2008
 * http://www.dasconference.ro/papers/2008/B7.pdf
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// AI Functions
using AI;

public class MyAIController : MonoBehaviour
{
  private EasyAI easyAI;
  private MediumAI mediumAI;
  private GameLogicController gameLogicController;

  void Start() {
    easyAI = FindObjectOfType(typeof(EasyAI)) as EasyAI;
    mediumAI = FindObjectOfType(typeof(MediumAI)) as MediumAI;
    gameLogicController = FindObjectOfType(typeof(GameLogicController))
                            as GameLogicController;
  }

  // Gets a move from the AI
  public List<short> GetAIMove(string difficulty, short placedAIPieces,
                               short placedHumanPieces) {
    List<short> move;
    Board gameBoard = boardFromRuneList(placedAIPieces, placedHumanPieces);
    string phase = gameBoard.getPhase(Tags.AI_TAG);

    if (difficulty == Difficulties.EASY)
      move = easyAI.getEasyAIMove(phase);
    else if (difficulty == Difficulties.MEDIUM)
      move = mediumAI.getMediumAIMove(gameBoard);
    else // Hard
      move = mediumAI.getMediumAIMove(gameBoard);

    return (move);
  }

  private Board boardFromRuneList(short placedAIPieces, short placedHumanPieces) {
    Board gameBoard = new Board(placedAIPieces, placedHumanPieces);

    for (int i = 0; i < 24; i++) {
      gameBoard.board[i] = gameLogicController.runeList[i].tag;
    }

    return (gameBoard);
  }
}
