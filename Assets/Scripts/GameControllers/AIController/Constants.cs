using System;
using System.Collections.Generic;

namespace AI
{
  public static class GameSettings
  {
    public const short NUMBER_OF_ORBS = 9;
    public const short FLYING_AT = 3;
    public const short LOSE_AT = 2;
  }

  public static class Tags
  {
    public const string EMPTY = "Empty";
    public const string AI_TAG = "Opponent";
    public const string HUMAN_TAG = "Player";
  }
  public static class Phases
  {
    public const string PLACEMENT = "placement";
    public const string MOVEMENT = "movement";
    public const string FLYING = "flying";
  }
  public static class Difficulties { 
    public const string EASY = "Easy";
    public const string MEDIUM = "Medium";
    public const string HARD = "Hard";
  }
  public static class Configurations {
    public static short[][] ADJACENT_SLOTS = new short[24][]{
      new short[2] { 1, 9 }, new short[3] { 0, 2, 4 }, new short[2] { 1, 14 },
      new short[2] { 4, 10 }, new short[4] { 1, 3, 5, 7 },
      new short[2] { 4, 13 }, new short[2] { 7, 11 }, new short[3] { 4, 6, 8 },
      new short[2] { 7, 12 }, new short[3] { 0, 10, 21 },
      new short[4] { 3, 9, 11, 18 }, new short[3] { 6, 10, 15 },
      new short[3] { 8, 13, 17 }, new short[4] { 5, 12, 14, 20 },
      new short[3] { 2, 13, 23 }, new short[2] { 11, 16 },
      new short[3] { 15, 17, 19 }, new short[2] { 12, 16 },
      new short[2] { 10, 19 }, new short[4] { 16, 18, 20, 22 },
      new short[2] { 13, 19 }, new short[2] { 9, 22 },
      new short[3] { 19, 21, 23 }, new short[2] { 14, 22 } };

    // The first 5 slots should be occupied, the 6th should be empty
    public static short[][] DOUBLE_MILLS = new short[][] {
      new short[6] { 0, 1, 2, 3, 5, 4}, new short[6] { 0, 2, 3, 4, 5, 1 },
      new short[6] { 3, 4, 5, 6, 8, 7 }, new short[6] { 3, 5, 8, 7, 8, 4 },
      new short[6] { 15, 16, 17, 18, 20, 19 },
      new short[6] { 15, 17, 18, 19, 20, 16 },
      new short[6] { 18, 19, 20, 21, 23, 22 },
      new short[6] { 18, 20, 21, 22, 23, 19 },
      new short[6] { 0, 9, 21, 3, 18, 10 },
      new short[6] { 0, 21, 3, 10, 18, 9 },
      new short[6] { 3, 10, 18, 6, 15, 11 },
      new short[6] { 3, 18, 6, 11, 15, 10 },
      new short[6] { 8, 12, 17, 5, 20, 17 },
      new short[6] { 8, 17, 5, 13, 20, 12 },
      new short[6] { 5, 13, 20, 2, 23, 14 },
      new short[6] { 5, 20, 2, 14, 23, 13 },
      new short[6] { 1, 4, 7, 11, 15, 6 }, new short[6] { 1, 4, 6, 11, 16, 7 },
      new short[6] { 1, 4, 7, 12, 17, 8 }, new short[6] { 1, 7, 8, 12, 17, 4 },
      new short[6] { 1, 4, 7, 10, 18, 3 }, new short[6] { 1, 7, 3, 10, 18, 4 },
      new short[6] { 1, 4, 7, 12, 20, 13 },
      new short[6] { 1, 7, 5, 13, 20, 12 },
      new short[6] { 1, 4, 7, 9, 21, 0 }, new short[6] { 4, 7, 0, 9, 21, 1 },
      new short[6] { 1, 4, 7, 14, 23, 2 }, new short[6] { 4, 7, 2, 14, 23, 1 },
      new short[6] { 12, 13, 14, 6, 7, 8 },
      new short[6] { 13, 14, 6, 7, 8, 12 },
      new short[6] { 12, 13, 14, 15, 16, 17 },
      new short[6] { 13, 14, 15, 16, 17, 12 },
      new short[6] { 12, 13, 14, 3, 4, 5 },
      new short[6] { 12, 14, 3, 4, 5, 13 },
      new short[6] { 12, 13, 14, 18, 19, 20 },
      new short[6] { 12, 14, 18, 19, 20, 13 },
      new short[6] { 12, 13, 14, 0, 1, 2 },
      new short[6] { 12, 13, 0, 1, 2, 14 },
      new short[6] { 12, 13, 14, 21, 22, 23 },
      new short[6] { 12, 13, 21, 22, 23, 14 },
      new short[6] { 16, 19, 22, 6, 11, 15 },
      new short[6] { 19, 22, 6, 11, 15, 16 },
      new short[6] { 16, 19, 22, 8, 12, 17 },
      new short[6] { 19, 22, 8, 12, 17, 16 },
      new short[6] { 16, 19, 22, 3, 10, 18 },
      new short[6] { 16, 22, 3, 10, 18, 19 },
      new short[6] { 16, 19, 22, 0, 9, 21 },
      new short[6] { 16, 19, 0, 9, 21, 22 },
      new short[6] { 16, 19, 22, 2, 14, 23 },
      new short[6] { 16, 19, 2, 14, 23, 22 },
      new short[6] { 9, 10, 11, 7, 8, 6 },
      new short[6] { 9, 10, 6, 7, 8, 11 },
      new short[6] { 9, 10, 11, 16, 17, 15 },
      new short[6] { 9, 10, 15, 16, 17, 11 },
      new short[6] { 9, 10, 11, 4, 5, 3 },
      new short[6] { 9, 11, 3, 4, 5, 10 },
      new short[6] { 9, 10, 11, 19, 20, 18 },
      new short[6] { 9, 11, 18, 19, 20, 10 },
      new short[6] { 9, 10, 11, 1, 2, 0, },
      new short[6] { 10, 11, 0, 1, 2, 9 },
      new short[6] { 9, 10, 11, 22, 23, 21 },
      new short[6] { 10, 11, 21, 22, 23, 9 } };

