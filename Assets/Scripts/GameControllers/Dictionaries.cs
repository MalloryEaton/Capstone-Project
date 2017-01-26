using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dictionaries : MonoBehaviour {

    public Dictionary<short, Vector3> orbPositions;
    public Dictionary<string, GameObject> orbs;
    public Dictionary<short, Material> highlightMaterials;
    public Dictionary<short, Material> originalMaterials;
    public Dictionary<string, GameObject> shrines;

    // Use this for initialization
    void Start ()
    {
        orbPositions = new Dictionary<short, Vector3>();
        highlightMaterials = new Dictionary<short, Material>();
        originalMaterials = new Dictionary<short, Material>();
        orbs = new Dictionary<string, GameObject>();
        shrines = new Dictionary<string, GameObject>();

        initializeMaterialHighlightDictionary();
        initializeMaterialOriginalDictionary();
        initializeLocationDictionary();
        initializeOrbDictionary();
        initializeShrineDictionary();
    }

    private void initializeOrbDictionary()
    {
        orbs.Add("black", Resources.Load(@"Orbs\BlackOrb", typeof(GameObject)) as GameObject);
        orbs.Add("blue", Resources.Load(@"Orbs\BlueOrb", typeof(GameObject)) as GameObject);
        orbs.Add("green", Resources.Load(@"Orbs\GreenOrb", typeof(GameObject)) as GameObject);
        orbs.Add("orange", Resources.Load(@"Orbs\OrangeOrb", typeof(GameObject)) as GameObject);
        orbs.Add("purple", Resources.Load(@"Orbs\PurpleOrb", typeof(GameObject)) as GameObject);
        orbs.Add("red", Resources.Load(@"Orbs\RedOrb", typeof(GameObject)) as GameObject);
        orbs.Add("white", Resources.Load(@"Orbs\WhiteOrb", typeof(GameObject)) as GameObject);
        orbs.Add("yellow", Resources.Load(@"Orbs\YellowOrb", typeof(GameObject)) as GameObject);
    }

    #region Orb Locations
    private void initializeLocationDictionary()
    {
        orbPositions.Add(1, new Vector3(24f, 0.7f, 0f));
        orbPositions.Add(2, new Vector3(12f, 0.7f, 0f));
        orbPositions.Add(3, new Vector3(0f, 0.7f, 0f));

        orbPositions.Add(4, new Vector3(20f, 0.7f, 4f));
        orbPositions.Add(5, new Vector3(12f, 0.7f, 4f));
        orbPositions.Add(6, new Vector3(4f, 0.7f, 4f));

        orbPositions.Add(7, new Vector3(16f, 0.7f, 8f));
        orbPositions.Add(8, new Vector3(12f, 0.7f, 8f));
        orbPositions.Add(9, new Vector3(8f, 0.7f, 8f));

        orbPositions.Add(10, new Vector3(24f, 0.7f, 12f));
        orbPositions.Add(11, new Vector3(20f, 0.7f, 12f));
        orbPositions.Add(12, new Vector3(16f, 0.7f, 12f));
        orbPositions.Add(13, new Vector3(8f, 0.7f, 12f));
        orbPositions.Add(14, new Vector3(4f, 0.7f, 12f));
        orbPositions.Add(15, new Vector3(0f, 0.7f, 12f));

        orbPositions.Add(16, new Vector3(16f, 0.7f, 16f));
        orbPositions.Add(17, new Vector3(12f, 0.7f, 16f));
        orbPositions.Add(18, new Vector3(8f, 0.7f, 16f));

        orbPositions.Add(19, new Vector3(20f, 0.7f, 20f));
        orbPositions.Add(20, new Vector3(12f, 0.7f, 20f));
        orbPositions.Add(21, new Vector3(4f, 0.7f, 20f));

        orbPositions.Add(22, new Vector3(24f, 0.7f, 24f));
        orbPositions.Add(23, new Vector3(12f, 0.7f, 24f));
        orbPositions.Add(24, new Vector3(0f, 0.7f, 24f));
    }
    #endregion

    #region Rune Material Dictionaries
    void initializeMaterialHighlightDictionary()
    {
        highlightMaterials.Add(0, Resources.Load(@"Runes\Stones1", typeof(Material)) as Material);
        highlightMaterials.Add(1, Resources.Load(@"Runes\Stones2", typeof(Material)) as Material);
        highlightMaterials.Add(2, Resources.Load(@"Runes\Stones3", typeof(Material)) as Material);
        highlightMaterials.Add(3, Resources.Load(@"Runes\Stones4", typeof(Material)) as Material);
        highlightMaterials.Add(4, Resources.Load(@"Runes\Stones5", typeof(Material)) as Material);
        highlightMaterials.Add(5, Resources.Load(@"Runes\Stones6", typeof(Material)) as Material);
        highlightMaterials.Add(6, Resources.Load(@"Runes\Stones7", typeof(Material)) as Material);
        highlightMaterials.Add(7, Resources.Load(@"Runes\Stones8", typeof(Material)) as Material);
        highlightMaterials.Add(8, Resources.Load(@"Runes\Stones9", typeof(Material)) as Material);
        highlightMaterials.Add(9, Resources.Load(@"Runes\Stones10", typeof(Material)) as Material);
    }

    void initializeMaterialOriginalDictionary()
    {
        originalMaterials.Add(0, Resources.Load(@"Runes\Stones1Dark", typeof(Material)) as Material);
        originalMaterials.Add(1, Resources.Load(@"Runes\Stones2Dark", typeof(Material)) as Material);
        originalMaterials.Add(2, Resources.Load(@"Runes\Stones3Dark", typeof(Material)) as Material);
        originalMaterials.Add(3, Resources.Load(@"Runes\Stones4Dark", typeof(Material)) as Material);
        originalMaterials.Add(4, Resources.Load(@"Runes\Stones5Dark", typeof(Material)) as Material);
        originalMaterials.Add(5, Resources.Load(@"Runes\Stones6Dark", typeof(Material)) as Material);
        originalMaterials.Add(6, Resources.Load(@"Runes\Stones7Dark", typeof(Material)) as Material);
        originalMaterials.Add(7, Resources.Load(@"Runes\Stones8Dark", typeof(Material)) as Material);
        originalMaterials.Add(8, Resources.Load(@"Runes\Stones9Dark", typeof(Material)) as Material);
        originalMaterials.Add(9, Resources.Load(@"Runes\Stones10Dark", typeof(Material)) as Material);
    }
    #endregion

    private void initializeShrineDictionary()
    {
        shrines.Add("black", Resources.Load(@"Shrines\ShrineBlack", typeof(GameObject)) as GameObject);
        shrines.Add("blue", Resources.Load(@"Shrines\ShrineBlue", typeof(GameObject)) as GameObject);
        shrines.Add("green", Resources.Load(@"Shrines\ShrineGreen", typeof(GameObject)) as GameObject);
        shrines.Add("orange", Resources.Load(@"Shrines\ShrineOrange", typeof(GameObject)) as GameObject);
        shrines.Add("purple", Resources.Load(@"Shrines\ShrinePurple", typeof(GameObject)) as GameObject);
        shrines.Add("red", Resources.Load(@"Shrines\ShrineRed", typeof(GameObject)) as GameObject);
        shrines.Add("white", Resources.Load(@"Shrines\ShrineWhite", typeof(GameObject)) as GameObject);
        shrines.Add("yellow", Resources.Load(@"Shrines\ShrineYellow", typeof(GameObject)) as GameObject);
    }
}
