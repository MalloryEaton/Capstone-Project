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

  void Start() {
    easyAI = FindObjectOfType(typeof(EasyAI)) as EasyAI;
    mediumAI = FindObjectOfType(typeof(MediumAI)) as MediumAI;
  }

  // Gets a move from the AI
  public List<short> GetAIMove(string phase, string difficulty) {
    List<short> move;

    if (difficulty == Constants.EASY) {
      //EasyAI ai = new EasyAI();
      move = easyAI.GetEasyAIMove(phase);
    }
    else if (difficulty == Constants.MEDIUM) {
      move = easyAI.GetEasyAIMove(phase);
    }
    // Hard
    else {
      move = easyAI.GetEasyAIMove(phase);
    }

    return (move);
  }
}
