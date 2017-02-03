using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogicController : MonoBehaviour
{
    public RuneController[] runeList;

    public string gamePhase; //placement, movementPickup, movementPlace, fly

    public bool isPlayerTurn;

    public short playerOrbCount = 0;
    public short opponentOrbCount = 0;
    public short placementPhase_RoundCount = 1;

    private short movementPhase_FromLocation;

    public string playerColor = "green";
    public string opponentColor = "purple";

    private Dictionaries dictionaries;

    private List<short> movementPhase_RunesThatCanBeMoved;

    void Start()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        gamePhase = "placement";
        isPlayerTurn = true;
        Instantiate(dictionaries.shrinesDictionary[playerColor], new Vector3(12, 0, 12), Quaternion.identity);
        movementPhase_RunesThatCanBeMoved = new List<short>();
    }

    public void PlacementPhase(short rune)
    {
        if (runeList[rune].tag == "Empty")
        {
            if (isPlayerTurn)
            {
                GameObject playerOrb = Instantiate(dictionaries.orbsDictionary[playerColor], dictionaries.orbPositionsDictionary[rune], Quaternion.identity);
                playerOrb.name = "OrbAtLocation_" + rune;
                runeList[rune].tag = "Player";
                playerOrbCount++;
            }
            else //opponent's turn
            {
                GameObject opponentOrb = Instantiate(dictionaries.orbsDictionary[opponentColor], dictionaries.orbPositionsDictionary[rune], Quaternion.identity);
                opponentOrb.name = "OrbAtLocation_" + rune;
                runeList[rune].tag = "Opponent";
                opponentOrbCount++;
                placementPhase_RoundCount++;
            }

            if (CheckIfRuneIsInMill(rune))
            {
                print("YOU GOT A MILL!");
                gamePhase = "removal";
                HighlightMoveableOrbs(MakeListOfRunesForOneSide());
                //wait for click
            }
            else
            {
                ChangeSide();
            }

            RemoveAllRuneHighlights();
        }
    }

    public void MovementPhase_Pickup()
    {
        List<short> runes = MakeListOfRunesForOneSide();

        if (CheckForAvailableMoves(runes))
        {
            HighlightMoveableOrbs(movementPhase_RunesThatCanBeMoved);

            //wait for click
        }
        else
        {
            print("NO AVAILABLE MOVES! YOU LOSE!");
        }
    }

    public void HandleOrbSelect(short selectedRune)
    {
        if (((isPlayerTurn && runeList[selectedRune].tag == "Player") ||
            (!isPlayerTurn && runeList[selectedRune].tag == "Opponent")) && movementPhase_RunesThatCanBeMoved.Contains(selectedRune))
        {
            movementPhase_FromLocation = selectedRune;

            RemoveAllRuneHighlights();
            RemoveAllOrbHighlights(movementPhase_FromLocation);
            HighlightAvailableMoves(movementPhase_FromLocation);

            gamePhase = "movementPlace";

            //wait for click
        }
    }

    public void MovementPhase_Place(short toLocation)
    {
        if (runeList[toLocation].tag == "Empty")
        {
            if (IsLegalMove(toLocation))
            {
                GameObject orb = GameObject.Find("OrbAtLocation_" + movementPhase_FromLocation);
                Destroy(orb.GetComponent<OrbController>());

                MoveOrb(toLocation);
                runeList[toLocation].tag = (isPlayerTurn) ? "Player" : "Opponent";
                runeList[movementPhase_FromLocation].tag = "Empty";
                runeList[movementPhase_FromLocation].isInMill = false;

                //check for a mill
                if (CheckIfRuneIsInMill(toLocation))
                {
                    print("YOU GOT A MILL!");
                    gamePhase = "removal";
                    HighlightMoveableOrbs(MakeListOfRunesForOneSide());
                    //wait for click
                }
                else
                {
                    ChangeSide();
                }
            }
        }
        else if ((isPlayerTurn && runeList[toLocation].tag == "Player") || (!isPlayerTurn && runeList[toLocation].tag == "Opponent")) //switch to highlighted piece
        {
            HandleOrbSelect(toLocation);
        }

    }

    public void RemovalPhase(short rune)
    {
        Destroy(GameObject.Find("OrbAtLocation_" + rune));

        runeList[rune].tag = "Empty";
        if(isPlayerTurn)
        {
            opponentOrbCount--;
        }
        else
        {
            playerOrbCount--;
        }

        RemoveAllOrbHighlights(rune);

        //check for win
        if (isPlayerTurn && opponentOrbCount == 2)
        {
            print("Game Over. " + playerColor + " wins!");
            gamePhase = "over";
        }
        else if (!isPlayerTurn && playerOrbCount == 2)
        {
            print("Game Over. " + opponentColor + " wins!");
            gamePhase = "over";
        }
        else
        {
            if(placementPhase_RoundCount <= 4)
            {
                gamePhase = "placement";
            }
            ChangeSide();
        }
    }

    private bool CheckIfRuneIsInMill(short rune)
    {
        if (CheckMillsHorizontally(rune) || CheckMillsVertically(rune))
        {
            return true;
        }

        return false;
    }

    private bool CheckMillsHorizontally(short rune)
    {
        for (int i = 0; i <= 21; i += 3)
        {
            if (runeList[i].tag == runeList[i + 1].tag &&
                runeList[i].tag == runeList[i + 2].tag &&
                runeList[i].tag != "Empty")
            {
                runeList[i].isInMill = true;
                runeList[i + 1].isInMill = true;
                runeList[i + 2].isInMill = true;

                if (((rune == i || rune == (i + 1) || rune == (i + 2)) && isPlayerTurn && runeList[rune].tag == "Player") ||
                    ((rune == i || rune == (i + 1) || rune == (i + 2)) && !isPlayerTurn && runeList[rune].tag == "Opponent"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckMillsVertically(short rune)
    {
        foreach (Mill mill in dictionaries.verticalMillsList)
        {
            if (runeList[mill.position1].tag == runeList[mill.position2].tag &&
            runeList[mill.position2].tag == runeList[mill.position3].tag &&
            runeList[mill.position1].tag != "Empty")
            {
                runeList[mill.position1].isInMill = true;
                runeList[mill.position2].isInMill = true;
                runeList[mill.position3].isInMill = true;

                if (((rune == mill.position1 || rune == mill.position2 || rune == mill.position3) && isPlayerTurn && runeList[rune].tag == "Player") ||
                    ((rune == mill.position1 || rune == mill.position2 || rune == mill.position3) && !isPlayerTurn && runeList[rune].tag == "Opponent"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsLegalMove(short toLocation)
    {
        if (dictionaries.adjacencyDictionary[movementPhase_FromLocation].Contains(toLocation))
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

    private List<short> MakeListOfRunesForOneSide()
    {
        List<short> runes = new List<short>();

        if (gamePhase == "removal")
        {
            foreach (RuneController rune in runeList)
                if (((isPlayerTurn && rune.tag == "Opponent") || (!isPlayerTurn && rune.tag == "Player")) && !rune.isInMill)
                    runes.Add(rune.runeNumber);
        }
        else
        {
            foreach (RuneController rune in runeList)
                if ((isPlayerTurn && rune.tag == "Player") || (!isPlayerTurn && rune.tag == "Opponent"))
                    runes.Add(rune.runeNumber);
        }

        return runes;
    }

    private bool CheckForAvailableMoves(List<short> runes)
    {
        List<short> moveableRunes = new List<short>();
        bool canMakeAMove = false;

        if (runes.Count() > 2) //cannot fly
        {
            //check all pieces to see if there is a move available
            foreach (short rune in runes)
            {
                foreach (short availableMove in dictionaries.adjacencyDictionary[rune])
                {
                    if (runeList[availableMove].tag == "Empty")
                    {
                        if (!moveableRunes.Contains(rune))
                        {
                            moveableRunes.Add(rune);
                        }
                        canMakeAMove = true;
                    }
                }
            }
            movementPhase_RunesThatCanBeMoved.Clear();
            movementPhase_RunesThatCanBeMoved.AddRange(moveableRunes);
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

    private void ChangeSide()
    {
        isPlayerTurn = !isPlayerTurn;

        switch(gamePhase)
        {
            case "placement":
                if (placementPhase_RoundCount > 4)
                //if (turnCount > 9)
                {
                    gamePhase = "movementPickup";
                    MovementPhase_Pickup();
                }
                break;
            case "movementPlace":
                gamePhase = "movementPickup";
                MovementPhase_Pickup();
                break;
            case "removal":
                gamePhase = "movementPickup";
                MovementPhase_Pickup();
                break;
        }
    }

    private void RemoveAllRuneHighlights()
    {
        for (int i = 0; i < runeList.Length; i++)
        {
            runeList[i].GetComponent<RuneController>().RemoveRuneHighlight();
        }
    }

    private void HighlightMoveableOrbs(List<short> runes)
    {
        foreach (short rune in runes)
        {
            MakeOrbHover(rune);
        }
    }

    private void MakeOrbHover(short rune)
    {
        GameObject orb = GameObject.Find("OrbAtLocation_" + rune);
        orb.AddComponent<OrbController>();
    }

    private void RemoveAllOrbHighlights(short runeNumber)
    {
        print("Blah");
        List<short> runes = MakeListOfRunesForOneSide();

        foreach(short rune in runes)
        {
            if (rune != runeNumber)
            {
                RemoveOrbHighlight(rune);
            }
        }

        if (gamePhase != "removal")
        {
            //if selected orb was not previously hovering, make it hover
            GameObject selectedOrb = GameObject.Find("OrbAtLocation_" + runeNumber);
            if (!selectedOrb.GetComponent<OrbController>())
            {
                selectedOrb.AddComponent<OrbController>();
            }
        }
    }

    private void RemoveOrbHighlight(short rune)
    {
        print("Remove orb " + rune);
        GameObject orb = GameObject.Find("OrbAtLocation_" + rune);
        Destroy(orb.GetComponent<OrbController>());
        orb.transform.position = dictionaries.orbPositionsDictionary[rune];
    }

    private void HighlightAvailableMoves(short rune)
    {
        foreach (short availableMove in dictionaries.adjacencyDictionary[rune])
        {
            if (runeList[availableMove].tag == "Empty")
            {
                runeList[availableMove].GetComponent<RuneController>().AddRuneHighlight();
            }
        }
    }

    private void MoveOrb(short toLocation)
    {
        GameObject orbToMove = GameObject.Find("OrbAtLocation_" + movementPhase_FromLocation);

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

    private bool CheckIfAllRunesAreInMills()
    {
        return false;
    }

    private void RemoveAllMills()
    {

    }

    private void LocateAllMills()
    {

    }
}