    public static short[][] MILLS = new short[16][]{
      new short[3] { 0, 1, 2 }, new short[3] { 0, 9, 21 },
      new short[3] { 1, 4, 7 }, new short[3] { 2, 14, 23 },
      new short[3] { 3, 4, 5 }, new short[3] { 3, 10, 18 },
      new short[3] { 5, 13, 20 }, new short[3] { 6, 7, 8 },
      new short[3] { 6, 11, 15 }, new short[3] { 8, 12, 17 },
      new short[3] { 9, 10, 11 }, new short[3] { 12, 13, 14 },
      new short[3] { 15, 16, 17 }, new short[3] { 16, 19, 22 },
      new short[3] { 18, 19, 20 }, new short[3] { 21, 22, 23 } };

    public static List<short> PLUS2 = new List<short> { 4, 10, 13, 19 };
    public static  List<short> PLUS1 = new List<short> { 1, 7, 9, 11, 12, 14,
                                                         16, 22 };

    public static short[][] THREE_PIECES = new short[][] {
      new short[3] { 0, 1, 9 }, new short[3] { 1, 2, 14 },
      new short[3] { 3, 4, 10 }, new short[3] { 4, 5, 13 },
      new short[3] { 11, 15, 16 }, new short[3] { 12, 16, 17 },
      new short[3] { 10, 18, 19 }, new short[3] { 13, 19, 20 },
      new short[3] { 9, 21, 22 }, new short[3] { 14, 22, 23 },
      new short[3] { 0, 1, 4 }, new short[3] { 1, 2, 4 },
      new short[3] { 2, 3, 4 }, new short[3] { 2, 4, 5 },
      new short[3] { 0, 9, 10 }, new short[3] { 2, 13, 14 },
      new short[3] { 9, 10, 21 }, new short[3] { 13, 14, 23 },
      new short[3] { 19, 21, 22 }, new short[3] { 19, 22, 23 },
      new short[3] { 3, 4, 7 }, new short[3] { 4, 5, 7 },
      new short[3] { 3, 10, 11 }, new short[3] { 5, 12, 13 },
      new short[3] { 10, 11, 15 }, new short[3] { 4, 6, 7 },
      new short[3] { 4, 7, 8 }, new short[3] { 12, 13, 20 },
      new short[3] { 16, 18, 19 }, new short[3] { 16, 19, 20 },
      new short[3] { 6, 10, 11 }, new short[3] { 8, 12, 13 },
      new short[3] { 15, 16, 19 }, new short[3] { 16, 17, 19 } };
  }
}
