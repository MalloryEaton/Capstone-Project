using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {
    public RuneController[] runeList;

    public string gamePhase; //placement, movementPickup, movementPlace, fly

    public bool isPlayerTurn;

    public short playerOrbCount = 0;
    public short opponentOrbCount = 0;
    public short turnCount = 1;

    private short fromLocation;

    public string playerColor = "green";
    public string opponentColor = "purple";

    private Dictionaries dictionaries;

    public bool moveOrb = false;

    // Use this for initialization
    void Start ()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        gamePhase = "placement";
        isPlayerTurn = true;
        Instantiate(dictionaries.shrinesDictionary[playerColor], new Vector3(12,0,12), Quaternion.identity);
    }

    //public void RevertGamePhase()
    //{
    //    gamePhase = tempGamePhase;
    //}

    public void RemoveAllHighlights()
    {
        for (int i = 0; i < runeList.Length; i++)
        {
            runeList[i].GetComponent<RuneController>().RemoveRuneHighlight();
        }
    }

    public void RemoveAllOrbHighlightsExceptSelected(short runeNumber)
    {
        for (int i = 0; i < runeList.Length; i++)
        {
            if(i != runeNumber)
            {
                runeList[i].GetComponent<RuneController>().RemoveRuneHighlight();
            }
        }
    }

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
        foreach (short rune in runes)
        {
            //add highlight to moveable runes
            runeList[rune].GetComponent<RuneController>().AddRuneHighlight();
        }
    }

    public bool IsLegalMove(short toLocation)
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
            //CheckForNewMills(moveToLocation);
            return true;
        }
        return false;
    }

    private IEnumerator MoveFunction(GameObject orb, Vector3 newPosition)
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            orb.transform.position = Vector3.Lerp(orb.transform.position, newPosition, timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if (orb.transform.position == newPosition)
            {
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
    }

    public void MoveOrb(short toLocation)
    {
        GameObject orbToMove = GameObject.Find("OrbAtLocation_" + fromLocation);

        StartCoroutine(MoveFunction(orbToMove, dictionaries.orbPositionsDictionary[toLocation]));

        orbToMove.name = "OrbAtLocation_" + toLocation;
        RemoveAllHighlights();
    }

    public void ChangeSide()
    {
        isPlayerTurn = !isPlayerTurn;

        CheckPhaseChange();
    }

    public void PlacementPhase(short runeNumber)
    {
        if (runeList[runeNumber].tag == "Empty")
        {
            if (isPlayerTurn)
            {
                GameObject playerOrb = (GameObject)Instantiate(dictionaries.orbsDictionary[playerColor], dictionaries.orbPositionsDictionary[runeNumber], Quaternion.identity);
                playerOrb.name = "OrbAtLocation_" + runeNumber;
                runeList[runeNumber].tag = "Player";
                playerOrbCount++;
            }
            else //opponent's turn
            {
                GameObject opponentOrb = (GameObject)Instantiate(dictionaries.orbsDictionary[opponentColor], dictionaries.orbPositionsDictionary[runeNumber], Quaternion.identity);
                opponentOrb.name = "OrbAtLocation_" + runeNumber;
                runeList[runeNumber].tag = "Opponent";
                opponentOrbCount++;
                turnCount++;
            }
            RemoveAllHighlights();
            ChangeSide();
        }
    }

    public void MovementPhase_Pickup()
    {
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
        if ((isPlayerTurn && runeList[selectedRuneNumber].tag == "Player") || (!isPlayerTurn && runeList[selectedRuneNumber].tag == "Opponent"))
        {
            fromLocation = selectedRuneNumber;

            //remove moveable pieces highlights except for one that is being moved
            RemoveAllOrbHighlightsExceptSelected(fromLocation);

            HighlightAvailableMoves(fromLocation);

            gamePhase = "movementPlace";
            //wait for click
        }
        //invalid click; wait until they select a piece
    }

    public void MovementPhase_Place(short toLocation)
    {
        if (runeList[toLocation].tag == "Empty")
        {
            if (IsLegalMove(toLocation))
            {
                MoveOrb(toLocation);
                runeList[toLocation].tag = (isPlayerTurn) ? "Player" : "Opponent";
                runeList[fromLocation].tag = "Empty";

                //switch to other player
                isPlayerTurn = !isPlayerTurn;
                gamePhase = "movementPickup";
                MovementPhase_Pickup();
            }
        }
        else if((isPlayerTurn && runeList[toLocation].tag == "Player") || (!isPlayerTurn && runeList[toLocation].tag == "Opponent")) //switch to highlighted piece
        {
            RemoveAllHighlights();
            HandlePieceSelect(toLocation);
        }
        else
        {

        }
        //check mills
        //  removal phase

        
    }

    private void CheckPhaseChange()
    {
        // After each player has placed all of their runes,
        // switch to the movement phase (phase 2).
        switch (gamePhase)
        {
            case "placement":
                if (turnCount > 3)
                //if (turnCount > 9)
                {
                    gamePhase = "movementPickup";
                    MovementPhase_Pickup();
                }
                break;
            case "movementPlace":
                //check win condition
                gamePhase = "movementPickup";
                MovementPhase_Pickup();
                break;
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
