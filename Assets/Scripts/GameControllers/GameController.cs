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

    private short fromLocation;

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

    //public void CheckForAvailableMoves(short runeNumber)
    //{
    //    bool canMove = false;

    //    #region Fly
    //    //if (CheckForFly())
    //    //{
    //    //    for (int i = 0; i < runeList.Length; i++)
    //    //    {
    //    //        if (HighlightAvailableMoves(i))
    //    //        {
    //    //            canMove = true;
    //    //        }
    //    //    }
    //    //}
    //    //else
    //    //{
    //    // The dictionary will produce the possible moves from
    //    // "runeLocation"
    //    // Highlight available movement options
    //    //foreach (short availableMove in dictionaries.adjacencyDictionary[runeNumber])
    //    //{
    //    //    canMove = HighlightAvailableMoves(availableMove) ? true : false;
    //    //}
    //    //}
    //    #endregion
        
    //    foreach (short availableMove in dictionaries.adjacencyDictionary[runeNumber])
    //    {
    //        canMove = HighlightAvailableMoves(availableMove) ? true : false;
    //    }

    //    if (canMove)
    //    {
    //        moveFromLocation = runeNumber;
    //        tempGamePhase = gamePhase;
    //        gamePhase = "movementPlace";
    //    }
    //    else
    //    {
    //        // Alert the user that this piece cannot be moved
    //    }
    //}

    public List<short> CheckAllAvailableMoves(List<short> runes)
    {
        List<short> runesThatCanBeMoved = new List<short>();
        bool canMakeAMove = false;

        if(runes.Count() > 2) //cannot fly
        {
            //check all pieces to see if there is a move available
            foreach (short rune in runes)
            {
                foreach (short availableMove in dictionaries.adjacencyDictionary[rune])
                {
                    if (runeList[availableMove].tag == "Empty")
                    {
                        if(!runesThatCanBeMoved.Contains(rune))
                        {
                            runesThatCanBeMoved.Add(rune);
                        }
                        canMakeAMove = true;
                    }
                }
            }
            if(canMakeAMove)
            {
                return runesThatCanBeMoved;
            }
            else //blocked
            {
                //lose message
            }
        }
        return new List<short>();

        #region Fly
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
        //foreach (short availableMove in dictionaries.adjacencyDictionary[runeNumber])
        //{
        //    canMove = HighlightAvailableMoves(availableMove) ? true : false;
        //}
        //}
        #endregion

    }

    private void HighlightAvailableMoves(short runeNumber)
    {
        foreach (short availableMove in dictionaries.adjacencyDictionary[runeNumber])
        {
            if (runeList[availableMove].tag == "Empty")
            {
                runeList[availableMove].GetComponent<RuneController>().AddRuneHighlight();
            }
        }
    }

    private void HighlightMoveablePieces(List<short> runes)
    {
        foreach(short rune in runes)
        {
            //add highlight to moveable runes
            //runeList[rune].GetComponent<RuneController>().AddRuneHighlight();
        }
    }

    public void CheckIfLegalMove(short toLocation)
    {
        // Check if the suggested move is a legal move from the selected rune
        if (dictionaries.adjacencyDictionary[fromLocation].Contains(toLocation))
            //|| CheckForFly())
        {
            // If the previous location was part of a mill, it isn't anymore
            // (and neither are the other pieces). Reset the state of all mills,
            // the next call to CheckForNewMills() will find them again
            //if (runeList[moveFromLocation].GetComponent<Rune>().isInMill == true)
            //{
            //    ResetMills();
            //}

            MoveOrb(toLocation);

            runeList[toLocation].tag = (isPlayerTurn) ? "Player" : "Opponent";

            runeList[fromLocation].tag = "Empty";

            //CheckForNewMills(moveToLocation);
        }
        else
        {
            RevertGamePhase();
        }
    }

    public void MoveOrb(short toLocation)
    {
        Vector3 pos = dictionaries.orbPositionsDictionary[fromLocation];
        GameObject orbToMove = GameObject.Find("OrbAtPosition_" + pos.x + "_" + pos.y + "_" + pos.z);
        orbToMove.transform.position = dictionaries.orbPositionsDictionary[toLocation];
        pos = dictionaries.orbPositionsDictionary[toLocation];
        orbToMove.name = "OrbAtPosition_" + pos.x + "_" + pos.y + "_" + pos.z;
        RemoveAllHighlights();
    }

    public void ChangeSide()
    {
        isPlayerTurn = !isPlayerTurn;

        CheckPhaseChange();
    }

    public void PlacementPhase(short runeNumber)
    {
        print("PlacementPhase");
        if (runeList[runeNumber].tag == "Empty")
        {
            if (isPlayerTurn)
            {
                print("here");
                GameObject playerOrb = (GameObject)Instantiate(dictionaries.orbsDictionary[playerColor], dictionaries.orbPositionsDictionary[runeNumber], Quaternion.identity);
                Vector3 pos = dictionaries.orbPositionsDictionary[runeNumber];
                playerOrb.name = "OrbAtPosition_" + pos.x + "_" + pos.y + "_" + pos.z;
                runeList[runeNumber].tag = "Player";
                playerOrbCount++;
            }
            else //opponent's turn
            {
                GameObject opponentOrb = (GameObject)Instantiate(dictionaries.orbsDictionary[opponentColor], dictionaries.orbPositionsDictionary[runeNumber], Quaternion.identity);
                Vector3 pos = dictionaries.orbPositionsDictionary[runeNumber];
                opponentOrb.name = "OrbAtPosition_" + pos.x + "_" + pos.y + "_" + pos.z;
                runeList[runeNumber].tag = "Opponent";
                opponentOrbCount++;
                turnCount++;
            }
            ChangeSide();
            RemoveAllHighlights();
        }
    }

    public void MovementPhase_Pickup()
    {
        print("MovementPhase_Pickup");
        List<short> runeNumbers = new List<short>();


        //make list of all pieces
        if (isPlayerTurn)
        {
            foreach(RuneController rune in runeList)
                if(rune.tag == "Player")
                    runeNumbers.Add(rune.runeNumber);
        }
        else
        {
            foreach (RuneController rune in runeList)
                if (rune.tag == "Opponent")
                    runeNumbers.Add(rune.runeNumber);
            
        }

        //check all available moves
        List<short> availableMoves = CheckAllAvailableMoves(runeNumbers); //returns a list of all moveable pieces
        if(availableMoves.Count > 0)
        {
            //highlight moveable pieces
            HighlightMoveablePieces(availableMoves); //make orbs hover?

            //wait for click
        }
        else
        {
            print("NO AVAILABLE MOVES! YOU LOSE!");
        }
    }

    public void HandlePieceSelect(short selectedRuneNumber)
    {
        print("HandlePieceSelect");
        if (runeList[selectedRuneNumber].tag == "Player")
        {
            fromLocation = selectedRuneNumber;
            //remove moveable pieces highlights except for one that is being moved
            HighlightAvailableMoves(fromLocation);

            gamePhase = "movementPlace";
            //wait for click
        }
        //invalid click; wait until they select a piece
    }

    public void MovementPhase_Place(short toLocation)
    {
        print("MovementPhase_Place");
        if (runeList[toLocation].tag == "Empty")
        {
            CheckIfLegalMove(toLocation);
        }
        else if(runeList[toLocation].tag == "Player") //switch to highlighted piece
        {
            RemoveAllHighlights();
            HandlePieceSelect(toLocation);
        }
        //check if placement legal
        //if empty
        //if player (reset placement phase)
        //else do nothing
        //move piece
        //check mills
        //removal phase
        //switch to Opponent
    }

    private void CheckPhaseChange()
    {
        // After each player has placed all of their runes,
        // switch to the movement phase (phase 2).
        if (gamePhase == "placement" && turnCount > 9)
        {
            gamePhase = "movementPickup";
            MovementPhase_Pickup();
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
