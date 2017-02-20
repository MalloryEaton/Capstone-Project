using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameLogicController : MonoBehaviour
{
    /*
     player colors
     scene selected (slected by master client)
     game type (network, one player and AI, two players, story mode?)
     difficulty
     https://docs.unity3d.com/ScriptReference/PlayerPrefs.SetString.html
    */

    /*
        Notes:
            When mill is made and removable runes are highlighted, should not highlight runes in mills
    */

    /*---------------------------------------------------------------------
    || GAME VARIABLES
    -----------------------------------------------------------------------*/
    public RuneController[] runeList;
    private Dictionaries dictionaries;
    private NetworkingController networking;

    private GameObject player1Mage;
    private GameObject player2Mage;

    public string player1Color;
    public string player2Color;

    public string gamePhase; //placement, movementPickup, movementPlace, removal
    private string previousGamePhase;

    public bool isNetworkGame;
    public bool isPlayer1;

    public bool isPlayer1Turn;
    public bool preventClick;
    public bool waitingOnOtherPlayer;
    public bool waitingOnAnimation;

    private short startingNumberOfOrbs;
    private short player1OrbCount;
    private short player2OrbCount;
    private short placementPhase_RoundCount;

    private List<Mill> player1Mills;
    private List<Mill> player2Mills;

    public short runeFromLocation;
    private List<short> runesThatCanBeMoved;
    private List<short> runesThatCanBeRemoved;

    private AudioSource moveSound;
    private AudioSource removeSound;

    public GameObject LoadingScreen;

    void Awake()
    {
        LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", true);
    }

    void Start()
    {
        AudioSource[] audio = GetComponents<AudioSource>();
        moveSound = audio[0];
        removeSound = audio[1];

        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        networking = FindObjectOfType(typeof(NetworkingController)) as NetworkingController;
        gamePhase = "placement";
        previousGamePhase = "placement";
        waitingOnAnimation = false;
        waitingOnOtherPlayer = false;
        isPlayer1Turn = true;

        startingNumberOfOrbs = 9;
        player1OrbCount = 0;
        player2OrbCount = 0;
        placementPhase_RoundCount = 1;
        player1Mills = new List<Mill>();
        player2Mills = new List<Mill>();
        runesThatCanBeMoved = new List<short>();
        runesThatCanBeRemoved = new List<short>();
        
        isNetworkGame = PlayerPrefs.GetString("GameType") == "Network" ? true : false;
        //isNetworkGame = false;

        if (isNetworkGame)
        {
            networking.SendColor();

            LeanTween.delayedCall(gameObject, 5f, () => {
                isPlayer1 = networking.DetermineIfMasterClient();
                waitingOnOtherPlayer = !isPlayer1; //prevent player 2 from clicking
                
                if(isPlayer1)
                {
                    player1Color = PlayerPrefs.GetString("PlayerColor");
                    player2Color = networking.otherPlayerColor;
                }
                else
                {
                    player1Color = networking.otherPlayerColor;
                    player2Color = PlayerPrefs.GetString("PlayerColor");
                }
                
                networking.ResetNetworkValues();

                print(player1Color + "  " + player2Color);

                LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);

                InitializeGameBoard();
            });
        }
        else
        {
            //player1Color = "Green";
            //player2Color = "Purple";
            player1Color = PlayerPrefs.GetString("Player1Color");
            player2Color = PlayerPrefs.GetString("Player2Color");
            InitializeGameBoard();
        }
    }
    
    public void ShowAvailableMoves()
    {
        RemoveAllRuneHighlights();
        RemoveAllOrbHighlights(-1);
        HighlightMoveableOrbs(runesThatCanBeMoved);
    }

    /*---------------------------------------------------------------------
    || INITIALIZATION
    -----------------------------------------------------------------------*/
    private void InitializeGameBoard()
    {
        InstantiateMages();
        InstantiateShrine();
        InstantiateOrbContainers();

        // preventClick = false;
    }

    private void InstantiateMages()
    {
        if(!isNetworkGame || (isNetworkGame && isPlayer1))
        {
            player1Mage = Instantiate(dictionaries.magesDictionary[player1Color], new Vector3(20, 1, 28), new Quaternion(0, 180, 0, 0));
            player1Mage.tag = "Mage";

            player2Mage = Instantiate(dictionaries.magesDictionary[player2Color], new Vector3(4, 1, -4), new Quaternion(0, 0, 0, 0));
            player2Mage.tag = "Mage";
        }
        else if(isNetworkGame && !isPlayer1)
        {
            player1Mage = Instantiate(dictionaries.magesDictionary[player1Color], new Vector3(4, 1, -4), new Quaternion(0, 0, 0, 0));
            player1Mage.tag = "Mage";

            player2Mage = Instantiate(dictionaries.magesDictionary[player2Color], new Vector3(20, 1, 28), new Quaternion(0, 180, 0, 0));
            player2Mage.tag = "Mage";
        }
    }

    private void InstantiateShrine()
    {
        Instantiate(dictionaries.shrinesDictionary[player1Color], new Vector3(12, 0, 12), Quaternion.identity);
    }

    private void InstantiateOrbContainers()
    {
        if (!isNetworkGame || (isNetworkGame && isPlayer1))
        {
            Instantiate(dictionaries.orbContainersDictionary[player1Color], new Vector3(0, 0, 28), Quaternion.identity);
            Instantiate(dictionaries.orbContainersDictionary[player2Color], new Vector3(8, 0, -4), Quaternion.identity);
        }
        else if(isNetworkGame && !isPlayer1)
        {
            Instantiate(dictionaries.orbContainersDictionary[player1Color], new Vector3(8, 0, -4), Quaternion.identity);
            Instantiate(dictionaries.orbContainersDictionary[player2Color], new Vector3(0, 0, 28), Quaternion.identity);
        }  
    }

    /*---------------------------------------------------------------------
    || GAME PHASE FUNCTIONS
    -----------------------------------------------------------------------*/
    // Placement //
    public void PlacementPhase(short rune)
    {
        if (runeList[rune].tag == "Empty")
        {
            networking.moveTo = rune;
            MoveOrb(rune);
            // NEED TO CHANGE TAGS TO PLAYER1 AND PLAYER2
            if (isPlayer1Turn)
            {
                runeList[rune].tag = "Player";
                player1OrbCount++;
            }
            else
            {
                runeList[rune].tag = "Opponent";
                player2OrbCount++;
                // Round count will always increment after second player's turn
                placementPhase_RoundCount++;
            }

            RemoveAllRuneHighlights();

            previousGamePhase = "placement";
        }
    }

    // Movement //
    private void PrepareForMovementPhase()
    {
        //only do this if it is your move
        if(!isNetworkGame || (isNetworkGame && ((isPlayer1 && isPlayer1Turn) || (!isPlayer1 && !isPlayer1Turn))))
        {
            if (ThereIsAnAvailableMove(MakeListOfRunesForCurrentPlayer()))
            {
                HighlightMoveableOrbs(runesThatCanBeMoved);
            }
            else
            {
                print("NO AVAILABLE MOVES! YOU LOSE!");
                isPlayer1Turn = !isPlayer1Turn;
                GameOver();
            }
        }
    }

    public void MovementPhase_Pickup(short selectedRune)
    {
        if (RuneCanBeMoved(selectedRune))
        {
            networking.moveFrom = selectedRune;
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
                networking.moveTo = toLocation;

                RemoveOrbHighlight(runeFromLocation);

                MoveOrb(toLocation);

                runeList[toLocation].tag = (isPlayer1Turn) ? "Player" : "Opponent";
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
            ShowAvailableMoves();
        }
    }

    // Removal //
    private void PrepareForRemovalPhase()
    {
        print("YOU GOT A MILL!");
        preventClick = false;
        previousGamePhase = gamePhase;
        gamePhase = "removal";
        HighlightMoveableOrbs(MakeListOfRunesThatCanBeRemoved());
    }

    public void RemovalPhase(short runeToRemove)
    {
        if (RuneCanBeRemoved(runeToRemove) || ((isPlayer1Turn && !isPlayer1) || (!isPlayer1Turn && isPlayer1)))
        {
            networking.removeFrom = runeToRemove;

            if (runeList[runeToRemove].isInMill)
                RemoveRunesFromMill();

            RemoveOrb(runeToRemove);
        }
    }

    // Game Over //
    private void GameOver()
    {
        if (isNetworkGame && ((isPlayer1Turn && isPlayer1) || (!isPlayer1Turn && !isPlayer1)))
        {
            networking.SendMove();
        }

        waitingOnAnimation = true; //prevent clicking

        RemoveAllOrbHighlights(-1);
        RemoveAllRuneHighlights();

        if (isPlayer1Turn)
            print("Game Over. " + player1Color + " wins!");
        else
            print("Game Over. " + player2Color + " wins!");
    }


    /*---------------------------------------------------------------------
    || GAME PHASE LOGIC
    -----------------------------------------------------------------------*/
    private bool CanFly()
    {
        if ((isPlayer1Turn && player1OrbCount == 3) || (!isPlayer1Turn && player2OrbCount == 3))
            return true;

        return false;
    }

    private void ChangeSide()
    {
        // Send move to opponent if in a network game
        if (isNetworkGame && ((isPlayer1Turn && isPlayer1) || (!isPlayer1Turn && !isPlayer1)))
        {
            Debug.Log("We're sending a move.");
            networking.SendMove();
            waitingOnOtherPlayer = true;
        }
        else if (isNetworkGame && ((isPlayer1Turn && !isPlayer1) || (!isPlayer1Turn && isPlayer1)))
        {
            waitingOnOtherPlayer = false;
        }

        isPlayer1Turn = !isPlayer1Turn;
        networking.ResetNetworkValues();

        if (previousGamePhase == "placement")
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
        if ((isPlayer1Turn && runeList[selectedRune].tag == "Player") || (!isPlayer1Turn && runeList[selectedRune].tag == "Opponent"))
            return true;
        return false;
    }

    private bool IsLegalMove(short toLocation)
    {
        if ((isPlayer1Turn && player1OrbCount == 3) || (!isPlayer1Turn && player2OrbCount == 3)) //can fly
        {
            if (runeList[toLocation])
                return true;
        }
        else if (dictionaries.adjacencyDictionary[runeFromLocation].Contains(toLocation))
        {
            return true;
        }

        return false;
    }

    private List<short> MakeListOfRunesForCurrentPlayer()
    {
        List<short> runes = new List<short>();

        foreach (RuneController rune in runeList)
            if ((isPlayer1Turn && rune.tag == "Player") || (!isPlayer1Turn && rune.tag == "Opponent"))
                runes.Add(rune.runeNumber);

        return runes;
    }

    private List<short> MakeListOfRunesThatCanBeRemoved()
    {
        List<short> runes = new List<short>();

        if (AllRunesAreInMills())
        {
            foreach (RuneController rune in runeList)
                if ((isPlayer1Turn && rune.tag == "Opponent") || (!isPlayer1Turn && rune.tag == "Player"))
                    runes.Add(rune.runeNumber);
        }
        else //only add runes that are not in mills
        {
            foreach (RuneController rune in runeList)
                if (((isPlayer1Turn && rune.tag == "Opponent") || (!isPlayer1Turn && rune.tag == "Player")) && !rune.isInMill)
                    runes.Add(rune.runeNumber);
        }

        runesThatCanBeRemoved.Clear();
        runesThatCanBeRemoved.AddRange(runes);

        return runes;
    }

    private bool RuneCanBeMoved(short selectedRune)
    {
        if (((isPlayer1Turn && runeList[selectedRune].tag == "Player") ||
            (!isPlayer1Turn && runeList[selectedRune].tag == "Opponent")) && runesThatCanBeMoved.Contains(selectedRune))
        {
            return true;
        }
        return false;
    }

    public bool RuneCanBeRemoved(short runeToRemove)
    {
        if (((isPlayer1Turn && runeList[runeToRemove].tag == "Opponent")
            || (!isPlayer1Turn && runeList[runeToRemove].tag == "Player"))
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
            if ((isPlayer1Turn && rune.tag == "Opponent") || (!isPlayer1Turn && rune.tag == "Player"))
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
        if (isPlayer1Turn)
        {
            for (short i = 0; i < player1Mills.Count; i++)
            {
                mill = player1Mills[i];
                if (mill.position1 == runeFromLocation || mill.position2 == runeFromLocation || mill.position3 == runeFromLocation)
                {
                    runeList[mill.position1].isInMill = false;
                    runeList[mill.position2].isInMill = false;
                    runeList[mill.position3].isInMill = false;
                    player1Mills.Remove(player1Mills[i]);
                }
            }
        }
        else
        {
            for (short i = 0; i < player2Mills.Count; i++)
            {
                mill = player2Mills[i];
                if (mill.position1 == runeFromLocation || mill.position2 == runeFromLocation || mill.position3 == runeFromLocation)
                {
                    runeList[mill.position1].isInMill = false;
                    runeList[mill.position2].isInMill = false;
                    runeList[mill.position3].isInMill = false;
                    player2Mills.Remove(mill);
                }
            }
        }
    }

    private void InstantiateMagicRings(Mill mill)
    {
        string color = isPlayer1Turn ? player1Color : player2Color;

        Transform ringTransform = dictionaries.magicRingDictionary[color].transform;

        Transform t1 = runeList[mill.position1].transform;
        Transform t2 = runeList[mill.position2].transform;
        Transform t3 = runeList[mill.position3].transform;

        Instantiate(dictionaries.magicRingDictionary[color], new Vector3(t1.position.x, 0.2f, t1.position.z), ringTransform.rotation);
        Instantiate(dictionaries.magicRingDictionary[color], new Vector3(t2.position.x, 0.2f, t2.position.z), ringTransform.rotation);
        Instantiate(dictionaries.magicRingDictionary[color], new Vector3(t3.position.x, 0.2f, t3.position.z), ringTransform.rotation);
    }

    private void DestroyMagicRings()
    {
        GameObject[] rings = GameObject.FindGameObjectsWithTag("MagicRing");
        foreach (GameObject ring in rings)
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
                if (((rune == i || rune == (i + 1) || rune == (i + 2)) && isPlayer1Turn && runeList[rune].tag == "Player") ||
                    ((rune == i || rune == (i + 1) || rune == (i + 2)) && !isPlayer1Turn && runeList[rune].tag == "Opponent"))
                {
                    runeList[i].isInMill = true;
                    runeList[i + 1].isInMill = true;
                    runeList[i + 2].isInMill = true;

                    Mill mill = new Mill(i, (short)(i + 1), (short)(i + 2));

                    InstantiateMagicRings(mill);

                    if (isPlayer1Turn)
                        player1Mills.Add(mill);
                    else
                        player2Mills.Add(mill);

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
                if (((rune == mill.position1 || rune == mill.position2 || rune == mill.position3) && isPlayer1Turn && runeList[rune].tag == "Player") ||
                    ((rune == mill.position1 || rune == mill.position2 || rune == mill.position3) && !isPlayer1Turn && runeList[rune].tag == "Opponent"))
                {
                    runeList[mill.position1].isInMill = true;
                    runeList[mill.position2].isInMill = true;
                    runeList[mill.position3].isInMill = true;

                    InstantiateMagicRings(mill);

                    if (isPlayer1Turn)
                        player1Mills.Add(new Mill(mill.position1, mill.position2, mill.position3));
                    else
                        player2Mills.Add(new Mill(mill.position1, mill.position2, mill.position3));

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
        orb.AddComponent<OrbHoverController>();
    }

    public void MoveOrb(short toLocation)
    {
        waitingOnAnimation = true;
        GameObject orbToMove;
        if (gamePhase == "placement")
        {
            orbToMove = isPlayer1Turn ? GameObject.Find(player1Color + "_Orb_" + placementPhase_RoundCount) : GameObject.Find(player2Color + "_Orb_" + placementPhase_RoundCount);
        }
        else
        {
            orbToMove = GameObject.Find("OrbAtLocation_" + runeFromLocation);
        }

        if (isPlayer1Turn)
            player1Mage.GetComponent<MageController>().PlayAttack1Animation(GameObject.Find("Rune" + toLocation));
        else
            player2Mage.GetComponent<MageController>().PlayAttack1Animation(GameObject.Find("Rune" + toLocation));

        orbToMove.name = "OrbAtLocation_" + toLocation;
        RemoveAllRuneHighlights();

        moveSound.Play();

        LeanTween.delayedCall(orbToMove, 0.3f, () =>
        {
            LeanTween.move(orbToMove, dictionaries.orbPositionsDictionary[toLocation], 0.5f).setOnComplete(() =>
            {
                if (isNetworkGame)
                {
                    // Receiving player should immediately go to ChangeSide()
                    if (((isPlayer1Turn && isPlayer1) || (!isPlayer1Turn && !isPlayer1)) && RuneIsInMill(toLocation))
                    {
                        PrepareForRemovalPhase();
                    }
                    else
                    {
                        // If there is an orb to remove on the receiving side, however,
                        // we don't want to call ChangeSide() quite yet.
                        if (((isPlayer1Turn && !isPlayer1) || (!isPlayer1Turn && isPlayer1)) && (networking.removeFrom != -1))
                        {
                            RemovalPhase(networking.removeFrom);
                        }
                        else
                        {
                            ChangeSide();
                        }
                    }
                }
                else
                {
                    if (RuneIsInMill(toLocation))
                    {
                        PrepareForRemovalPhase();
                    }
                    else
                    {
                        ChangeSide();
                    }
                }
                waitingOnAnimation = false;
            });
        });
    }

    public void RemoveOrb(short runeNumber)
    {
        if (isPlayer1Turn)
            player1Mage.GetComponent<MageController>().PlayAttack1Animation(GameObject.Find("Rune" + runeNumber));
        else
            player2Mage.GetComponent<MageController>().PlayAttack1Animation(GameObject.Find("Rune" + runeNumber));
        
        LeanTween.delayedCall(gameObject, 0.6f, () =>
        {
            removeSound.Play();
            GameObject orbToDestroy = GameObject.Find("OrbAtLocation_" + runeNumber);
            GameObject hit;
            if (isPlayer1Turn)
            {
                hit = Instantiate(dictionaries.magicHitDictionary[player1Color],
                    new Vector3(orbToDestroy.transform.position.x, 0.2f, orbToDestroy.transform.position.z), Quaternion.identity);
            }
            else
            {
                hit = Instantiate(dictionaries.magicHitDictionary[player2Color],
                    new Vector3(orbToDestroy.transform.position.x, 0.2f, orbToDestroy.transform.position.z), Quaternion.identity);
            }

            LeanTween.delayedCall(gameObject, 0.1f, () => {
                Destroy(orbToDestroy);
            });

            LeanTween.delayedCall(gameObject, 1f, () => {
                Destroy(hit);
            });

            runeList[runeNumber].tag = "Empty";

            if (isPlayer1Turn)
            {
                player2OrbCount--;
            }
            else
            {
                player1OrbCount--;
            }

            RemoveAllOrbHighlights(-1);
            DestroyMagicRings();

            if (previousGamePhase != "placement" && (player1OrbCount == 2 || player2OrbCount == 2)) //check for win
                GameOver();
            else //continue game
                ChangeSide();
        });
            
    }

    private void RemoveAllOrbHighlights(short runeNumber)
    {
        if (runeNumber == -1) // remove all
        {
            foreach (RuneController rune in runeList)
            {
                if (rune.tag != "Empty")
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
                if (!selectedOrb.GetComponent<OrbHoverController>())
                {
                    selectedOrb.AddComponent<OrbHoverController>();
                }
            }
        }
    }

    private void RemoveOrbHighlight(short rune)
    {
        GameObject orb = GameObject.Find("OrbAtLocation_" + rune);
        Destroy(orb.GetComponent<OrbHoverController>());
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