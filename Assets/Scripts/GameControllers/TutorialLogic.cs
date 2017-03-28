using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLogic : MonoBehaviour {

    public GameObject TextList;
    private Dictionaries dictionaries;
    public RuneControllerTutorial[] runeList;
    public GameObject TutorialPanel;
    public List<GameObject> TextBoxes;

    private GameObject player1Mage;
    private GameObject player2Mage;
    Animator anim1;
    Animator anim2;
    public string player1Color;
    public string player2Color;

    public bool preventClick;
    public bool waitingOnAnimation;

    private AudioSource moveSound;
    private AudioSource removeSound;

    private bool isPlayer1Turn = true;

    private float speed = 0.3f;

    public int textIndex = 0;

    private void Awake()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
    }

    void Start ()
    {
        preventClick = true;

        AudioSource[] audio = GetComponents<AudioSource>();
        moveSound = audio[0];
        removeSound = audio[1];
        
        player1Color = "Green";
        player2Color = "Purple";

        InitializeGameBoard();
        SetupText();
    }

    private void InitializeGameBoard()
    {
        InstantiateMages();
        InstantiateShrine();
        InstantiateOrbContainers();
    }

    private void InstantiateMages()
    {
        player1Mage = Instantiate(dictionaries.magesDictionary[player1Color], new Vector3(20, 1, 28), new Quaternion(0, 180, 0, 0));
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
    }

    public void TransitionText()
    {
        textIndex++;

        TextBoxes[textIndex - 1].SetActive(false);
        TextBoxes[textIndex].SetActive(true);

        if (textIndex == 10)
        {
            GameObject.Find("Green_Orb_1").AddComponent<OrbHoverController>();
            runeList[20].GetComponent<RuneControllerTutorial>().AddRuneHighlight();
        }
        if (textIndex == 11)
        {
            MoveOrb(3, "Purple_Orb_1", 3f);
        }

        if (textIndex == 12)
        {
            GameObject.Find("Green_Orb_2").AddComponent<OrbHoverController>();
            runeList[5].GetComponent<RuneControllerTutorial>().AddRuneHighlight();
        }
        if (textIndex == 13)
        {
            MoveOrb(2, "Purple_Orb_2", 3f);
        }

        if (textIndex == 14)
        {
            GameObject.Find("Green_Orb_3").AddComponent<OrbHoverController>();
            runeList[13].GetComponent<RuneControllerTutorial>().AddRuneHighlight();
        }
        if (textIndex == 15)
        {
            isPlayer1Turn = true;
            InstantiateMagicRings("Green");
            MakeOrbHover(GameObject.Find("OrbAtLocation_2"));
            MakeOrbHover(GameObject.Find("OrbAtLocation_3"));
        }
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
        GameObject orbToMove = GameObject.Find(name);

        RemoveOrbHighlight(orbToMove);
        RemoveAllRuneHighlights();

        orbToMove.name = "OrbAtLocation_" + toLocation;

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

    private void RemoveOrbHighlight(GameObject orb)
    {
        Destroy(orb.GetComponent<OrbHoverController>());
    }

    private void InstantiateMagicRings(string color)
    {
        Transform ringTransform = dictionaries.magicRingDictionary[color].transform;

        Transform t1 = runeList[20].transform;
        Transform t2 = runeList[13].transform;
        Transform t3 = runeList[5].transform;

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
}