using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{
    public short runeNumber;
    private bool hasBeenClicked = false;
    private Renderer r;

    private short numberOfMaterials = 5;

    private Dictionary<short, Vector3> orbPositions;
    private Dictionary<short, Material> highlightMaterials;
    private Dictionary<short, Material> originalMaterials;

    private Material highlightMaterial;
    private Material originalMaterial;

    private void initializeLocationDictionary()
    {
        orbPositions.Add(1, new Vector3(25.8f, -7, -3.2f));
        orbPositions.Add(2, new Vector3(13.8f, -7, -3.2f));
        orbPositions.Add(3, new Vector3(1.8f, -7, -3.2f));
        orbPositions.Add(4, new Vector3(21.8f, -7, 0.8f));
        orbPositions.Add(5, new Vector3(13.8f, -7, 0.8f));
        orbPositions.Add(6, new Vector3(5.8f, -7, 0.8f));
        orbPositions.Add(7, new Vector3(17.8f, -7, 4.8f));
        orbPositions.Add(8, new Vector3(13.8f, -7, 4.8f));
        orbPositions.Add(9, new Vector3(9.8f, -7, 4.8f));
        orbPositions.Add(10, new Vector3(25.8f, -7, 8.8f));
        orbPositions.Add(11, new Vector3(21.8f, -7, 8.8f));
        orbPositions.Add(12, new Vector3(17.8f, -7, 8.8f));
        orbPositions.Add(13, new Vector3(9.8f, -7, 8.8f));
        orbPositions.Add(14, new Vector3(5.8f, -7, 8.8f));
        orbPositions.Add(15, new Vector3(1.8f, -7, 8.8f));
        orbPositions.Add(16, new Vector3(17.8f, -7, 12.8f));
        orbPositions.Add(17, new Vector3(13.8f, -7, 12.8f));
        orbPositions.Add(18, new Vector3(9.8f, -7, 12.8f));
        orbPositions.Add(19, new Vector3(21.8f, -7, 16.8f));
        orbPositions.Add(20, new Vector3(13.8f, -7, 16.8f));
        orbPositions.Add(21, new Vector3(5.8f, -7, 16.8f));
        orbPositions.Add(22, new Vector3(25.8f, -7, 20.8f));
        orbPositions.Add(23, new Vector3(13.8f, -7, 20.8f));
        orbPositions.Add(24, new Vector3(1.8f, -7, 20.8f));
    }

    void initializeMaterialHighlightDictionary()
    {
        highlightMaterials.Add(0, Resources.Load("Stones1", typeof(Material)) as Material);
        highlightMaterials.Add(1, Resources.Load("Stones2", typeof(Material)) as Material);
        highlightMaterials.Add(2, Resources.Load("Stones3", typeof(Material)) as Material);
        highlightMaterials.Add(3, Resources.Load("Stones4", typeof(Material)) as Material);
        highlightMaterials.Add(4, Resources.Load("Stones5", typeof(Material)) as Material);
    }

    void initializeMaterialOriginalDictionary()
    {
        originalMaterials.Add(0, Resources.Load("Stones1Dark", typeof(Material)) as Material);
        originalMaterials.Add(1, Resources.Load("Stones2Dark", typeof(Material)) as Material);
        originalMaterials.Add(2, Resources.Load("Stones3Dark", typeof(Material)) as Material);
        originalMaterials.Add(3, Resources.Load("Stones4Dark", typeof(Material)) as Material);
        originalMaterials.Add(4, Resources.Load("Stones5Dark", typeof(Material)) as Material);
    }

    void setRandomRotation()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 1000f), 0);
    }

    void setRandomMaterial()
    {
        short num = (short)Random.Range(0, numberOfMaterials);
        highlightMaterial = highlightMaterials[num];
        r.material = originalMaterial = originalMaterials[num];
    }

    // Use this for initialization
    void Start()
    {
        //runeMaterials = new List<Material>(5);
        orbPositions = new Dictionary<short, Vector3>();
        highlightMaterials = new Dictionary<short, Material>();
        originalMaterials = new Dictionary<short, Material>();
        r = GetComponent<Renderer>();
        initializeMaterialHighlightDictionary();
        initializeMaterialOriginalDictionary();
        setRandomRotation();
        setRandomMaterial();
        initializeLocationDictionary();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //detect click
    private void OnMouseDown()
    {
        hasBeenClicked = !hasBeenClicked;
        if (hasBeenClicked)
        {
            r.material = highlightMaterial;
        }
        else
        {
            r.material = originalMaterial;
        }

        //GameObject blackOrb = GameObject.Find("BlackOrb");
        //blackOrb.transform.position = orbPositions[locationNumber];
    }
}