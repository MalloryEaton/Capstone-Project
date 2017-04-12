using System;
using System.Collections.Generic;

using AI;

// Represents a move
public class Move
{
  public short moveFrom = -1;
  public short moveTo = -1;
  public short removeFrom = -1;
  public int score = 0;

  public List<short> list() {
    return (new List<short> { moveFrom, moveTo, removeFrom });
  }
}

// Represent the game board
public class Board
{
  public short BOARD_SIZE = 24;

  public string[] board;
  private short numAIPieces;
  private short numHumanPieces;
  private short placedAIPieces;
  private short placedHumanPieces;

  public Board(short placedAI, short placedHuman) {
    board = new string[24];
    for (short i = 0; i < BOARD_SIZE; i++)
      board[i] = Tags.EMPTY;

    numAIPieces = 0;
    numHumanPieces = 0;
    placedAIPieces = placedAI;
    placedHumanPieces = placedHuman;
  }
  // Place a piece on the board
  public short placePiece(int slot, string player) {
    board[slot] = player;

    if (player == Tags.AI_TAG)
      incPlacedPieces(player);
    else
      incPlacedPieces(player);

    return (incPieces(player));
  }
  // Move a piece from one slot to another
  public void movePiece(int moveFrom, int moveTo) {
    string player = board[moveFrom];
    board[moveFrom] = Tags.EMPTY;
    board[moveTo] = player;
  }
  // Remove a piece from a given slot
  public short removePiece(int slot) {
    short pieces = decPieces(board[slot]);
    board[slot] = Tags.EMPTY;

    return (pieces);
  }
  // Return the number of pieces for the given players
  public short getNumPieces(string player) {
    short num = -1;
    if (player == Tags.AI_TAG)
      num = numAIPieces;
    else if (player == Tags.HUMAN_TAG)
      num = numHumanPieces;

    return (num);
  }
  // Return the number of placed pieces for the given players
  public short getPlacedPieces(string player) {
    short num = -1;
    if (player == Tags.AI_TAG)
      num = placedAIPieces;
    else if (player == Tags.HUMAN_TAG)
      num = placedHumanPieces;

    return (num);
  }
  // Return the current phase
  public string getPhase(string player) {
    string result;

    if (player == Tags.AI_TAG)
      if (placedAIPieces < GameSettings.NUMBER_OF_ORBS)
        result = Phases.PLACEMENT;
      else if (numAIPieces == GameSettings.FLYING_AT)
        result = Phases.FLYING;
      else
        result = Phases.MOVEMENT;
    else {
      if (placedHumanPieces < GameSettings.NUMBER_OF_ORBS)
        result = Phases.PLACEMENT;
      else if (numHumanPieces == GameSettings.FLYING_AT)
        result = Phases.FLYING;
      else
        result = Phases.MOVEMENT;
    }

    return (result);
  }
  /* Increment the number of placed for the given player
   * Returns the total number of placed pieces for said player
   */
  public short incPlacedPieces(string player) {
    short result = -1;
    if (player == Tags.AI_TAG) {
      placedAIPieces += 1;
      result = placedAIPieces;
    }
    else if (player == Tags.HUMAN_TAG) {
      placedHumanPieces += 1;
      result = placedHumanPieces;
    }
    return (result);
  }
  // Same as above, except it decrements the values
  public short decPlacedPieces(string player) {
    short result = -1;
    if (player == Tags.AI_TAG) {
      placedAIPieces -= 1;
      result = placedAIPieces;
    }
    else if (player == Tags.HUMAN_TAG) {
      placedHumanPieces -= 1;
      result = placedHumanPieces;
    }
    return (result);
  }
  /* Increment the number of pieces for the given player
   * Returns the total number of pieces for said player
   */
  public short incPieces(string player) {
    short result = -1;
    if (player == Tags.AI_TAG) {
      numAIPieces += 1;
      result = numAIPieces;
    }
    else if (player == Tags.HUMAN_TAG) {
      numHumanPieces += 1;
      result = numHumanPieces;
    }
    return (result);
  }
  // Same as above, except it decrements the values
  public short decPieces(string player) {
    short result = -1;
    if (player == Tags.AI_TAG) {
      numAIPieces -= 1;
      result = numAIPieces;
    }
    else if (player == Tags.HUMAN_TAG) {
      numHumanPieces -= 1;
      result = numHumanPieces;
    }
    return (result);
  }
}