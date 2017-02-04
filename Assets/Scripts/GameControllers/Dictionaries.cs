using System;
using System.Collections.Generic;
using UnityEngine;

public class Dictionaries : MonoBehaviour {

    public Dictionary<short, short[]> adjacencyDictionary; 
    public Dictionary<short, Vector3> orbPositionsDictionary;
    public Dictionary<string, GameObject> orbsDictionary;
    public Dictionary<short, Material> runeHighlightsDictionary;
    public Dictionary<short, Material> runeOriginalsDictionary;
    public Dictionary<string, GameObject> shrinesDictionary;

    public List<Mill> verticalMillsList;

    // Use this for initialization
    void Start ()
    {
        adjacencyDictionary = new Dictionary<short, short[]>();
        orbPositionsDictionary = new Dictionary<short, Vector3>();
        runeHighlightsDictionary = new Dictionary<short, Material>();
        runeOriginalsDictionary = new Dictionary<short, Material>();
        orbsDictionary = new Dictionary<string, GameObject>();
        shrinesDictionary = new Dictionary<string, GameObject>();
        verticalMillsList = new List<Mill>();

        InitializeAdjacencyDictionary();
        InitializeRuneHighlightsDictionary();
        InitializeRuneOriginalsDictionary();
        InitializeLocationDictionary();
        InitializeOrbDictionary();
        InitializeShrineDictionary();
        InitializeVerticalMillsList();
    }

    private void InitializeVerticalMillsList()
    {
        verticalMillsList.Add(new Mill(0, 9, 21));
        verticalMillsList.Add(new Mill(1, 4, 7));
        verticalMillsList.Add(new Mill(2, 14, 23));
        verticalMillsList.Add(new Mill(3, 10, 18));
        verticalMillsList.Add(new Mill(5, 13, 20));
        verticalMillsList.Add(new Mill(6, 11, 15));
        verticalMillsList.Add(new Mill(8, 12, 17));
        verticalMillsList.Add(new Mill(16, 19, 22));
    }

    private void InitializeAdjacencyDictionary()
    {
        adjacencyDictionary.Add(0, new short[] { 1, 9 });
        adjacencyDictionary.Add(1, new short[] { 0, 4, 2 });
        adjacencyDictionary.Add(2, new short[] { 1, 14 });

        adjacencyDictionary.Add(3, new short[] { 4, 10 });
        adjacencyDictionary.Add(4, new short[] { 1, 3, 7, 5 });
        adjacencyDictionary.Add(5, new short[] { 4, 13 });

        adjacencyDictionary.Add(6, new short[] { 7, 11 });
        adjacencyDictionary.Add(7, new short[] { 4, 6, 8 });
        adjacencyDictionary.Add(8, new short[] { 7, 12 });

        adjacencyDictionary.Add(9, new short[] { 0, 21, 10 });
        adjacencyDictionary.Add(10, new short[] { 3, 9, 18, 11 });
        adjacencyDictionary.Add(11, new short[] { 6, 10, 15 });

        adjacencyDictionary.Add(12, new short[] { 8, 17, 13 });
        adjacencyDictionary.Add(13, new short[] { 5, 12, 20, 14 });
        adjacencyDictionary.Add(14, new short[] { 2, 13, 23 });

        adjacencyDictionary.Add(15, new short[] { 11, 16 });
        adjacencyDictionary.Add(16, new short[] { 15, 19, 17 });
        adjacencyDictionary.Add(17, new short[] { 12, 16 });

        adjacencyDictionary.Add(18, new short[] { 10, 19 });
        adjacencyDictionary.Add(19, new short[] { 16, 18, 22, 20 });
        adjacencyDictionary.Add(20, new short[] { 13, 19 });

        adjacencyDictionary.Add(21, new short[] { 9, 22 });
        adjacencyDictionary.Add(22, new short[] { 19, 21, 23 });
        adjacencyDictionary.Add(23, new short[] { 14, 22 });
    }

