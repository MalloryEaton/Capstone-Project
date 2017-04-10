using System;

using AI;

// Represents a move
public class Move
{
  public short moveFrom = -1;
  public short moveTo = -1;
  public short removeFrom = -1;
}

// Represent the game board
public class Board
{
  public short BOARD_SIZE = 24;

  public string[] board;
  private short numAIPieces;
  private short numPlayerPieces;

  public Board() {
    board = new string[24];
    for (short i = 0; i < BOARD_SIZE; i++)
      board[i] = Tags.EMPTY;

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
    else if (player == Tags.PLAYER_TAG)
      num = numPlayerPieces;

    return (num);
  }
  /* Increment the number of pieces for the given player
    * Returns the total number of pieces for said player
    */
  private short incPieces(string player) {
    short result = -1;
    if (player == Tags.AI_TAG) {
      numAIPieces += 1;
      result = numAIPieces;
    }
    else if (player == Tags.PLAYER_TAG) {
      numPlayerPieces += 1;
      result = numPlayerPieces;
    }
    return (result);
  }
  // Same as above, except it decrementns the values
  private short decPieces(string player) {
    short result = -1;
    if (player == Tags.AI_TAG) {
      numAIPieces -= 1;
      result = numAIPieces;
    }
    else if (player == Tags.PLAYER_TAG) {
      numPlayerPieces -= 1;
      result = numPlayerPieces;
    }
    return (result);
  }
}