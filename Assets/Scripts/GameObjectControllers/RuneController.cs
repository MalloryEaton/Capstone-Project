using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{
    private GameController gameController;
    private Dictionaries dictionaries;

    private bool isInMill;

    public short runeNumber;
    private Renderer runeRenderer;

    private short numberOfMaterials = 10;

    private Material highlightMaterial;
    private Material originalMaterial;

    // Use this for initialization
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        runeRenderer = GetComponent<Renderer>();
        setRandomRotation();
        setRandomMaterial();
    }

    void setRandomRotation()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 1000f), 0);
    }

    void setRandomMaterial()
    {
        short num = (short)Random.Range(0, numberOfMaterials);
        highlightMaterial = dictionaries.runeHighlightsDictionary[num];
        runeRenderer.material = originalMaterial = dictionaries.runeOriginalsDictionary[num];
    }
    
    private void OnMouseEnter()
    {
        if(gameController.gamePhase == "placement" && tag == "Empty")
        {
            AddRuneHighlight();
        }
    }

    private void OnMouseExit()
    {
        if (gameController.gamePhase == "placement" && tag == "Empty")
        {
            RemoveRuneHighlight();
        }
    }

    //detect click
    private void OnMouseDown()
    {
        switch (gameController.gamePhase)
        {
            case "placement":
                gameController.PlacementPhase(runeNumber);
                break;
            case "movementPickup":
                gameController.HandlePieceSelect(runeNumber);
                break;
            case "movementPlace":
                gameController.MovementPhase_Place(runeNumber);
                break;
                //case "removalPhase":
                //    RemovalPhase();
                //    break;
        }
    }

    public void AddRuneHighlight()
    {
        runeRenderer.material = highlightMaterial;
    }

    public void RemoveRuneHighlight()
    {
        runeRenderer.material = originalMaterial;
    }
}