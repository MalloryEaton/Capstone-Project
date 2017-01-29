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

    private List<short> runesThatCanBeMoved;

    // Use this for initialization
    void Start ()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        gamePhase = "placement";
        isPlayerTurn = true;
        Instantiate(dictionaries.shrinesDictionary[playerColor], new Vector3(12,0,12), Quaternion.identity);
        runesThatCanBeMoved = new List<short>();
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

    private void ChangeSide()
    {
        isPlayerTurn = !isPlayerTurn;

        CheckPhaseChange();
    }

    #region Orbs
    private void HighlightMoveableOrbs(List<short> runes)
    {
        foreach (short rune in runes)
        {
            MakeOrbHover(rune);
        }
    }

    private void RemoveAllOrbHighlightsExceptSelected(short runeNumber)
    {
        List<short> runes = MakeListOfRunes();

        foreach (short rune in runes)
        {
            if (rune != runeNumber)
            {
                RemoveOrbHighlight(rune);
            }
        }

        GameObject selectedOrb = GameObject.Find("OrbAtLocation_" + runeNumber);
        if (!selectedOrb.GetComponent<OrbController>())
        {
            selectedOrb.AddComponent<OrbController>();
        }
    }
    
    private void RemoveOrbHighlight(short runeNumber)
    {
        GameObject orb = GameObject.Find("OrbAtLocation_" + runeNumber);
        Destroy(orb.GetComponent<OrbController>());
        orb.transform.position = dictionaries.orbPositionsDictionary[runeNumber];
    }

    private void MakeOrbHover(short rune)
    {
        GameObject orb = GameObject.Find("OrbAtLocation_" + rune);
        orb.AddComponent<OrbController>();
    }

    private void MoveOrb(short toLocation)
    {
        GameObject orbToMove = GameObject.Find("OrbAtLocation_" + fromLocation);

        StartCoroutine(MoveOrbAnimation(orbToMove, dictionaries.orbPositionsDictionary[toLocation]));

        orbToMove.name = "OrbAtLocation_" + toLocation;
        RemoveAllRuneHighlights();
    }

    private IEnumerator MoveOrbAnimation(GameObject orb, Vector3 newPosition)
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
    #endregion

    #region Runes
    private List<short> MakeListOfRunes()
    {
        List<short> runeNumbers = new List<short>();

        if (isPlayerTurn)
        {
            foreach (RuneController rune in runeList)
                if (rune.tag == "Player")
                    runeNumbers.Add(rune.runeNumber);
        }
        else
        {
            foreach (RuneController rune in runeList)
                if (rune.tag == "Opponent")
                    runeNumbers.Add(rune.runeNumber);

        }
        return runeNumbers;
    }

    private void RemoveAllRuneHighlights()
    {
        for (int i = 0; i < runeList.Length; i++)
        {
            runeList[i].GetComponent<RuneController>().RemoveRuneHighlight();
        }
    }
    #endregion

    #region Moves
    private bool CheckAllAvailableMoves(List<short> runes)
    {
        List<short> moveableRunes = new List<short>();
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
                        if(!moveableRunes.Contains(rune))
                        {
                            moveableRunes.Add(rune);
                        }
                        canMakeAMove = true;
                    }
                }
            }
            runesThatCanBeMoved.Clear();
            runesThatCanBeMoved.AddRange(moveableRunes);
            return canMakeAMove;
        }
        return false;
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

    private bool IsLegalMove(short toLocation)
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
    #endregion
    
    #region Placement Phase
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
            RemoveAllRuneHighlights();
            ChangeSide();
        }
    }
    #endregion

    #region Movement Phase
    public void MovementPhase_Pickup()
    {
        List<short> runes = MakeListOfRunes();

        //check all available moves
        bool canMove = CheckAllAvailableMoves(runes); //returns a list of all moveable pieces
        if (canMove)
        {
            //highlight moveable pieces
            HighlightMoveableOrbs(runesThatCanBeMoved); //make orbs hover?

            //wait for click
        }
        else
        {
            print("NO AVAILABLE MOVES! YOU LOSE!");
        }
    }

    public void HandleOrbSelect(short selectedRuneNumber)
    {
        if ((isPlayerTurn && runeList[selectedRuneNumber].tag == "Player" && runesThatCanBeMoved.Contains(selectedRuneNumber)) || 
            (!isPlayerTurn && runeList[selectedRuneNumber].tag == "Opponent"))
        {
            fromLocation = selectedRuneNumber;
            
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
                GameObject orb = GameObject.Find("OrbAtLocation_" + fromLocation);
                Destroy(orb.GetComponent<OrbController>());

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
            RemoveAllRuneHighlights();
            HandleOrbSelect(toLocation);
        }
        else
        {

        }
        //check mills
        //  removal phase

        
    }
    #endregion
}
