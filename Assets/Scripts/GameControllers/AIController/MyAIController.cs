/* This file is part of Runic by Ensorcelled Studios
 * AI by Bryan Cuneo
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
    gameLogicController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
  }

  // Gets a move from the AI
  public List<short> GetAIMove(string phase, string difficulty) {
    List<short> move;

    if (difficulty == Difficulties.EASY) {
      //EasyAI ai = new EasyAI();
      move = easyAI.GetEasyAIMove(phase);
    }
    else if (difficulty == Difficulties.MEDIUM) {
      move = mediumAI.GetMediumAIMove(boardFromRuneList(), Phases.PLACEMENT);
    }
    // Hard
    else {
      move = easyAI.GetEasyAIMove(phase);
    }

    return (move);
  }

  private Board boardFromRuneList() {
    Board gameBoard = new Board();

    for (int i = 0; i < gameBoard.BOARD_SIZE; i++)
      gameBoard.board[i] = gameLogicController.runeList[i].tag;

    return (gameBoard);
  }
}