    private void InitializeOrbDictionary()
    {
        orbsDictionary.Add("black", Resources.Load(@"Orbs\BlackOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("blue", Resources.Load(@"Orbs\BlueOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("green", Resources.Load(@"Orbs\GreenOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("orange", Resources.Load(@"Orbs\OrangeOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("purple", Resources.Load(@"Orbs\PurpleOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("red", Resources.Load(@"Orbs\RedOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("white", Resources.Load(@"Orbs\WhiteOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("yellow", Resources.Load(@"Orbs\YellowOrb", typeof(GameObject)) as GameObject);
    }

    #region Orb Locations
    private void InitializeLocationDictionary()
    {
        orbPositionsDictionary.Add(0, new Vector3(24f, 1.5f, 0f));
        orbPositionsDictionary.Add(1, new Vector3(12f, 1.5f, 0f));
        orbPositionsDictionary.Add(2, new Vector3(0f, 1.5f, 0f));

        orbPositionsDictionary.Add(3, new Vector3(20f, 1.5f, 4f));
        orbPositionsDictionary.Add(4, new Vector3(12f, 1.5f, 4f));
        orbPositionsDictionary.Add(5, new Vector3(4f, 1.5f, 4f));

        orbPositionsDictionary.Add(6, new Vector3(16f, 1.5f, 8f));
        orbPositionsDictionary.Add(7, new Vector3(12f, 1.5f, 8f));
        orbPositionsDictionary.Add(8, new Vector3(8f, 1.5f, 8f));

        orbPositionsDictionary.Add(9, new Vector3(24f, 1.5f, 12f));
        orbPositionsDictionary.Add(10, new Vector3(20f, 1.5f, 12f));
        orbPositionsDictionary.Add(11, new Vector3(16f, 1.5f, 12f));
        orbPositionsDictionary.Add(12, new Vector3(8f, 1.5f, 12f));
        orbPositionsDictionary.Add(13, new Vector3(4f, 1.5f, 12f));
        orbPositionsDictionary.Add(14, new Vector3(0f, 1.5f, 12f));

        orbPositionsDictionary.Add(15, new Vector3(16f, 1.5f, 16f));
        orbPositionsDictionary.Add(16, new Vector3(12f, 1.5f, 16f));
        orbPositionsDictionary.Add(17, new Vector3(8f, 1.5f, 16f));

        orbPositionsDictionary.Add(18, new Vector3(20f, 1.5f, 20f));
        orbPositionsDictionary.Add(19, new Vector3(12f, 1.5f, 20f));
        orbPositionsDictionary.Add(20, new Vector3(4f, 1.5f, 20f));

        orbPositionsDictionary.Add(21, new Vector3(24f, 1.5f, 24f));
        orbPositionsDictionary.Add(22, new Vector3(12f, 1.5f, 24f));
        orbPositionsDictionary.Add(23, new Vector3(0f, 1.5f, 24f));
    }
    #endregion

    #region Rune Material Dictionaries
    void InitializeRuneHighlightsDictionary()
    {
        runeHighlightsDictionary.Add(0, Resources.Load(@"Runes\Stones1", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(1, Resources.Load(@"Runes\Stones2", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(2, Resources.Load(@"Runes\Stones3", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(3, Resources.Load(@"Runes\Stones4", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(4, Resources.Load(@"Runes\Stones5", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(5, Resources.Load(@"Runes\Stones6", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(6, Resources.Load(@"Runes\Stones7", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(7, Resources.Load(@"Runes\Stones8", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(8, Resources.Load(@"Runes\Stones9", typeof(Material)) as Material);
        runeHighlightsDictionary.Add(9, Resources.Load(@"Runes\Stones10", typeof(Material)) as Material);
    }

    void InitializeRuneOriginalsDictionary()
    {
        runeOriginalsDictionary.Add(0, Resources.Load(@"Runes\Stones1Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(1, Resources.Load(@"Runes\Stones2Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(2, Resources.Load(@"Runes\Stones3Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(3, Resources.Load(@"Runes\Stones4Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(4, Resources.Load(@"Runes\Stones5Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(5, Resources.Load(@"Runes\Stones6Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(6, Resources.Load(@"Runes\Stones7Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(7, Resources.Load(@"Runes\Stones8Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(8, Resources.Load(@"Runes\Stones9Dark", typeof(Material)) as Material);
        runeOriginalsDictionary.Add(9, Resources.Load(@"Runes\Stones10Dark", typeof(Material)) as Material);
    }
    #endregion

    private void InitializeShrineDictionary()
    {
        shrinesDictionary.Add("black", Resources.Load(@"Shrines\ShrineBlack", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("blue", Resources.Load(@"Shrines\ShrineBlue", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("green", Resources.Load(@"Shrines\ShrineGreen", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("orange", Resources.Load(@"Shrines\ShrineOrange", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("purple", Resources.Load(@"Shrines\ShrinePurple", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("red", Resources.Load(@"Shrines\ShrineRed", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("white", Resources.Load(@"Shrines\ShrineWhite", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("yellow", Resources.Load(@"Shrines\ShrineYellow", typeof(GameObject)) as GameObject);
    }
}
