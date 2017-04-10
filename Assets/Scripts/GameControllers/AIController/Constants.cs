using System;

namespace AI
{
  public static class Tags
  {
    public const string EMPTY = "Empty";
    public const string AI_TAG = "Opponent";
    public const string PLAYER_TAG = "Player";
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
    public static short[][] MILLS = new short[16][]{
      new short[3]{ 0, 1, 2 }, new short[3]{ 0, 9, 21 },
      new short[3]{ 1, 4, 7 }, new short[3]{ 2, 14, 23 },
      new short[3]{ 3, 4, 5 }, new short[3]{ 3, 10, 18 },
      new short[3]{ 5, 13, 20 }, new short[3]{ 6, 7, 8 },
      new short[3]{ 6, 11, 15 }, new short[3]{ 8, 12, 17 },
      new short[3]{ 9, 10, 11 }, new short[3]{ 12, 13, 14 },
      new short[3]{ 15, 16, 17 }, new short[3]{ 16, 19, 22 },
      new short[3]{ 18, 19, 20 }, new short[3]{ 21, 22, 23 } };

    public static short[][] ADJACENT_SLOTS = new short[24][]{
      new short[2]{ 1, 9 }, new short[3]{ 0, 2, 4 }, new short[2]{ 1, 14 },
      new short[2]{ 4, 10 }, new short[4]{ 1, 3, 5, 7 },
      new short[2]{ 4, 13 }, new short[2]{ 7, 11 }, new short[3]{ 4, 6, 8 },
      new short[2]{ 7, 12 }, new short[3]{ 0, 10, 21 },
      new short[4]{ 3, 9, 11, 18 }, new short[3]{ 6, 10, 15 },
      new short[3]{ 8, 13, 17 }, new short[4]{ 5, 12, 14, 20 },
      new short[3]{ 2, 13, 23 }, new short[2]{ 11, 16 },
      new short[3]{ 15, 17, 19 }, new short[2]{ 12, 16 },
      new short[2]{ 10, 19 }, new short[4]{ 16, 18, 20, 22 },
      new short[2]{ 13, 19 }, new short[2]{ 9, 22 },
      new short[3]{ 19, 21, 23 }, new short[2]{ 14, 22 } };
  }
}
