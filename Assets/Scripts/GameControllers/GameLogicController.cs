using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class GameLogicController : Photon.PunBehaviour
{
    /*---------------------------------------------------------------------
    || NETWORKING VARIABLES
    -----------------------------------------------------------------------*/
    public InputField chatInputField;

    private bool isMasterClient;

    private string opponentName;

    private short network_moveTo;
    private short network_moveFrom;
    private short network_removeFrom;

    /*---------------------------------------------------------------------
    || GAME VARIABLES
    -----------------------------------------------------------------------*/
    public RuneController[] runeList;
    private Dictionaries dictionaries;

    private GameObject playerMage;
    private GameObject opponentMage;

    public string playerColor = "Green";
    public string opponentColor = "Purple";

    public string gamePhase; //placement, movementPickup, movementPlace, removal
    private string previousGamePhase;

    public bool isPlayerTurn;
    public bool preventClick;

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
        playerMage = GameObject.Find("GreenMage");
        opponentMage = GameObject.Find("PurpleMage");
        gamePhase = "placement";
        previousGamePhase = "placement";
        DetermineIfMasterClient();
        startingNumberOfOrbs = 4;
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
    || NETWORKING FUNCTIONS
    -----------------------------------------------------------------------*/
    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        // Scene 1 is the network lobby
        SceneManager.LoadScene(1);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // Not seen if you're the player connecting

        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // Called before OnPhotonPlayerDisconnected

            LoadArena();
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        // If one player disconnects, we disconnect the other player and alert them
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // Seen when other disconnects

        LeaveRoom();
    }

    public void DetermineIfMasterClient()
    {
        // The master client moves first
        isPlayerTurn = PhotonNetwork.isMasterClient;
        preventClick = !isPlayerTurn;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SendName()
    {
        //playerNameText.text = PhotonNetwork.playerName;
        photonView.RPC("ReceieveName", PhotonTargets.Others, PhotonNetwork.playerName);
    }

    public void SendMove()
    {
        photonView.RPC("ReceiveMove", PhotonTargets.Others, network_moveTo, network_moveFrom, network_removeFrom);
    }

    public void SendChat()
    {
        string sentMessage;
        sentMessage = chatInputField.text;
        sentMessage = sentMessage.Trim();
        if (sentMessage != "")
        {
            photonView.RPC("ReceiveChat", PhotonTargets.Others, sentMessage);
        }
    }

    private void LoadArena()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master Client");
        }

        Debug.Log("PhotonNetwork : Loading Mal'sBoard");
        PhotonNetwork.LoadLevel("Mal'sBoard");
    }

    [PunRPC]
    public void ReceiveName(string opponentName)
    {
        this.opponentName = opponentName;
        //opponentNameText.text = opponentName;
    }

    [PunRPC]
    public void ReceiveMove(short moveTo, short moveFrom, short removeFrom)
    {
        Debug.Log("Move to: " + moveTo);
        Debug.Log("Move from: " + moveFrom);
        Debug.Log("Remove from: " + removeFrom);

        if (moveTo != -1)
        {
            // Place opponent rune
            MoveOrb(moveTo);
        }
        if (moveFrom != -1)
        {
            runeFromLocation = moveFrom;
            MoveOrb(moveTo);
        }
        if (removeFrom != -1)
        {
            RemoveOrb(removeFrom);
        }

        // Give control back to this user
        preventClick = false;
    }

    [PunRPC]
    public void ReceiveChat(string receivedMessage)
    {
        Debug.Log("Opponent: " + receivedMessage);
        chatInputField.text = opponentName + ": " + receivedMessage;
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
                network_moveTo = rune;
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
            network_moveFrom = selectedRune;

            runeFromLocation = selectedRune;

            RemoveAllRuneHighlights();
            RemoveAllOrbHighlights(selectedRune);
            HighlightAvailableMoves(selectedRune);

            previousGamePhase = gamePhase;
            gamePhase = "movementPlace";
            preventClick = false;
        }
        else
        {
            preventClick = false;
        }
    }

    public void MovementPhase_Place(short toLocation)
    {
        if (runeList[toLocation].tag == "Empty")
        {
            if (IsLegalMove(toLocation))
            {
                network_moveTo = toLocation;

                RemoveOrbHighlight(runeFromLocation);

                MoveOrb(toLocation);

                runeList[toLocation].tag = (isPlayerTurn) ? "Player" : "Opponent";
                runeList[runeFromLocation].tag = "Empty";

                if (runeList[runeFromLocation].isInMill)
                {
                    RemoveRunesFromMill();
                }
            }
        }
        else if (ClickedOnDifferentPiece(toLocation)) //switch to highlighted piece
        {
            previousGamePhase = "movementPlace";
            MovementPhase_Pickup(toLocation);
        }
        else
        {
            preventClick = false;
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
            network_removeFrom = runeToRemove;

            if (runeList[runeToRemove].isInMill)
                RemoveRunesFromMill();

            RemoveOrb(runeToRemove);

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
        // Send move to opponent if in a network game
        // If in network game
        SendMove();

        preventClick = true;
        // Else
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

        if (runes.Count() > 3) //cannot fly
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
        }
        else
        {
            foreach (short rune in runes)
            {
                    if (!moveableRunes.Contains(rune))
                    {
                        moveableRunes.Add(rune);
                    }
                    canMakeAMove = true;
            }
        }
        runesThatCanBeMoved.Clear();
        runesThatCanBeMoved.AddRange(moveableRunes);
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
        Mill mill;
        if (isPlayerTurn)
        {
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

    private void InstantiateMagicRings(Mill mill)
    {
        string color = isPlayerTurn ? playerColor : opponentColor;

        Transform ringTransform = dictionaries.magicRingDictionary[color].transform;

        Transform t1 = runeList[mill.position1].transform;
        Transform t2 = runeList[mill.position2].transform;
        Transform t3 = runeList[mill.position3].transform;

        GameObject ring;
        ring = Instantiate(dictionaries.magicRingDictionary[color], new Vector3(t1.position.x, 0.2f, t1.position.z), ringTransform.rotation);
        ring.name = "RingAt_" + mill.position1;
        ring = Instantiate(dictionaries.magicRingDictionary[color], new Vector3(t2.position.x, 0.2f, t2.position.z), ringTransform.rotation);
        ring.name = "RingAt_" + mill.position2;
        ring = Instantiate(dictionaries.magicRingDictionary[color], new Vector3(t3.position.x, 0.2f, t3.position.z), ringTransform.rotation);
        ring.name = "RingAt_" + mill.position3;
    }

    private void DestroyMagicRings()
    {
        GameObject[] rings = GameObject.FindGameObjectsWithTag("MagicRing");
        foreach(GameObject ring in rings)
        {
            Destroy(ring);
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

                    Mill mill = new Mill(i, (short)(i + 1), (short)(i + 2));

                    InstantiateMagicRings(mill);

                    if (isPlayerTurn)
                        playerMills.Add(mill);
                    else
                        opponentMills.Add(mill);

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

                    InstantiateMagicRings(mill);

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

        if (isPlayerTurn)
            playerMage.GetComponent<MageController>().PlayAttack1Animation(GameObject.Find("Rune" + toLocation));
        else
            opponentMage.GetComponent<MageController>().PlayAttack1Animation(GameObject.Find("Rune" + toLocation));

        orbToMove.name = "OrbAtLocation_" + toLocation;
        RemoveAllRuneHighlights();

        LeanTween.delayedCall(orbToMove, 0.3f, () =>
        {
            LeanTween.move(orbToMove, dictionaries.orbPositionsDictionary[toLocation], 0.5f).setOnComplete(() => 
            {
                //this is the callback
                if (RuneIsInMill(toLocation))
                {
                    PrepareForRemovalPhase();
                }
                else
                {
                    ChangeSide();
                }
                
                preventClick = false;
            });
        });
    }

    private void RemoveOrb(short runeNumber)
    {
        Destroy(GameObject.Find("OrbAtLocation_" + runeNumber));
        runeList[runeNumber].tag = "Empty";

        if (isPlayerTurn)
        {
            opponentOrbCount--;
        }
        else
        {
            playerOrbCount--;
        }

        RemoveAllOrbHighlights(-1);
        DestroyMagicRings();
        preventClick = false;
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