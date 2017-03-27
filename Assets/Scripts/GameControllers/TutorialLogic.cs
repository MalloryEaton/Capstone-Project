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
    }

    public void TransitionText()
    {
        textIndex++;

        TextBoxes[textIndex - 1].SetActive(false);
        TextBoxes[textIndex].SetActive(true);

        if (textIndex == 1)
        {
            GameObject orb = GameObject.Find("Green_Orb_" + 1);
            orb.AddComponent<OrbHoverController>();
            runeList[20].GetComponent<RuneControllerTutorial>().AddRuneHighlight();
        }
        if(textIndex == 2)
        {
            GameObject orbToMove = GameObject.Find("Purple_Orb_" + 1);
            LeanTween.delayedCall(orbToMove, 3f, () =>
            {
                float direction = isPlayer1Turn ? 180f : 0f;

                transform.LookAt(GameObject.Find("Rune" + 3).transform);
                anim2.Play("Attack1");

                LeanTween.delayedCall(0.7f, () =>
                {
                    LeanTween.rotateY(gameObject, direction, 0.1f);
                });
                LeanTween.move(orbToMove, dictionaries.orbPositionsDictionary[3], 0.5f).setOnComplete(() =>
                {
                    waitingOnAnimation = false;
                    isPlayer1Turn = !isPlayer1Turn;
                    TransitionText();
                });
            });
        }
    }

    public void MoveOrb(short toLocation, string name)
    {
        waitingOnAnimation = true;
        GameObject orbToMove = GameObject.Find(name);

        RemoveOrbHighlight(orbToMove);
        RemoveAllRuneHighlights();

        moveSound.Play();

        LeanTween.delayedCall(orbToMove, 0.3f, () =>
        {
            float direction = isPlayer1Turn ? 180f : 0f;

            transform.LookAt(GameObject.Find("Rune" + toLocation).transform);
            
            if(isPlayer1Turn)
                anim1.Play("Attack1");
            else
                anim2.Play("Attack1");

            LeanTween.delayedCall(0.7f, () =>
            {
                LeanTween.rotateY(gameObject, direction, 0.1f);
            });
            LeanTween.move(orbToMove, dictionaries.orbPositionsDictionary[toLocation], 0.5f).setOnComplete(() =>
            {
                waitingOnAnimation = false;
                isPlayer1Turn = !isPlayer1Turn;
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
}