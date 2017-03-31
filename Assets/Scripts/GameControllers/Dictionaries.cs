using System;
using System.Collections.Generic;
using UnityEngine;

public class Dictionaries : MonoBehaviour {

    public Dictionary<short, short[]> adjacencyDictionary; 
    public Dictionary<short, Vector3> orbPositionsDictionary;
    public Dictionary<short, Vector3> orbSide1Dictionary;
    public Dictionary<short, Vector3> orbSide2Dictionary;
    public Dictionary<string, GameObject> orbsDictionary;
    public Dictionary<short, Material> runeHighlightsDictionary;
    public Dictionary<short, Material> runeOriginalsDictionary;
    public Dictionary<string, GameObject> shrinesDictionary;
    public Dictionary<string, GameObject> shrinesTutorialDictionary;
    public Dictionary<string, GameObject> magicRingDictionary;
    public Dictionary<string, GameObject> magesDictionary;
    public Dictionary<string, GameObject> magicHitDictionary;
    public Dictionary<string, GameObject> orbContainersDictionary;

    public List<Mill> verticalMillsList;

    // Use this for initialization
    void Start ()
    {
        adjacencyDictionary = new Dictionary<short, short[]>();
        orbPositionsDictionary = new Dictionary<short, Vector3>();
        orbSide1Dictionary = new Dictionary<short, Vector3>();
        orbSide2Dictionary = new Dictionary<short, Vector3>();
        runeHighlightsDictionary = new Dictionary<short, Material>();
        runeOriginalsDictionary = new Dictionary<short, Material>();
        orbsDictionary = new Dictionary<string, GameObject>();
        shrinesTutorialDictionary = new Dictionary<string, GameObject>();
        shrinesDictionary = new Dictionary<string, GameObject>();
        magicRingDictionary = new Dictionary<string, GameObject>();
        magesDictionary = new Dictionary<string, GameObject>();
        magicHitDictionary = new Dictionary<string, GameObject>();
        orbContainersDictionary = new Dictionary<string, GameObject>();
        verticalMillsList = new List<Mill>();

        InitializeAdjacencyDictionary();
        InitializeRuneHighlightsDictionary();
        InitializeRuneOriginalsDictionary();
        InitializeLocationDictionary();
        InitializeStartingLocationsDictionary();
        InitializeOrbDictionary();
        InitializeShrineDictionary();
        InitializeShrineTutorialDictionary();
        InitializeVerticalMillsList();
        InitializeMagicRingDictionary();
        InitializeMagesDictionary();
        InitializeMagicHitDictionary();
        InitializeOrbContainersDictionary();
    }

    private void InitializeOrbContainersDictionary()
    {
        orbContainersDictionary.Add("Green", Resources.Load(@"Orbs\OrbContainers\GreenOrbContainer", typeof(GameObject)) as GameObject);
        orbContainersDictionary.Add("Purple", Resources.Load(@"Orbs\OrbContainers\PurpleOrbContainer", typeof(GameObject)) as GameObject);
        orbContainersDictionary.Add("GreenMovement", Resources.Load(@"Orbs\OrbContainers\GreenOrbsMovementPhase", typeof(GameObject)) as GameObject);
        orbContainersDictionary.Add("PurpleMovement", Resources.Load(@"Orbs\OrbContainers\PurpleOrbsMovementPhase", typeof(GameObject)) as GameObject);
        orbContainersDictionary.Add("GreenFly", Resources.Load(@"Orbs\OrbContainers\GreenOrbsFlyPhase", typeof(GameObject)) as GameObject);
        orbContainersDictionary.Add("PurpleFly", Resources.Load(@"Orbs\OrbContainers\PurpleOrbsFlyPhase", typeof(GameObject)) as GameObject);
    }

    private void InitializeMagesDictionary()
    {
        magesDictionary.Add("Black", Resources.Load(@"MagesForBoard\BlackMage", typeof(GameObject)) as GameObject);
        magesDictionary.Add("Blue", Resources.Load(@"MagesForBoard\BlueMage", typeof(GameObject)) as GameObject);
        magesDictionary.Add("Green", Resources.Load(@"MagesForBoard\GreenMage", typeof(GameObject)) as GameObject);
        magesDictionary.Add("Orange", Resources.Load(@"MagesForBoard\OrangeMage", typeof(GameObject)) as GameObject);
        magesDictionary.Add("Purple", Resources.Load(@"MagesForBoard\PurpleMage", typeof(GameObject)) as GameObject);
        magesDictionary.Add("Red", Resources.Load(@"MagesForBoard\RedMage", typeof(GameObject)) as GameObject);
        magesDictionary.Add("White", Resources.Load(@"MagesForBoard\WhiteMage", typeof(GameObject)) as GameObject);
        magesDictionary.Add("Yellow", Resources.Load(@"MagesForBoard\YellowMage", typeof(GameObject)) as GameObject);
    }

