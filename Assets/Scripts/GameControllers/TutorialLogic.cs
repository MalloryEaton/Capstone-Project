using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialLogic : MonoBehaviour {

    public GameObject TextList;
    private Dictionaries dictionaries;
    public RuneControllerTutorial[] runeList;
    public List<GameObject> TextBoxes;
    public GameObject plane;

    public GameObject LoadingScreen;

    private GameObject player1Mage;
    private GameObject player2Mage;
    Animator anim1;
    Animator anim2;
    public string player1Color;
    public string player2Color;

    public bool menuIsOpen;

    public bool preventClick;
    public bool waitingOnAnimation;

    private AudioSource moveSound;
    private AudioSource removeSound;
    private AudioSource millSound;
    public GameObject music;

    public Slider sfxSlider;
    public Slider musicSlider;

    public short runeFromLocation;

    public string gamePhase = "Placement";
    public string previousGamePhase = "MovementPickup";
    private bool isPlayer1Turn = true;

    private bool canFly = false;

    private float speed = 0.3f;

    public int textIndex = 0;


    private void Awake()
    {
        LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", false);
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
    }

    void Start ()
    {
        menuIsOpen = false;
        textIndex = 0;
        preventClick = true;

        AudioSource[] audio = GetComponents<AudioSource>();
        moveSound = audio[0];
        removeSound = audio[1];
        millSound = audio[2];

        moveSound.volume = sfxSlider.value;
        removeSound.volume = sfxSlider.value;
        music.GetComponent<AudioSource>().volume = musicSlider.value;

        player1Color = "Green";
        player2Color = "Purple";

        InitializeGameBoard();
        SetupText();
    }

    public void sfxVolumeUpdate()
    {
        moveSound.volume = sfxSlider.value;
        removeSound.volume = sfxSlider.value;
        millSound.volume = sfxSlider.value;
    }

    public void musicVolumeUpdate()
    {
        music.GetComponent<AudioSource>().volume = musicSlider.value;
    }

    private void InitializeGameBoard()
    {
        InstantiateMages();
        InstantiateShrine();
        InstantiateOrbContainers();
    }

    private void InstantiateMages()
    {
        player1Mage = Instantiate(dictionaries.magesDictionary[player1Color], new Vector3(20, 1, 28.5f), new Quaternion(0, 180, 0, 0));
        player1Mage.tag = "Mage";

        player2Mage = Instantiate(dictionaries.magesDictionary[player2Color], new Vector3(4, 1, -4), new Quaternion(0, 0, 0, 0));
        player2Mage.tag = "Mage";

        anim1 = player1Mage.GetComponent<Animator>();
        anim2 = player2Mage.GetComponent<Animator>();
    }

    private void InstantiateShrine()
    {
        Instantiate(dictionaries.shrinesTutorialDictionary[player1Color], new Vector3(12, 0, 12), Quaternion.identity);
    }

    private void InstantiateOrbContainers()
    {
        Instantiate(dictionaries.orbContainersDictionary[player1Color], new Vector3(0, 0, 28), Quaternion.identity);
        Instantiate(dictionaries.orbContainersDictionary[player2Color], new Vector3(8, 0, -4), Quaternion.identity);
    }

    public void SetupText()
    {
        TextBoxes[0].SetActive(true);
        TextBoxes[1].SetActive(false);
        TextBoxes[2].SetActive(false);
        TextBoxes[3].SetActive(false);
        TextBoxes[4].SetActive(false);
        TextBoxes[5].SetActive(false);
        TextBoxes[6].SetActive(false);
        TextBoxes[7].SetActive(false);
        TextBoxes[8].SetActive(false);
        TextBoxes[9].SetActive(false);
        TextBoxes[10].SetActive(false);
        TextBoxes[11].SetActive(false);
        TextBoxes[12].SetActive(false);
        TextBoxes[13].SetActive(false);
        TextBoxes[14].SetActive(false);
        TextBoxes[15].SetActive(false);
        TextBoxes[16].SetActive(false);
        TextBoxes[17].SetActive(false);
        TextBoxes[18].SetActive(false);
        TextBoxes[19].SetActive(false);
        TextBoxes[20].SetActive(false);
        TextBoxes[21].SetActive(false);
        TextBoxes[22].SetActive(false);
        TextBoxes[23].SetActive(false);
        TextBoxes[24].SetActive(false);
        TextBoxes[25].SetActive(false);
        TextBoxes[26].SetActive(false);
        TextBoxes[27].SetActive(false);
        TextBoxes[28].SetActive(false);
        TextBoxes[29].SetActive(false);
        TextBoxes[30].SetActive(false);
        TextBoxes[31].SetActive(false);
        TextBoxes[32].SetActive(false);
        TextBoxes[33].SetActive(false);
        TextBoxes[34].SetActive(false);
        TextBoxes[35].SetActive(false);
        TextBoxes[36].SetActive(false);
    }

    public void returnToMenu()
    {
        LoadingScreen.GetComponent<Animator>().SetBool("isDisplayed", true);
        LeanTween.cancelAll();
        StartCoroutine(LoadAsync(1));
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelNum);
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void TransitionText()
    {
        textIndex++;

        TextBoxes[textIndex - 1].SetActive(false);
        TextBoxes[textIndex].SetActive(true);

        if (textIndex == 11)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
            runeList[20].GetComponent<RuneControllerTutorial>().AddRuneHighlight();
        }
        if (textIndex == 12)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
            MoveOrb(3, "Purple_Orb_1", 1f); //3
        }

        if (textIndex == 13)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
            runeList[5].GetComponent<RuneControllerTutorial>().AddRuneHighlight();
        }
        if (textIndex == 14)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
            MoveOrb(2, "Purple_Orb_2", 1f); //3
        }

        if (textIndex == 15)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
            runeList[13].GetComponent<RuneControllerTutorial>().AddRuneHighlight();
        }
        if (textIndex == 16)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
            isPlayer1Turn = true;
            InstantiateMagicRings("Green");
            MakeOrbHover(GameObject.Find("OrbAtLocation_2"));
            MakeOrbHover(GameObject.Find("OrbAtLocation_3"));
        }
        if(textIndex == 20)
        {
            ResetBoard();
        }
        if (textIndex == 23)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
            preventClick = false;
        }
        if (textIndex == 26)
        {
            isPlayer1Turn = true;
            Destroy(GameObject.Find("GreenOrbContainer(Clone)"));
            Destroy(GameObject.Find("PurpleOrbContainer(Clone)"));
            foreach (RuneControllerTutorial rune in runeList)
            {
                rune.tag = "Empty";
            }
            gamePhase = "MovementPickup";

            // set up board for movement phase
            Instantiate(dictionaries.orbContainersDictionary["GreenMovement"], new Vector3(0, 0, 0), Quaternion.identity);
            Instantiate(dictionaries.orbContainersDictionary["PurpleMovement"], new Vector3(0, 0, 0), Quaternion.identity);

            //add tags to non-empty runes
            runeList[0].tag = "Player";
            runeList[2].tag = "Player";
            runeList[5].tag = "Player";
            runeList[6].tag = "Player";
            runeList[7].tag = "Player";
            runeList[10].tag = "Player";
            runeList[13].tag = "Player";
            runeList[16].tag = "Player";
            runeList[22].tag = "Player";
            runeList[1].tag = "Opponent";
            runeList[3].tag = "Opponent";
            runeList[4].tag = "Opponent";
            runeList[8].tag = "Opponent";
            runeList[9].tag = "Opponent";
            runeList[11].tag = "Opponent";
            runeList[17].tag = "Opponent";
            runeList[21].tag = "Opponent";
            runeList[23].tag = "Opponent";

            // highlight moveable orbs
            HighlightMoveableOrbs(ThereIsAnAvailableMove(MakeListOfRunesForCurrentPlayer()));
        }
        if (textIndex == 27)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
        }
        if (textIndex == 31)
        {
            TextBoxes[textIndex].GetComponent<Button>().interactable = false;
            isPlayer1Turn = true;
            //fly phase
            Destroy(GameObject.Find("GreenOrbsMovementPhase(Clone)"));
            Destroy(GameObject.Find("PurpleOrbsMovementPhase(Clone)"));
            foreach (RuneControllerTutorial rune in runeList)
            {
                rune.tag = "Empty";
            }
            gamePhase = "MovementPickup";
            canFly = true;
            isPlayer1Turn = true;

            // set up board for movement phase
            Instantiate(dictionaries.orbContainersDictionary["GreenFly"], new Vector3(0, 0, 0), Quaternion.identity);
            Instantiate(dictionaries.orbContainersDictionary["PurpleFly"], new Vector3(0, 0, 0), Quaternion.identity);

            //add tags to non-empty runes
            runeList[2].tag = "Player";
            runeList[10].tag = "Player";
            runeList[16].tag = "Player";
            runeList[1].tag = "Opponent";
            runeList[4].tag = "Opponent";
            runeList[7].tag = "Opponent";
            runeList[11].tag = "Opponent";
            runeList[21].tag = "Opponent";
        }
    }

    public void MovementPhase_Pickup(short selectedRune)
    {
        if (RuneCanBeMoved(selectedRune))
        {
            runeFromLocation = selectedRune;

            RemoveAllRuneHighlights();
            RemoveAllOrbHighlights(selectedRune);
            HighlightAvailableMoves(selectedRune);

            previousGamePhase = gamePhase;
            gamePhase = "MovementPlace";
        }
    }

    public void MovementPhase_Place(short toLocation)
    {
        if (runeList[toLocation].tag == "Empty")
        {
            if (IsLegalMove(toLocation))
            {
                RemoveOrbHighlight(runeFromLocation);

                MoveOrb(toLocation, "OrbAtLocation_" + runeFromLocation, 0.3f);

                runeList[toLocation].tag = (isPlayer1Turn) ? "Player" : "Opponent";
                runeList[runeFromLocation].tag = "Empty";
            }
        }
        else if (ClickedOnDifferentPiece(toLocation)) //switch to highlighted piece
        {
            previousGamePhase = "MovementPlace";
            MovementPhase_Pickup(toLocation);
        }
    }

    private bool IsLegalMove(short toLocation)
    {
        if (canFly) //can fly
        {
            if (runeList[toLocation])
                return true;
        }
        else
        {
            short[] adjacentRunes = dictionaries.adjacencyDictionary[runeFromLocation];
            foreach (short rune in adjacentRunes)
                if(rune == toLocation)
                    return true;
        }

        return false;
    }

    public bool ClickedOnDifferentPiece(short selectedRune)
    {
        List<short> runesThatCanMove = ThereIsAnAvailableMove(MakeListOfRunesForCurrentPlayer());

        if (runeList[selectedRune].tag == "Player" && runesThatCanMove.Contains(selectedRune))
            return true;
        return false;
    }

    private void RemoveAllOrbHighlights(short runeNumber)
    {
        List<short> runes = MakeListOfRunesForCurrentPlayer();
        List<short> runesThatCanMove = ThereIsAnAvailableMove(MakeListOfRunesForCurrentPlayer());

        if(runesThatCanMove.Contains(runeNumber))
        {
            foreach (short rune in runes)
            {
                RemoveOrbHighlight(rune);
                if (rune == runeNumber)
                {
                    MakeOrbHover(GameObject.Find("OrbAtLocation_" + rune));
                }
            }
        }
    }

    private void RemoveAllOrbHighlights()
    {
        foreach (RuneControllerTutorial rune in runeList)
        {
            if (rune.tag != "Empty")
                RemoveOrbHighlightStill(GameObject.Find("OrbAtLocation_" + rune.runeNumber), rune.runeNumber);
        }
    }

    private void HighlightAvailableMoves(short rune)
    {
        if (canFly)
        {
            foreach (RuneControllerTutorial r in runeList)
            {
                if (r.tag == "Empty")
                {
                    r.GetComponent<RuneControllerTutorial>().AddRuneHighlight();
                }
            }
        }
        else
        {
            foreach (short availableMove in dictionaries.adjacencyDictionary[rune])
            {
                if (runeList[availableMove].tag == "Empty")
                {
                    runeList[availableMove].GetComponent<RuneControllerTutorial>().AddRuneHighlight();
                }
            }
        }
    }

    private bool RuneCanBeMoved(short selectedRune)
    {
        if (runeList[selectedRune].tag == "Player")
        {
            return true;
        }
        return false;
    }

    private void ResetBoard()
    {
        Destroy(GameObject.Find("GreenOrbContainer(Clone)"));
        Destroy(GameObject.Find("PurpleOrbContainer(Clone)"));
        foreach (RuneControllerTutorial rune in runeList)
        {
            rune.tag = "Empty";
        }
        InstantiateOrbContainers();
        isPlayer1Turn = true;
    }

    private void RemoveOrbHighlight(short rune)
    {
        GameObject orb = GameObject.Find("OrbAtLocation_" + rune);
        Destroy(orb.GetComponent<OrbHoverController>());
        orb.transform.position = dictionaries.orbPositionsDictionary[rune];
    }

    private void PlayAttackAnimation(short toLocation)
    {
        float direction = isPlayer1Turn ? 180f : 0f;

        if (isPlayer1Turn)
        {
            anim1.Play("Attack1");
            player1Mage.transform.LookAt(GameObject.Find("Rune" + toLocation).transform);
        }
        else
        {
            anim2.Play("Attack1");
            player2Mage.transform.LookAt(GameObject.Find("Rune" + toLocation).transform);
        }

        LeanTween.delayedCall(0.7f, () =>
        {
            if (isPlayer1Turn)
                LeanTween.rotateY(player1Mage, direction, 0.1f);
            else
                LeanTween.rotateY(player2Mage, direction, 0.1f);
        });
    } 

    public void MoveOrb(short toLocation, string name, float speed)
    {
        waitingOnAnimation = true;
        preventClick = true;
        GameObject orbToMove = GameObject.Find(name);

        RemoveOrbHighlightMoving(orbToMove);
        RemoveAllRuneHighlights();

        orbToMove.name = "OrbAtLocation_" + toLocation;
        runeList[toLocation].tag = isPlayer1Turn ? "Player" : "Opponent";

        LeanTween.delayedCall(orbToMove, speed, () =>
        {
            moveSound.Play();
            PlayAttackAnimation(toLocation);
            
            LeanTween.delayedCall(0.3f, () =>
            {
                LeanTween.move(orbToMove, dictionaries.orbPositionsDictionary[toLocation], 0.5f).setOnComplete(() =>
                {
                    waitingOnAnimation = false;
                    isPlayer1Turn = !isPlayer1Turn;
                    TransitionText();
                });
            });
        });
    }

    public void DestroyOrb(short runeNumber)
    {
        PlayAttackAnimation(runeNumber);
        GameObject orbToDestroy = GameObject.Find("OrbAtLocation_" + runeNumber);
        runeList[runeNumber].tag = "Empty";
        GameObject hit;
        LeanTween.delayedCall(gameObject, 0.6f, () =>
        {
            removeSound.Play();
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
                RemoveAllOrbHighlights();
                DestroyMagicRings();

                TransitionText();
            });
        });
    }

    private void RemoveAllRuneHighlights()
    {
        for (int i = 0; i < runeList.Length; i++)
        {
            runeList[i].GetComponent<RuneControllerTutorial>().RemoveRuneHighlight();
        }
    }
    
    private void MakeOrbHover(GameObject orb)
    {
        orb.AddComponent<OrbHoverController>();
    }

    private void RemoveOrbHighlightMoving(GameObject orb)
    {
        Destroy(orb.GetComponent<OrbHoverController>());
    }

    private void RemoveOrbHighlightStill(GameObject orb, short rune)
    {
        Destroy(orb.GetComponent<OrbHoverController>());
        orb.transform.position = dictionaries.orbPositionsDictionary[rune];
    }

    private void InstantiateMagicRings(string color)
    {
        Transform ringTransform = dictionaries.magicRingDictionary[color].transform;

        Transform t1 = runeList[20].transform;
        Transform t2 = runeList[13].transform;
        Transform t3 = runeList[5].transform;

        millSound.Play();

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

    public List<short> MakeListOfRunesForCurrentPlayer()
    {
        List<short> runes = new List<short>();

        foreach (RuneControllerTutorial rune in runeList)
            if (rune.tag == "Player")
                runes.Add(rune.runeNumber);

        return runes;
    }

    private void HighlightMoveableOrbs(List<short> runes)
    {
        foreach (short rune in runes)
        {
            MakeOrbHover(GameObject.Find("OrbAtLocation_" + rune));
        }
    }

    private List<short> ThereIsAnAvailableMove(List<short> runes)
    {
        List<short> moveableRunes = new List<short>();
        if (!canFly)
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
            }
        }
        return moveableRunes;
    }
 }