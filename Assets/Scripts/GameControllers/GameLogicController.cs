using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogicController : MonoBehaviour
{
    public RuneController[] runeList;

    public string gamePhase; //placement, movementPickup, movementPlace, fly
    private string previousGamePhase;

    public bool isPlayerTurn;

    public short playerOrbCount = 0;
    public short opponentOrbCount = 0;
    public short placementPhase_RoundCount = 1;

    private short movementPhase_FromLocation;

    public string playerColor = "green";
    public string opponentColor = "purple";

    private List<Mill> playerMills;
    private List<Mill> opponentMills;

    private Dictionaries dictionaries;

    private List<short> movementPhase_RunesThatCanBeMoved;

    void Start()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        gamePhase = "placement";
        isPlayerTurn = true;
        Instantiate(dictionaries.shrinesDictionary[playerColor], new Vector3(12, 0, 12), Quaternion.identity);
        movementPhase_RunesThatCanBeMoved = new List<short>();
        playerMills = new List<Mill>();
        opponentMills = new List<Mill>();
    }

    /*---------------------------------------------------------------------
    || GAME PHASE FUNCTIONS
    -----------------------------------------------------------------------*/
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

            if (RuneIsInMill(rune))
            {
                print("YOU GOT A MILL!");
                previousGamePhase = "placement";
                gamePhase = "removal";
                HighlightMoveableOrbs(MakeListOfRunesForRemoval());
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

        if (AvailableMovesExist(runes))
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

            previousGamePhase = "movementPickup";
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

                if(runeList[movementPhase_FromLocation].isInMill)
                {
                    RemoveRunesFromMill();
                }
                
                if (RuneIsInMill(toLocation))
                {
                    print("YOU GOT A MILL!");
                    previousGamePhase = "movementPlace";
                    gamePhase = "removal";
                    RemoveAllOrbHighlights(-1);
                    HighlightMoveableOrbs(MakeListOfRunesForRemoval());
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
            previousGamePhase = "movementPlace";
            HandleOrbSelect(toLocation);
        }
    }

    public void RemovalPhase(short rune)
    {
        if((isPlayerTurn && runeList[rune].tag == "Opponent") || (!isPlayerTurn && runeList[rune].tag == "Player"))
        {
            if (runeList[rune].isInMill)
                RemoveRunesFromMill();

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

            if(previousGamePhase == "placement")
            {
                gamePhase = previousGamePhase;
                ChangeSide();
            }
            else
            {
                if (playerOrbCount == 2 || opponentOrbCount == 2)
                    GameOver();
                else
                    ChangeSide();
            }
        }
    }

    private void GameOver()
    {
        RemoveAllOrbHighlights(-1);
        RemoveAllRuneHighlights();
        if(isPlayerTurn)
            print("Game Over. " + playerColor + " wins!");
        else
            print("Game Over. " + opponentColor + " wins!");
    }


    /*---------------------------------------------------------------------
    || GAME PHASE LOGIC
    -----------------------------------------------------------------------*/
    private bool IsLegalMove(short toLocation)
    {
        if((isPlayerTurn && playerOrbCount == 3) || (!isPlayerTurn && opponentOrbCount == 3)) //can fly
        {
            if (runeList[toLocation])
                return true;
        }
        else if(dictionaries.adjacencyDictionary[movementPhase_FromLocation].Contains(toLocation))
        {
            return true;
        }

        return false;
    }

    private void ChangeSide()
    {
        isPlayerTurn = !isPlayerTurn;

        if(gamePhase == "placement")
        {
            if (placementPhase_RoundCount > 5)
            //if (turnCount > 9)
            {
                gamePhase = "movementPickup";
                MovementPhase_Pickup();
            }
        }
        else
        {
            gamePhase = "movementPickup";
            RemoveAllOrbHighlights(-1);
            MovementPhase_Pickup();
        }
    }

    private List<short> MakeListOfRunesForRemoval()
    {
        List<short> runes = new List<short>();

        if (AllRunesAreInMills())
        {
            foreach (RuneController rune in runeList)
                if ((isPlayerTurn && rune.tag == "Opponent") || (!isPlayerTurn && rune.tag == "Player"))
                    runes.Add(rune.runeNumber);
        }
        else //only add runes that are not in mills
        {
            foreach (RuneController rune in runeList)
                if (((isPlayerTurn && rune.tag == "Opponent") || (!isPlayerTurn && rune.tag == "Player")) && !runeList[rune.runeNumber].isInMill)
                    runes.Add(rune.runeNumber); 
        }

        return runes;
    }

    private List<short> MakeListOfRunesForOneSide()
    {
        List<short> runes = new List<short>();

        foreach (RuneController rune in runeList)
            if ((isPlayerTurn && rune.tag == "Player") || (!isPlayerTurn && rune.tag == "Opponent"))
                runes.Add(rune.runeNumber);

        return runes;
    }

    private bool AvailableMovesExist(List<short> runes)
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
        }
        else
        {
            foreach(short rune in runes)
            {
                if (runeList[rune].tag == "Empty")
                {
                    if (!moveableRunes.Contains(rune))
                    {
                        moveableRunes.Add(rune);
                    }
                    canMakeAMove = true;
                }
            }
        }
        return canMakeAMove;
    }


    /*---------------------------------------------------------------------
    || MILL FUNCTIONS
    -----------------------------------------------------------------------*/
    private bool RuneIsInMill(short rune)
    {
        if (IsInHorizontalMill(rune) || IsInVerticalMill(rune))
        {
            return true;
        }

        return false;
    }

    private bool AllRunesAreInMills()
    {
        List<short> runes = MakeListOfRunesForOneSide();

        foreach(short rune in runes)
        {
            if (!runeList[rune].isInMill)
                return false;
        }

        return true;
    }

    private void RemoveRunesFromMill()
    {
        if (isPlayerTurn)
        {
            Mill mill;
            for (short i = 0; i < playerMills.Count; i++)
            {
                mill = playerMills[i];
                if (mill.position1 == movementPhase_FromLocation || mill.position2 == movementPhase_FromLocation || mill.position3 == movementPhase_FromLocation)
                {
                    runeList[mill.position1].isInMill = false;
                    runeList[mill.position2].isInMill = false;
                    runeList[mill.position3].isInMill = false;
                    playerMills.Remove(playerMills[i]);
                }
            }
        }
        else
        { 
            Mill mill;
            for (short i = 0; i < playerMills.Count; i++)
            {
                    mill = opponentMills[i];
                if (mill.position1 == movementPhase_FromLocation || mill.position2 == movementPhase_FromLocation || mill.position3 == movementPhase_FromLocation)
                {
                    runeList[mill.position1].isInMill = false;
                    runeList[mill.position2].isInMill = false;
                    runeList[mill.position3].isInMill = false;
                    opponentMills.Remove(mill);
                }
            }
        }
    }

    private bool IsInHorizontalMill(short rune)
    {
        for (short i = 0; i <= 21; i += 3)
        {
            if (runeList[i].tag == runeList[i + 1].tag &&
                runeList[i].tag == runeList[i + 2].tag &&
                runeList[i].tag != "Empty")
            {
                if (((rune == i || rune == (i + 1) || rune == (i + 2)) && isPlayerTurn && runeList[rune].tag == "Player") ||
                    ((rune == i || rune == (i + 1) || rune == (i + 2)) && !isPlayerTurn && runeList[rune].tag == "Opponent"))
                {
                    runeList[i].isInMill = true;
                    runeList[i + 1].isInMill = true;
                    runeList[i + 2].isInMill = true;

                    if (isPlayerTurn)
                        playerMills.Add(new Mill(i, (short)(i + 1), (short)(i + 2)));
                    else
                        opponentMills.Add(new Mill(i, (short)(i + 1), (short)(i + 2)));

                    return true;
                }
            }
        }
        return false;
    }

    private bool IsInVerticalMill(short rune)
    {
        foreach (Mill mill in dictionaries.verticalMillsList)
        {
            if (runeList[mill.position1].tag == runeList[mill.position2].tag &&
            runeList[mill.position2].tag == runeList[mill.position3].tag &&
            runeList[mill.position1].tag != "Empty")
            {
                if (((rune == mill.position1 || rune == mill.position2 || rune == mill.position3) && isPlayerTurn && runeList[rune].tag == "Player") ||
                    ((rune == mill.position1 || rune == mill.position2 || rune == mill.position3) && !isPlayerTurn && runeList[rune].tag == "Opponent"))
                {
                    runeList[mill.position1].isInMill = true;
                    runeList[mill.position2].isInMill = true;
                    runeList[mill.position3].isInMill = true;

                    if (isPlayerTurn)
                        playerMills.Add(new Mill(mill.position1, mill.position2, mill.position3));
                    else
                        opponentMills.Add(new Mill(mill.position1, mill.position2, mill.position3));

                    return true;
                }
            }
        }
        return false;
    }


    /*---------------------------------------------------------------------
    || VISUALS FUNCTIONS
    -----------------------------------------------------------------------*/
    // Runes //
    private void RemoveAllRuneHighlights()
    {
        for (int i = 0; i < runeList.Length; i++)
        {
            runeList[i].GetComponent<RuneController>().RemoveRuneHighlight();
        }
    }

    private void HighlightAvailableMoves(short rune)
    {
        if ((isPlayerTurn && playerOrbCount == 3) || (!isPlayerTurn && opponentOrbCount == 3)) //can fly
        {
            foreach (RuneController r in runeList)
            {
                if (r.tag == "Empty")
                {
                    r.GetComponent<RuneController>().AddRuneHighlight();
                }
            }
        }
        else
        {
            foreach (short availableMove in dictionaries.adjacencyDictionary[rune])
            {
                if (runeList[availableMove].tag == "Empty")
                {
                    runeList[availableMove].GetComponent<RuneController>().AddRuneHighlight();
                }
            }
        }
    }

    // Orbs //
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
        if(runeNumber == -1) // remove all
        {
            foreach (RuneController rune in runeList)
            {
                RemoveOrbHighlight(rune.runeNumber);
            }
        }
        else
        {
            List<short> runes = MakeListOfRunesForOneSide();

            foreach (short rune in runes)
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
    }

    private void RemoveOrbHighlight(short rune)
    {
        GameObject orb = GameObject.Find("OrbAtLocation_" + rune);
        Destroy(orb.GetComponent<OrbController>());
        orb.transform.position = dictionaries.orbPositionsDictionary[rune];
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
}