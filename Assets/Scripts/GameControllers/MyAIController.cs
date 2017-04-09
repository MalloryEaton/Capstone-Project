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
  // Gets a move from the AI
  public List<short> GetAIMove(string phase, string difficulty) {
    List<short> move;

    if (difficulty == Constants.EASY) {
      EasyAI ai = new EasyAI();
      move = ai.GetEasyAIMove(phase);
    }
    else if (difficulty == Constants.MEDIUM) {
      EasyAI ai = new EasyAI();
      move = ai.GetEasyAIMove(phase);
    }
    // Hard
    else { 
      EasyAI ai = new EasyAI();
      move = ai.GetEasyAIMove(phase);
    }

    return (move);
  }
}
