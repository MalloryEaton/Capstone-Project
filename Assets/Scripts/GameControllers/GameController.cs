using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {

    private RuneController runeController;
    public RuneController[] runeList;

    public string gamePhase; //placement, movementPickup, movementPlace, fly
    private string tempGamePhase;

    public bool isPlayerTurn;

    public short playerOrbCount = 0;
    public short opponentOrbCount = 0;
    public short turnCount = 1;

    private short moveFromLocation;

    public string playerColor = "green";
    public string opponentColor = "purple";

    private Dictionaries dictionaries;

    // Use this for initialization
    void Start ()
    {
        runeController = FindObjectOfType(typeof(RuneController)) as RuneController;
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        gamePhase = "placement";
        isPlayerTurn = true;
        Instantiate(dictionaries.shrinesDictionary[playerColor], new Vector3(12,0,12), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void RevertGamePhase()
    {
        gamePhase = tempGamePhase;
    }

    public void RemoveAllHighlights()
    {
        for (int i = 0; i < runeList.Length; i++)
        {
            runeList[i].GetComponent<RuneController>().RemoveRuneHighlight();
        }
    }

    public void CheckForAvailableMoves(short runeNumber)
    {
        bool canMove = false;

        //if (CheckForFly())
        //{
        //    for (int i = 0; i < runeList.Length; i++)
        //    {
        //        if (HighlightAvailableMoves(i))
        //        {
        //            canMove = true;
        //        }
        //    }
        //}
        //else
        //{
            // The dictionary will produce the possible moves from
            // "runeLocation"
            // Highlight available movement options
            foreach (short availableMove in dictionaries.adjacencyDictionary[runeNumber])
            {
                canMove = HighlightAvailableMoves(availableMove) ? true : false;
            }
        //}

        if (canMove)
        {
            moveFromLocation = runeNumber;
            tempGamePhase = gamePhase;
            gamePhase = "movementPlace";
        }
        else
        {
            // Alert the user that this piece cannot be moved
        }
    }

    private bool HighlightAvailableMoves(short runeNumber)
    {
        if (runeList[runeNumber].tag == "Empty")
        {
            runeList[runeNumber].GetComponent<RuneController>().AddRuneHighlight();
            return true;
        }

        return false;
    }

    public void CheckIfLegalMove(short moveToLocation)
    {
        // Check if the suggested move is a legal move from the 
        // selected rune
        if (dictionaries.adjacencyDictionary[moveFromLocation].Contains(moveToLocation))
            //|| CheckForFly())
        {
            // If the previous location was part of a mill, it isn't anymore
            // (and neither are the other pieces). Reset the state of all mills,
            // the next call to CheckForNewMills() will find them again
            //if (runeList[moveFromLocation].GetComponent<Rune>().isInMill == true)
            //{
            //    ResetMills();
            //}

            GameObject orbToMove = GameObject.FindGameObjectWithTag("Rune" + moveFromLocation);
            orbToMove.transform.position = dictionaries.orbPositionsDictionary[moveToLocation];

            runeList[moveToLocation].tag = (isPlayerTurn) ? "Player" : "Opponent";

            runeList[moveFromLocation].tag = "Empty";

            //CheckForNewMills(moveToLocation);
        }
        else
        {
            RevertGamePhase();
        }
    }

    public void ChangeSide()
    {
        isPlayerTurn = !isPlayerTurn;

        CheckPhaseChange();
    }

    private void CheckPhaseChange()
    {
        // After each player has placed all of their runes,
        // move to the movement phase (phase 2).
        if (gamePhase == "placement" && turnCount > 9)
        {
            gamePhase = "movementPickup";
        }

        //if (gamePhase == "movementSelectPhase")
        //{
        //    if (firstRuneCount == 3 && firstCanFly == false)
        //    {
        //        firstCanFly = true;
        //    }
        //    else if (secondRuneCount == 3 && secondCanFly == false)
        //    {
        //        secondCanFly = true;
        //    }
        //}
    }
}
