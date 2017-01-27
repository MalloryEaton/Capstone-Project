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

    // Use this for initialization
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        runeRenderer = GetComponent<Renderer>();
        setRandomRotation();
        setRandomMaterial();
    }

    // Update is called once per frame
    void Update()
    {

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
                PlacementPhase();
                RemoveRuneHighlight();
                break;
            case "movementPickup":
                gameController.RemoveAllHighlights();
                MovementPhase_Pickup();
                break;
            case "movementPlace":
                MovementPhase_Place();
                gameController.RemoveAllHighlights();
                break;
            //case "removalPhase":
            //    RemovalPhase();
            //    break;
        }
        
        //hasBeenClicked = !hasBeenClicked;
        //if (hasBeenClicked)
        //{
        //    runeRenderer.material = highlightMaterial;
        //}
        //else
        //{
        //    runeRenderer.material = originalMaterial;
        //}
    }

    public void AddRuneHighlight()
    {
        runeRenderer.material = highlightMaterial;
    }

    public void RemoveRuneHighlight()
    {
        runeRenderer.material = originalMaterial;
    }

    private void PlacementPhase()
    {
        if (tag == "Empty")
        {
            if (gameController.isPlayerTurn)
            {
                GameObject playerOrb = (GameObject)Instantiate(dictionaries.orbsDictionary[gameController.playerColor], dictionaries.orbPositionsDictionary[runeNumber], Quaternion.identity);
                playerOrb.name = "PlayerOrb" + gameController.playerOrbCount;
                playerOrb.tag = "Rune" + runeNumber;
                tag = "Player";
                gameController.playerOrbCount++;
            }
            else //opponent's turn
            {
                GameObject opponentOrb = (GameObject)Instantiate(dictionaries.orbsDictionary[gameController.opponentColor], dictionaries.orbPositionsDictionary[runeNumber], Quaternion.identity);
                opponentOrb.name = "OpponentOrb" + gameController.opponentOrbCount;
                opponentOrb.tag = "Rune" + runeNumber;
                tag = "Opponent";
                gameController.opponentOrbCount++;
                gameController.turnCount++;
            }
            gameController.ChangeSide();
        }
    }

    private void MovementPhase_Pickup()
    {
        if (tag == "Player")
        {
            gameController.CheckForAvailableMoves(runeNumber);
        }
    }

    private void MovementPhase_Place()
    {
        if (tag == "Empty")
        {
            gameController.CheckIfLegalMove(runeNumber);
        }
        else
        {
            // Revert to phase 2
            gameController.RevertGamePhase();
        }
    }
}