using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogicController : MonoBehaviour
{
    public RuneController[] runeList;
    private Dictionaries dictionaries;

    public string playerColor = "Green";
    public string opponentColor = "Purple";

    public string gamePhase; //placement, movementPickup, movementPlace, removal
    private string previousGamePhase;

    public bool isPlayerTurn;

    private short startingNumberOfOrbs;
    private short playerOrbCount;
    private short opponentOrbCount;
    private short placementPhase_RoundCount;
    
    private List<Mill> playerMills;
    private List<Mill> opponentMills;

    private short runeFromLocation;
    private List<short> runesThatCanBeMoved;
    private List<short> runesThatCanBeRemoved;

    void Start()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        gamePhase = "placement";
        previousGamePhase = "placement";
        isPlayerTurn = true;
        startingNumberOfOrbs = 9;
        playerOrbCount = 0;
        opponentOrbCount = 0;
        placementPhase_RoundCount = 1;
        playerMills = new List<Mill>();
        opponentMills = new List<Mill>();
        runesThatCanBeMoved = new List<short>();
        runesThatCanBeRemoved = new List<short>();
        Instantiate(dictionaries.shrinesDictionary[playerColor], new Vector3(12, 0, 12), Quaternion.identity);
    }

    /*---------------------------------------------------------------------
    || GAME PHASE FUNCTIONS
    -----------------------------------------------------------------------*/
    // Placement //
    public void PlacementPhase(short rune)
    {
        if (runeList[rune].tag == "Empty")
        {
            if (isPlayerTurn)
            {
                MoveOrb(rune);
                runeList[rune].tag = "Player";
                playerOrbCount++;
            }
            else //opponent's turn
            {
                MoveOrb(rune);
                runeList[rune].tag = "Opponent";
                opponentOrbCount++;
                placementPhase_RoundCount++;
            }

            RemoveAllRuneHighlights();

            previousGamePhase = "placement";

            if (RuneIsInMill(rune))
            {
                PrepareForRemovalPhase();
            }
            else
            {
                ChangeSide();
            }
        }
    }

    // Movement //
    private void PrepareForMovementPhase()
    {
        if (ThereIsAnAvailableMove(MakeListOfRunesForCurrentPlayer()))
        {
            HighlightMoveableOrbs(runesThatCanBeMoved);
        }
        else
        {
            print("NO AVAILABLE MOVES! YOU LOSE!");
            isPlayerTurn = !isPlayerTurn;
            GameOver();
        }
    }

    public void MovementPhase_Pickup(short selectedRune)
    {
        if(RuneCanBeMoved(selectedRune))
        {
            runeFromLocation = selectedRune;

            RemoveAllRuneHighlights();
            RemoveAllOrbHighlights(selectedRune);
            HighlightAvailableMoves(selectedRune);

            previousGamePhase = gamePhase;
            gamePhase = "movementPlace";
        }
    }

    public void MovementPhase_Place(short toLocation)
    {
        if (runeList[toLocation].tag == "Empty")
        {
            if (IsLegalMove(toLocation))
            {
                RemoveOrbHighlight(runeFromLocation);

                MoveOrb(toLocation);

                runeList[toLocation].tag = (isPlayerTurn) ? "Player" : "Opponent";
                runeList[runeFromLocation].tag = "Empty";

                if(runeList[runeFromLocation].isInMill)
                {
                    RemoveRunesFromMill();
                }
                
                if (RuneIsInMill(toLocation))
                {
                    PrepareForRemovalPhase();
                }
                else
                {
                    ChangeSide();
                }
            }
        }
        else if (ClickedOnDifferentPiece(toLocation)) //switch to highlighted piece
        {
            previousGamePhase = "movementPlace";
            MovementPhase_Pickup(toLocation);
        }
    }

    // Removal //
    private void PrepareForRemovalPhase()
    {
        print("YOU GOT A MILL!");
        previousGamePhase = gamePhase;
        gamePhase = "removal";
        HighlightMoveableOrbs(MakeListOfRunesThatCanBeRemoved());
    }

    public void RemovalPhase(short runeToRemove)
    {
        if(RuneCanBeRemoved(runeToRemove))
        {
            if (runeList[runeToRemove].isInMill)
                RemoveRunesFromMill();

            Destroy(GameObject.Find("OrbAtLocation_" + runeToRemove));
            runeList[runeToRemove].tag = "Empty";

            if(isPlayerTurn)
            {
                opponentOrbCount--;
            }
            else
            {
                playerOrbCount--;
            }

            RemoveAllOrbHighlights(-1);

            if (previousGamePhase != "placement" && (playerOrbCount == 2 || opponentOrbCount == 2)) //check for win
                GameOver();
            else //continue game
                ChangeSide();
        }
    }

    // Game Over //
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
    private bool CanFly()
    {
        if ((isPlayerTurn && playerOrbCount == 3) || (!isPlayerTurn && opponentOrbCount == 3))
            return true;

        return false;
    }

    private void ChangeSide()
    {
        isPlayerTurn = !isPlayerTurn;

        if(previousGamePhase == "placement")
        {
            previousGamePhase = gamePhase;
            if (placementPhase_RoundCount > startingNumberOfOrbs)
            {
                gamePhase = "movementPickup";
                PrepareForMovementPhase();
            }
            else
            {
                gamePhase = "placement";
            }
        }
        else
        {
            previousGamePhase = gamePhase;
            gamePhase = "movementPickup";
            RemoveAllOrbHighlights(-1);
            PrepareForMovementPhase();
        }
    }

    public bool ClickedOnDifferentPiece(short selectedRune)
    {
        if ((isPlayerTurn && runeList[selectedRune].tag == "Player") || (!isPlayerTurn && runeList[selectedRune].tag == "Opponent"))
            return true;
        return false;
    }

    private bool IsLegalMove(short toLocation)
    {
        if((isPlayerTurn && playerOrbCount == 3) || (!isPlayerTurn && opponentOrbCount == 3)) //can fly
        {
            if (runeList[toLocation])
                return true;
        }
        else if(dictionaries.adjacencyDictionary[runeFromLocation].Contains(toLocation))
        {
            return true;
        }

        return false;
    }

    private List<short> MakeListOfRunesForCurrentPlayer()
    {
        List<short> runes = new List<short>();

        foreach (RuneController rune in runeList)
            if ((isPlayerTurn && rune.tag == "Player") || (!isPlayerTurn && rune.tag == "Opponent"))
                runes.Add(rune.runeNumber);

        return runes;
    }

    private List<short> MakeListOfRunesThatCanBeRemoved()
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
                if (((isPlayerTurn && rune.tag == "Opponent") || (!isPlayerTurn && rune.tag == "Player")) && !rune.isInMill)
                    runes.Add(rune.runeNumber); 
        }

        runesThatCanBeRemoved.Clear();
        runesThatCanBeRemoved.AddRange(runes);

        return runes;
    }

    private bool RuneCanBeMoved(short selectedRune)
    {
        if (((isPlayerTurn && runeList[selectedRune].tag == "Player") ||
            (!isPlayerTurn && runeList[selectedRune].tag == "Opponent")) && runesThatCanBeMoved.Contains(selectedRune))
        {
            return true;
        }
        return false;
    }

    public bool RuneCanBeRemoved(short runeToRemove)
    {
        if (((isPlayerTurn && runeList[runeToRemove].tag == "Opponent") 
            || (!isPlayerTurn && runeList[runeToRemove].tag == "Player")) 
            && runesThatCanBeRemoved.Contains(runeToRemove))
        {
            return true;
        }
            return false;
    }

    private bool ThereIsAnAvailableMove(List<short> runes)
    {
        List<short> moveableRunes = new List<short>();
        bool canMakeAMove = false;

        if (runes.Count() > 2) //cannot fly
        {
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
            runesThatCanBeMoved.Clear();
            runesThatCanBeMoved.AddRange(moveableRunes);
        }
        else
        {
            foreach (short rune in runes)
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
        List<short> runes = new List<short>();

        foreach (RuneController rune in runeList)
            if ((isPlayerTurn && rune.tag == "Opponent") || (!isPlayerTurn && rune.tag == "Player"))
                runes.Add(rune.runeNumber);

        foreach (short rune in runes)
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
                if (mill.position1 == runeFromLocation || mill.position2 == runeFromLocation || mill.position3 == runeFromLocation)
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
            for (short i = 0; i < opponentMills.Count; i++)
            {
                    mill = opponentMills[i];
                if (mill.position1 == runeFromLocation || mill.position2 == runeFromLocation || mill.position3 == runeFromLocation)
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

    private void MoveOrb(short toLocation)
    {
        GameObject orbToMove;
        if(gamePhase == "placement")
        {
            orbToMove = isPlayerTurn ? GameObject.Find(playerColor + "_Orb_" + placementPhase_RoundCount) : GameObject.Find(opponentColor + "_Orb_" + placementPhase_RoundCount);
        }
        else
        {
            orbToMove = GameObject.Find("OrbAtLocation_" + runeFromLocation);
        }

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

    private void RemoveAllOrbHighlights(short runeNumber)
    {
        if(runeNumber == -1) // remove all
        {
            foreach (RuneController rune in runeList)
            {
                if(rune.tag != "Empty")
                    RemoveOrbHighlight(rune.runeNumber);
            }
        }
        else
        {
            List<short> runes = MakeListOfRunesForCurrentPlayer();

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

    // Runes //
    private void HighlightAvailableMoves(short rune)
    {
        if (CanFly())
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

    private void RemoveAllRuneHighlights()
    {
        for (int i = 0; i < runeList.Length; i++)
        {
            runeList[i].GetComponent<RuneController>().RemoveRuneHighlight();
        }
    }
}