    private void InitializeMagicHitDictionary()
    {
        magicHitDictionary.Add("Black", Resources.Load(@"Hits\BlackHit", typeof(GameObject)) as GameObject);
        magicHitDictionary.Add("Blue", Resources.Load(@"Hits\BlueHit", typeof(GameObject)) as GameObject);
        magicHitDictionary.Add("Green", Resources.Load(@"Hits\GreenHit", typeof(GameObject)) as GameObject);
        magicHitDictionary.Add("Orange", Resources.Load(@"Hits\OrangeHit", typeof(GameObject)) as GameObject);
        magicHitDictionary.Add("Purple", Resources.Load(@"Hits\PurpleHit", typeof(GameObject)) as GameObject);
        magicHitDictionary.Add("Red", Resources.Load(@"Hits\RedHit", typeof(GameObject)) as GameObject);
        magicHitDictionary.Add("White", Resources.Load(@"Hits\WhiteHit", typeof(GameObject)) as GameObject);
        magicHitDictionary.Add("Yellow", Resources.Load(@"Hits\YellowHit", typeof(GameObject)) as GameObject);
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
        orbsDictionary.Add("Black", Resources.Load(@"Orbs\BlackOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("Blue", Resources.Load(@"Orbs\BlueOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("Green", Resources.Load(@"Orbs\GreenOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("Orange", Resources.Load(@"Orbs\OrangeOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("Purple", Resources.Load(@"Orbs\PurpleOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("Red", Resources.Load(@"Orbs\RedOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("White", Resources.Load(@"Orbs\WhiteOrb", typeof(GameObject)) as GameObject);
        orbsDictionary.Add("Yellow", Resources.Load(@"Orbs\YellowOrb", typeof(GameObject)) as GameObject);
    }
    
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

    private void InitializeStartingLocationsDictionary()
    {
        orbSide1Dictionary.Add(1, new Vector3(16f, 2f, 28f));
        orbSide1Dictionary.Add(2, new Vector3(14f, 2f, 28f));
        orbSide1Dictionary.Add(3, new Vector3(12f, 2f, 28f));
        orbSide1Dictionary.Add(4, new Vector3(10f, 2f, 28f));
        orbSide1Dictionary.Add(5, new Vector3(8f, 2f, 28f));
        orbSide1Dictionary.Add(6, new Vector3(6f, 2f, 28f));
        orbSide1Dictionary.Add(7, new Vector3(4f, 2f, 28f));
        orbSide1Dictionary.Add(8, new Vector3(2f, 2f, 28f));
        orbSide1Dictionary.Add(9, new Vector3(0f, 2f, 28f));

        orbSide2Dictionary.Add(1, new Vector3(8f, 2f, -4f));
        orbSide2Dictionary.Add(2, new Vector3(10f, 2f, -4f));
        orbSide2Dictionary.Add(3, new Vector3(12f, 2f, -4f));
        orbSide2Dictionary.Add(4, new Vector3(14f, 2f, -4f));
        orbSide2Dictionary.Add(5, new Vector3(16f, 2f, -4f));
        orbSide2Dictionary.Add(6, new Vector3(18f, 2f, -4f));
        orbSide2Dictionary.Add(7, new Vector3(20f, 2f, -4f));
        orbSide2Dictionary.Add(8, new Vector3(22f, 2f, -4f));
        orbSide2Dictionary.Add(9, new Vector3(24f, 2f, -4f));
    }

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
        shrinesDictionary.Add("Black", Resources.Load(@"Shrines\ShrineBlack", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("Blue", Resources.Load(@"Shrines\ShrineBlue", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("Green", Resources.Load(@"Shrines\ShrineGreen", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("Orange", Resources.Load(@"Shrines\ShrineOrange", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("Purple", Resources.Load(@"Shrines\ShrinePurple", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("Red", Resources.Load(@"Shrines\ShrineRed", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("White", Resources.Load(@"Shrines\ShrineWhite", typeof(GameObject)) as GameObject);
        shrinesDictionary.Add("Yellow", Resources.Load(@"Shrines\ShrineYellow", typeof(GameObject)) as GameObject);
    }

    private void InitializeShrineTutorialDictionary()
    {
        shrinesTutorialDictionary.Add("Black", Resources.Load(@"ShrinesTutorial\ShrineBlack", typeof(GameObject)) as GameObject);
        shrinesTutorialDictionary.Add("Blue", Resources.Load(@"ShrinesTutorial\ShrineBlue", typeof(GameObject)) as GameObject);
        shrinesTutorialDictionary.Add("Green", Resources.Load(@"ShrinesTutorial\ShrineGreen", typeof(GameObject)) as GameObject);
        shrinesTutorialDictionary.Add("Orange", Resources.Load(@"ShrinesTutorial\ShrineOrange", typeof(GameObject)) as GameObject);
        shrinesTutorialDictionary.Add("Purple", Resources.Load(@"ShrinesTutorial\ShrinePurple", typeof(GameObject)) as GameObject);
        shrinesTutorialDictionary.Add("Red", Resources.Load(@"ShrinesTutorial\ShrineRed", typeof(GameObject)) as GameObject);
        shrinesTutorialDictionary.Add("White", Resources.Load(@"ShrinesTutorial\ShrineWhite", typeof(GameObject)) as GameObject);
        shrinesTutorialDictionary.Add("Yellow", Resources.Load(@"ShrinesTutorial\ShrineYellow", typeof(GameObject)) as GameObject);
    }

    private void InitializeMagicRingDictionary()
    {
        magicRingDictionary.Add("Black", Resources.Load(@"MagicRings\BlackRing", typeof(GameObject)) as GameObject);
        magicRingDictionary.Add("Blue", Resources.Load(@"MagicRings\BlueRing", typeof(GameObject)) as GameObject);
        magicRingDictionary.Add("Green", Resources.Load(@"MagicRings\GreenRing", typeof(GameObject)) as GameObject);
        magicRingDictionary.Add("Orange", Resources.Load(@"MagicRings\OrangeRing", typeof(GameObject)) as GameObject);
        magicRingDictionary.Add("Purple", Resources.Load(@"MagicRings\PurpleRing", typeof(GameObject)) as GameObject);
        magicRingDictionary.Add("Red", Resources.Load(@"MagicRings\RedRing", typeof(GameObject)) as GameObject);
        magicRingDictionary.Add("White", Resources.Load(@"MagicRings\WhiteRing", typeof(GameObject)) as GameObject);
        magicRingDictionary.Add("Yellow", Resources.Load(@"MagicRings\YellowRing", typeof(GameObject)) as GameObject);
    }
}
