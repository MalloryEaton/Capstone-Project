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
     scene selected (slected by master client)
     game type (network, one player and AI, two players, story mode?)
     difficulty
     https://docs.unity3d.com/ScriptReference/PlayerPrefs.SetString.html
    */

    /*---------------------------------------------------------------------
    || GAME VARIABLES
    -----------------------------------------------------------------------*/
    public RuneController[] runeList;
    public Dictionaries dictionaries;
    private NetworkingController networking;
    private MyAIController aicontroller;
    public GameBoardUIController uiController;

    private GameObject centerOfBoard;

    private GameObject player1Mage;
    private GameObject player2Mage;

    public string player1Color;
    public string player2Color;
    public string playerForfeit;

    public string gamePhase; //placement, movementPickup, movementPlace, removal
    private string previousGamePhase;

    private short drawCount;
    private bool canOfferDraw;

    public bool isAIGame;
    public bool isAITurn;
    public bool isNetworkGame;
    public bool isPlayer1;
    public string AIDifficulty;

    public bool isPlayer1Turn;
    public bool waitingOnOtherPlayer;
    public bool waitingOnAnimation;
    public bool menuIsOpen;

    private short startingNumberOfOrbs;
    private short player1OrbCount;
    private short player2OrbCount;
    private short placementPhase_RoundCount;

    private List<Mill> player1Mills;
    private List<Mill> player2Mills;

    public short runeFromLocation;
    private List<short> runesThatCanBeMoved;
    private List<short> runesThatCanBeRemoved;

    private List<short> AIMove;

    public GameObject LoadingScreen;
    public Button TextBox;

    private AudioSource moveSound;
    private AudioSource removeSound;
    private AudioSource millSound;
    private AudioSource summonSound;
    public AudioSource waterSound;
    public GameObject music;

    public Slider sfxSlider;
    public Slider musicSlider;

    private float speed = 0.3f;

    public bool showHints;

    void Awake()
    {
        LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", true);

        TextBox.gameObject.SetActive(false);
    }

    void Start()
    {
        centerOfBoard = GameObject.Find("CenterOfBoard");
        menuIsOpen = false;
        AudioSource[] audio = GetComponents<AudioSource>();
        moveSound = audio[0];
        removeSound = audio[1];
        millSound = audio[2];
        summonSound = audio[3];

        moveSound.volume = sfxSlider.value;
        removeSound.volume = sfxSlider.value;
        music.GetComponent<AudioSource>().volume = musicSlider.value;

        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        networking = FindObjectOfType(typeof(NetworkingController)) as NetworkingController;
        aicontroller = FindObjectOfType(typeof(MyAIController)) as MyAIController;
        gamePhase = "placement";
        previousGamePhase = "placement";
        waitingOnAnimation = false;
        waitingOnOtherPlayer = false;
        isPlayer1Turn = true;
        AIMove = new List<short> { -1, -1, -1 };

        startingNumberOfOrbs = 9;
        player1OrbCount = 0;
        player2OrbCount = 0;
        placementPhase_RoundCount = 1;
        player1Mills = new List<Mill>();
        player2Mills = new List<Mill>();
        runesThatCanBeMoved = new List<short>();
        runesThatCanBeRemoved = new List<short>();

        showHints = true;

        isNetworkGame = false;
        isAIGame = false;

        drawCount = 0;
        canOfferDraw = true;

        //PlayerPrefs.DeleteAll();

        Debug.Log(PlayerPrefs.GetString("GameType").ToString());

        if (PlayerPrefs.GetString("GameType") == "Network")
        {
            isNetworkGame = true;
            Debug.Log("This is a network game.");
        }
        else if (PlayerPrefs.GetString("GameType") == "AI" || PlayerPrefs.GetString("GameType") == "Story")
        {
            if (PlayerPrefs.GetString("GameType") == "Story")
            {
                if (PlayerPrefs.GetInt("StoryStage") == 0 || PlayerPrefs.GetInt("StoryStage") == 2 ||
                    PlayerPrefs.GetInt("StoryStage") == 4)
                {
                    PlayerPrefs.SetString("AIGoesFirst", "false");
                }
                else
                {
                    PlayerPrefs.SetString("AIGoesFirst", "true");
                }
            }

            isAIGame = true;
            isAITurn = false;
            AIDifficulty = PlayerPrefs.GetString("Difficulty");
            canOfferDraw = false;
        }

        if (isNetworkGame)
        {
            networking.SendColor();
            networking.SendName();

            LeanTween.delayedCall(gameObject, 5f, () =>
            {
                isPlayer1 = networking.DetermineIfMasterClient();
                waitingOnOtherPlayer = !isPlayer1; //prevent player 2 from clicking

                if (isPlayer1)
                {
                    player1Color = PlayerPrefs.GetString("PlayerColor");
                    if (networking.otherPlayerColor == null || networking.otherPlayerColor == player1Color)
                        PickRandomColor(2);
                    else
                        player2Color = networking.otherPlayerColor;
                }
                else
                {
                    player2Color = PlayerPrefs.GetString("PlayerColor");
                    if (networking.otherPlayerColor == null || networking.otherPlayerColor == player2Color)
                        PickRandomColor(1);
                    else
                        player1Color = networking.otherPlayerColor;
                }

                networking.ResetNetworkValues();

                print(player1Color + "  " + player2Color);
                InitializeGameBoard();

                LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);
                Destroy(GameObject.FindGameObjectWithTag("BlackPanel"));

                PlayMageIntroAnimations();
            });
        }
        else
        {
            player1Color = PlayerPrefs.GetString("Player1Color");

            if (isAIGame) // have the user select the opponent's color as well
            {
                // TODO: Set a player preference that determines who is going first -
                // the player or the AI.
                isPlayer1 = true;
                if (PlayerPrefs.GetString("GameType") == "AI")
                    PickRandomColor(2);
                else
                    player2Color = PlayerPrefs.GetString("Player2Color");
            }
            else
            {
                player2Color = PlayerPrefs.GetString("Player2Color");
            }

            LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);
            Destroy(GameObject.FindGameObjectWithTag("BlackPanel"));

            InitializeGameBoard();

            PlayMageIntroAnimations();
        }
    }

    public void sfxVolumeUpdate()
    {
        moveSound.volume = sfxSlider.value;
        removeSound.volume = sfxSlider.value;
        millSound.volume = sfxSlider.value;
        if (SceneManager.GetActiveScene().name == "WaterGameBoard")
            waterSound.volume = sfxSlider.value;
    }

    public void musicVolumeUpdate()
    {
        music.GetComponent<AudioSource>().volume = musicSlider.value;
    }

    private void InstantiateSide1Orbs(string color)
    {
        GameObject orb;
        orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
        orb.AddComponent<OrbIntro>();
        orb.name = color + "_Orb_" + 1;
        orb.GetComponent<OrbIntro>().FloatUp(1, color);
        LeanTween.delayedCall(speed, () =>
        {
            orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
            orb.AddComponent<OrbIntro>();
            orb.name = color + "_Orb_" + 2;
            orb.GetComponent<OrbIntro>().FloatUp(2, color);

            LeanTween.delayedCall(speed, () =>
            {
                orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
                orb.AddComponent<OrbIntro>();
                orb.name = color + "_Orb_" + 3;
                orb.GetComponent<OrbIntro>().FloatUp(3, color);

                LeanTween.delayedCall(speed, () =>
                {
                    orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
                    orb.AddComponent<OrbIntro>();
                    orb.name = color + "_Orb_" + 4;
                    orb.GetComponent<OrbIntro>().FloatUp(4, color);

                    LeanTween.delayedCall(speed, () =>
                    {
                        orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
                        orb.AddComponent<OrbIntro>();
                        orb.name = color + "_Orb_" + 5;
                        orb.GetComponent<OrbIntro>().FloatUp(5, color);

                        LeanTween.delayedCall(speed, () =>
                        {
                            orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
                            orb.AddComponent<OrbIntro>();
                            orb.name = color + "_Orb_" + 6;
                            orb.GetComponent<OrbIntro>().FloatUp(6, color);

                            LeanTween.delayedCall(speed, () =>
                            {
                                orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
                                orb.AddComponent<OrbIntro>();
                                orb.name = color + "_Orb_" + 7;
                                orb.GetComponent<OrbIntro>().FloatUp(7, color);

                                LeanTween.delayedCall(speed, () =>
                                {
                                    orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
                                    orb.AddComponent<OrbIntro>();
                                    orb.name = color + "_Orb_" + 8;
                                    orb.GetComponent<OrbIntro>().FloatUp(8, color);

                                    LeanTween.delayedCall(speed, () =>
                                    {
                                        orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(22.75f, 10f, 27.25f), Quaternion.identity);
                                        orb.AddComponent<OrbIntro>();
                                        orb.name = color + "_Orb_" + 9;
                                        orb.GetComponent<OrbIntro>().FloatUp(9, color);
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    private void InstantiateSide2Orbs(string color)
    {
        GameObject orb;
        orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
        orb.AddComponent<OrbIntro>();
        orb.name = color + "_Orb_" + 1;
        orb.GetComponent<OrbIntro>().FloatUp(1, color);
        LeanTween.delayedCall(speed, () =>
        {
            orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
            orb.AddComponent<OrbIntro>();
            orb.name = color + "_Orb_" + 2;
            orb.GetComponent<OrbIntro>().FloatUp(2, color);

            LeanTween.delayedCall(speed, () =>
            {
                orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
                orb.AddComponent<OrbIntro>();
                orb.name = color + "_Orb_" + 3;
                orb.GetComponent<OrbIntro>().FloatUp(3, color);

                LeanTween.delayedCall(speed, () =>
                {
                    orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
                    orb.AddComponent<OrbIntro>();
                    orb.name = color + "_Orb_" + 4;
                    orb.GetComponent<OrbIntro>().FloatUp(4, color);

                    LeanTween.delayedCall(speed, () =>
                    {
                        orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
                        orb.AddComponent<OrbIntro>();
                        orb.name = color + "_Orb_" + 5;
                        orb.GetComponent<OrbIntro>().FloatUp(5, color);

                        LeanTween.delayedCall(speed, () =>
                        {
                            orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
                            orb.AddComponent<OrbIntro>();
                            orb.name = color + "_Orb_" + 6;
                            orb.GetComponent<OrbIntro>().FloatUp(6, color);

                            LeanTween.delayedCall(speed, () =>
                            {
                                orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
                                orb.AddComponent<OrbIntro>();
                                orb.name = color + "_Orb_" + 7;
                                orb.GetComponent<OrbIntro>().FloatUp(7, color);

                                LeanTween.delayedCall(speed, () =>
                                {
                                    orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
                                    orb.AddComponent<OrbIntro>();
                                    orb.name = color + "_Orb_" + 8;
                                    orb.GetComponent<OrbIntro>().FloatUp(8, color);

                                    LeanTween.delayedCall(speed, () =>
                                    {
                                        orb = Instantiate(dictionaries.orbsDictionary[color], new Vector3(1.25f, 10f, -3.25f), Quaternion.identity);
                                        orb.AddComponent<OrbIntro>();
                                        orb.name = color + "_Orb_" + 9;
                                        orb.GetComponent<OrbIntro>().FloatUp(9, color);
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    private void PlayMageIntroAnimations()
    {
        waitingOnAnimation = true;
        player1Mage.GetComponent<MageController>().PlayLevitateAnimation();
        player2Mage.GetComponent<MageController>().PlayLevitateAnimation();
        if (isNetworkGame && !isPlayer1)
        {
            LeanTween.delayedCall(0.7f, () =>
            {
                summonSound.Play();
                InstantiateSide1Orbs(player2Color);
                InstantiateSide2Orbs(player1Color);
            });
        }
        else
        {
            LeanTween.delayedCall(0.7f, () =>
            {
                summonSound.Play();
                InstantiateSide1Orbs(player1Color);
                InstantiateSide2Orbs(player2Color);
            });
        }

        LeanTween.delayedCall(4f, () =>
        {
            player1Mage.GetComponent<MageController>().PlayLandingAnimation();
            player2Mage.GetComponent<MageController>().PlayLandingAnimation();

            if (isAIGame && PlayerPrefs.GetString("AIGoesFirst") == "true")
                ChangeSide();

            waitingOnAnimation = false;
        });
    }

    // We don't want the AI (or other network player) to be the same color as
    // the user.
    private void PickRandomColor(int player)
    {
        do
        {
            short randomOpponentColor;
            randomOpponentColor = (short)UnityEngine.Random.Range(0, 7);

            switch (randomOpponentColor)
            {
                case 0:
                    if (player == 1)
                        player1Color = "Black";
                    else
                        player2Color = "Black";
                    break;
                case 1:
                    if (player == 1)
                        player1Color = "Blue";
                    else
                        player2Color = "Blue";
                    break;
                case 2:
                    if (player == 1)
                        player1Color = "Green";
                    else
                        player2Color = "Green";
                    break;
                case 3:
                    if (player == 1)
                        player1Color = "Orange";
                    else
                        player2Color = "Orange";
                    break;
                case 4:
                    if (player == 1)
                        player1Color = "Purple";
                    else
                        player2Color = "Purple";
                    break;
                case 5:
                    if (player == 1)
                        player1Color = "Red";
                    else
                        player2Color = "Red";
                    break;
                case 6:
                    if (player == 1)
                        player1Color = "White";
                    else
                        player2Color = "White";
                    break;
                case 7:
                    if (player == 1)
                        player1Color = "Yellow";
                    else
                        player2Color = "Yellow";
                    break;
            }
        } while (player1Color == player2Color);
    }

    /* make it so that if click off a selected rune, shows the available
     * moves again
     */
    public void ShowAvailableMoves()
    {
        RemoveAllRuneHighlights();
        RemoveAllOrbHighlights(-1);
        HighlightMoveableOrbs(runesThatCanBeMoved);
    }

    /*-----------------------------------------------------------------
    || INITIALIZATION
    -----------------------------------------------------------------*/
    private void InitializeGameBoard()
    {
        InstantiateMages();
        InstantiateShrine();
    }

    private void InstantiateMages()
    {
        if (!isNetworkGame || (isNetworkGame && isPlayer1))
        {
            player1Mage = Instantiate(dictionaries.magesDictionary[player1Color],
                                      new Vector3(20, 1, 29), new Quaternion(0, 180, 0, 0));
            player1Mage.tag = "Mage";

            player2Mage = Instantiate(dictionaries.magesDictionary[player2Color],
                                      new Vector3(4, 1, -4), new Quaternion(0, 0, 0, 0));
            player2Mage.tag = "Mage";
        }
        else if (isNetworkGame && !isPlayer1)
        {
            player1Mage = Instantiate(dictionaries.magesDictionary[player1Color],
                                      new Vector3(4, 1, -4), new Quaternion(0, 0, 0, 0));
            player1Mage.tag = "Mage";

            player2Mage = Instantiate(dictionaries.magesDictionary[player2Color],
                                      new Vector3(20, 1, 29), new Quaternion(0, 180, 0, 0));
            player2Mage.tag = "Mage";
        }
    }

    private void InstantiateShrine()
    {
        if(isAIGame && PlayerPrefs.GetString("AIGoesFirst") == "true")
            Instantiate(dictionaries.shrinesDictionary[player2Color], new Vector3(12, 0, 12), Quaternion.identity);
        else
            Instantiate(dictionaries.shrinesDictionary[player1Color], new Vector3(12, 0, 12), Quaternion.identity);
    }

    /*-----------------------------------------------------------------
    || GAME PHASE FUNCTIONS
    -----------------------------------------------------------------*/
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
                if (isAIGame)
                {
                    if (PlayerPrefs.GetString("AIGoesFirst") == "true")
                    {
                        placementPhase_RoundCount++;
                    }
                }
            }
            else
            {
                runeList[rune].tag = "Opponent";
                player2OrbCount++;
                // Round count will always increment after second player's turn
                if (PlayerPrefs.GetString("AIGoesFirst") == "false" ||
                    isNetworkGame || (!isNetworkGame && !isAIGame))
                {
                    placementPhase_RoundCount++;
                }
            }

            RemoveAllRuneHighlights();

            previousGamePhase = "placement";
        }
    }

    // Movement //
    private void PrepareForMovementPhase()
    {
        //only do this if it is your move
        if (!isNetworkGame || (isNetworkGame &&
            ((isPlayer1 && isPlayer1Turn) || (!isPlayer1 && !isPlayer1Turn))))
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
            if (IsLegalMove(toLocation) || (isAIGame && isAITurn))
            {
                networking.moveTo = toLocation;

                // Do not call this if it is the AI's turn
                if (isAIGame && !isAITurn)
                    RemoveOrbHighlight(runeFromLocation);
                else if (!isAIGame)
                    RemoveOrbHighlight(runeFromLocation);

                MoveOrb(toLocation);

                drawCount++;

                runeList[toLocation].tag = (isPlayer1Turn) ? "Player" : "Opponent";
                runeList[runeFromLocation].tag = "Empty";

                if (runeList[runeFromLocation].isInMill)
                {
                    RemoveRunesFromMill();
                }
            }
        }
        //switch to highlighted piece
        else if (ClickedOnDifferentPiece(toLocation))
        {
            previousGamePhase = "movementPlace";
            MovementPhase_Pickup(toLocation);
        }
        else if ((isPlayer1Turn && runeList[toLocation].tag != "Player") ||
                 (!isPlayer1Turn && runeList[toLocation].tag != "Opponent"))
        {
            //nothing
        }
        else
        {
            ShowAvailableMoves();
        }
    }

    // Removal //
    private void PrepareForRemovalPhase()
    {
        millSound.Play();
        print("YOU GOT A MILL!");
        previousGamePhase = gamePhase;
        gamePhase = "removal";
        HighlightMoveableOrbs(MakeListOfRunesThatCanBeRemoved());
    }

    public void RemovalPhase(short runeToRemove)
    {
        if ((isAIGame && ((isPlayer1Turn && !isPlayer1) ||
                           (!isPlayer1Turn && isPlayer1))) ||
            (RuneCanBeRemoved(runeToRemove) ||
             (isNetworkGame && ((isPlayer1Turn && !isPlayer1) ||
                                (!isPlayer1Turn && isPlayer1)))))
        {
            networking.removeFrom = runeToRemove;

            if (runeList[runeToRemove].isInMill)
                RemoveRunesFromMill();

            RemoveOrb(runeToRemove);

            drawCount = 0;
        }
    }

    // Game Over //
    public void GameOver()
    {
        if (isNetworkGame && playerForfeit == "other")
        {
            // TODO: Popup that says "the other player has forfeit"
            // This could be similar to the movement phase message
        }
        else if (isNetworkGame && ((isPlayer1Turn && isPlayer1) ||
                 (!isPlayer1Turn && !isPlayer1)))
        {
            networking.SendMove();
        }

        waitingOnAnimation = true; //prevent clicking

        RemoveAllOrbHighlights(-1);
        RemoveAllRuneHighlights();

        if (isNetworkGame && playerForfeit == "other")
        {
            if (isPlayer1)
                uiController.displayWinMessage(player1Color);
            else
                uiController.displayWinMessage(player2Color);
        }
        else
        {
            //show win and lose messages
            if (isPlayer1Turn)
                uiController.displayWinMessage(player1Color);
            //print("Game Over. " + player1Color + " wins!");
            else
            {
                if(PlayerPrefs.GetString("GameType") == "Story")
                {
                    //Display prompt
                    uiController.displayLose();
                }
                else
                {
                    uiController.displayWinMessage(player2Color);
                    //print("Game Over. " + player2Color + " wins!");
                }
            }
        }
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelNum);
        while (!async.isDone)
        {
            yield return null;
        }
    }


    /*-----------------------------------------------------------------
    || GAME PHASE LOGIC
    -----------------------------------------------------------------*/
    public bool CanFly()
    {
        if ((isPlayer1Turn && player1OrbCount == 3) ||
            (!isPlayer1Turn && player2OrbCount == 3))
            return true;

        return false;
    }

    private void ChangeSide()
    {
        // Send move to opponent if in a network game
        if (isNetworkGame && ((isPlayer1Turn && isPlayer1) ||
            (!isPlayer1Turn && !isPlayer1)))
        {
            networking.SendMove();
            waitingOnOtherPlayer = true;
        }
        else if (isNetworkGame && ((isPlayer1Turn && !isPlayer1) ||
                 (!isPlayer1Turn && isPlayer1)))
        {
            waitingOnOtherPlayer = false;
        }

        if (isAIGame && drawCount >= 10)
        {
            drawCount = 0;
            //offer draw
            print("offering draw");
            canOfferDraw = true; //show something in the ui
            uiController.displayDraw();
        }

        if (isAIGame)
            waitingOnOtherPlayer = !isAITurn;
        isPlayer1Turn = !isPlayer1Turn;
        isAITurn = !isAITurn;
        networking.ResetNetworkValues();

        if (showHints)
        {
            LeanTween.cancel(centerOfBoard);

            if (!isNetworkGame && !isAIGame) //local game
            {
                if (isPlayer1Turn)
                    DisplayText("Player 1's Turn", 2);
                else
                    DisplayText("Player 2's Turn", 2);
            }
            else if (isNetworkGame)
            {
                if (isPlayer1Turn)
                {
                    if (isPlayer1)
                        DisplayText("It's Your Turn", 2);
                    else if (!isPlayer1)
                        DisplayText("It's Your Opponent's Turn", 2);
                }
                else
                {
                    if (!isPlayer1)
                        DisplayText("It's Your Turn", 2);
                    else if (isPlayer1)
                        DisplayText("It's Your Opponent's Turn", 2);
                }
            }
            else if (isAIGame)
            {
                if (isPlayer1Turn && isPlayer1)
                    DisplayText("It's Your Turn", 2);
                else if (!isPlayer1Turn && !isPlayer1)
                    DisplayText("It's Your Opponent's Turn", 2);
            }
        }

        if (previousGamePhase == "placement")
        {
            previousGamePhase = gamePhase;
            if (placementPhase_RoundCount > startingNumberOfOrbs)
            {
                gamePhase = "movementPickup";
                PrepareForMovementPhase();
                if (showHints)
                {
                    LeanTween.cancel(centerOfBoard);
                    DisplayMovementPhaseText();
                    previousGamePhase = "movementPhase";
                }
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

        // Get move from AI opponent if it is their turn
        // TODO: IF GAME IS NOT OVER
        if (isAIGame && isAITurn)
        {
            makeAIMove();
        }
    }
    // Makes a move from the game's AI
    private void makeAIMove()
    {
        short placedAIPieces;
        short placedHumanPieces;

        // Determine how many pieces have been placed so far

        if (PlayerPrefs.GetString("AIGoesFirst") == "true")
        {

            placedAIPieces = placementPhase_RoundCount;
            placedHumanPieces = (short)(placementPhase_RoundCount - 1);
        }
        else
        {
            placedAIPieces = (short)(placementPhase_RoundCount - 1);
            placedHumanPieces = placementPhase_RoundCount;
        }

        // Get the move
        AIMove = aicontroller.GetAIMove(AIDifficulty, placedAIPieces,
                                        placedHumanPieces);

        LeanTween.delayedCall(0.7f, () => {
            // Apply the move based on the current phase
            if (gamePhase == "placement")
                PlacementPhase(AIMove[1]);
            else
            {
                //Debug.Log("AI Move From: " + AIMove[0]);
                //Debug.Log("AI Move To: " + AIMove[1]);
                runeFromLocation = AIMove[0];
                MovementPhase_Place(AIMove[1]);
            }
        });

    }

    public bool ClickedOnDifferentPiece(short selectedRune)
    {
        if ((isPlayer1Turn && runeList[selectedRune].tag == "Player") ||
            (!isPlayer1Turn && runeList[selectedRune].tag == "Opponent"))
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

    public List<short> MakeListOfRunesForCurrentPlayer()
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
    public bool RuneIsInMill(short rune)
    {
        if (IsInHorizontalMill(rune) || IsInVerticalMill(rune))
        {
            if (showHints)
            {
                TextBox.gameObject.SetActive(false);
                LeanTween.cancel(centerOfBoard);
                if (!isNetworkGame && !isAIGame) //local game		
                {
                    if (isPlayer1Turn)
                        DisplayText("Player 1 Got A Mill!", 3);
                    else
                        DisplayText("Player 2 Got A Mill!", 3);
                }
                else if (isNetworkGame)
                {
                    if (isPlayer1Turn && isPlayer1)
                    {
                        DisplayText("You Got A Mill!", 3);
                    }
                    else if (!isPlayer1Turn && !isPlayer1)
                    {
                        DisplayText("You Got A Mill!", 3);
                    }
                }
                else if (isAIGame)
                {
                    if (isPlayer1Turn && isPlayer1)
                        DisplayText("You Got A Mill!", 3);
                    else if (!isPlayer1Turn && !isPlayer1)
                        DisplayText("You Got A Mill!", 3);
                }
            }
            return true;
        }

        return false;
    }

    public bool AllRunesAreInMills()
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
                    // It is possible that some of these orbs could still be in a mill.
                    RecheckForMills(mill.position1);
                    RecheckForMills(mill.position2);
                    RecheckForMills(mill.position3);
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
                    // It is possible that some of these orbs could still be in a mill.
                    RecheckForMills(mill.position1);
                    RecheckForMills(mill.position2);
                    RecheckForMills(mill.position3);
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

    private void RecheckForMills(short rune)
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
                }
            }
        }

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
                }
            }
        }
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
                            RecheckForMills(networking.moveTo);
                        }
                        else
                        {
                            TextBox.gameObject.SetActive(false);
                            LeanTween.cancel(centerOfBoard);
                            ChangeSide();
                        }
                    }
                }
                else if (isAIGame)
                {
                    if (((isPlayer1Turn && isPlayer1) || (!isPlayer1Turn && !isPlayer1)) && RuneIsInMill(toLocation))
                    {
                        PrepareForRemovalPhase();
                    }
                    else
                    {
                        // If there is an orb to remove on the receiving side, however,
                        // we don't want to call ChangeSide() quite yet.
                        if (((isPlayer1Turn && !isPlayer1) || (!isPlayer1Turn && isPlayer1)) && (AIMove[2] != -1) && RuneIsInMill(AIMove[1]))
                        {
                            LeanTween.delayedCall(0.7f, () => { 
                                RemovalPhase(AIMove[2]);
                            });
                        }
                        else
                        {
                            TextBox.gameObject.SetActive(false);
                            LeanTween.cancel(centerOfBoard);
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
                        TextBox.gameObject.SetActive(false);
                        LeanTween.cancel(centerOfBoard);
                        ChangeSide();
                    }
                }
                waitingOnAnimation = false;
            });
        });
    }

    public void RemoveOrb(short runeNumber)
    {
        waitingOnAnimation = true;

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

            LeanTween.delayedCall(gameObject, 0.1f, () =>
            {
                Destroy(orbToDestroy);
            });

            LeanTween.delayedCall(gameObject, 1f, () =>
            {
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

            if (placementPhase_RoundCount > startingNumberOfOrbs && (player1OrbCount == 2 || player2OrbCount == 2)) //check for win
                GameOver();
            else //continue game
            {
                TextBox.gameObject.SetActive(false);
                LeanTween.cancel(centerOfBoard);
                ChangeSide();
            }
            waitingOnAnimation = false;
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

    // Text Displaying //
    public void DisplayText(string text, float time)
    {
        TextBox.GetComponentInChildren<Text>().text = text;
        TextBox.gameObject.SetActive(true);
        LeanTween.delayedCall(centerOfBoard, time, () =>
        {
            TextBox.gameObject.SetActive(false);
            //TextBox.GetComponent<Renderer>().material.color.a = 0.5;
        });
    }

    private void DisplayMovementPhaseText()
    {
        Debug.Log("Movement Phase Has Begun!");
        StartCoroutine(uiController.displayPhase("Movement Phase!"));
        LeanTween.delayedCall(centerOfBoard, 3f, () =>
        {
            if (!isNetworkGame && !isAIGame) //local game		
            {
                if (isPlayer1Turn)
                    DisplayText("It's Player 1's Turn", 2);
                else
                    DisplayText("It's Player 2's Turn", 2);
            }
            else if (isNetworkGame)
            {
                if (isPlayer1Turn)
                {
                    if (isPlayer1)
                        DisplayText("It's Your Turn", 2);
                    else if (!isPlayer1)
                        DisplayText("It's Your Opponent's Turn", 2);
                }
                else
                {
                    if (!isPlayer1)
                        DisplayText("It's Your Turn", 2);
                    else if (isPlayer1)
                        DisplayText("It's Your Opponent's Turn", 2);
                }
            }
            else if (isAIGame)
            {
                if (isPlayer1Turn && isPlayer1)
                    DisplayText("It's Your Turn", 2);
                else if (!isPlayer1Turn && !isPlayer1)
                    DisplayText("It's Your Opponent's Turn", 2);
            }
        });
    }
}