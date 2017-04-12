using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneControllerTutorial : MonoBehaviour
{
    private TutorialLogic tutorialController;
    private Dictionaries dictionaries;

    public bool isInMill;

    public short runeNumber;
    private Renderer runeRenderer;

    private short numberOfMaterials = 10;

    private Material highlightMaterial;
    private Material originalMaterial;

    // Use this for initialization
    void Start()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        tutorialController = FindObjectOfType(typeof(TutorialLogic)) as TutorialLogic;
        runeRenderer = GetComponent<Renderer>();
        setRandomRotation();
        setRandomMaterial();
    }

    void setRandomRotation()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 1000f), 0);
    }

    void setRandomMaterial()
    {
        short num = (short)Random.Range(0, numberOfMaterials);
        highlightMaterial = dictionaries.runeHighlightsDictionary[num];
        runeRenderer.material = originalMaterial = dictionaries.runeOriginalsDictionary[num];
    }

    private void OnMouseEnter()
    {
        if(tutorialController.gamePhase == "Placement" && !tutorialController.preventClick)
            AddRuneHighlight();
    }

    private void OnMouseExit()
    {
        if (tutorialController.gamePhase == "Placement" && !tutorialController.preventClick)
            RemoveRuneHighlight();
    }

    private void OnMouseDown()
    {
        if (!tutorialController.waitingOnAnimation && !tutorialController.menuIsOpen)
        {
            switch(tutorialController.textIndex)
            {
                case 11:
                    if (runeNumber == 20)
                    {
                        tutorialController.MoveOrb(20, "Green_Orb_1", 0.3f);
                    }
                    break;
                case 13:
                    if (runeNumber == 5)
                    {
                        tutorialController.MoveOrb(5, "Green_Orb_2", 0.3f);
                    }
                    break;
                case 15:
                    if (runeNumber == 13)
                    {
                        tutorialController.MoveOrb(13, "Green_Orb_3", 0.3f);
                    }
                    break;
                case 16:
                    if (runeNumber == 2 || runeNumber == 3)
                    {
                        tutorialController.DestroyOrb(runeNumber);
                    }
                    break;
                case 23:
                    tutorialController.MoveOrb(runeNumber, "Green_Orb_1", 0.3f);
                    tutorialController.preventClick = true;
                    break;
                case 27:
                    if(tutorialController.gamePhase == "MovementPickup")
                    {
                        tutorialController.MovementPhase_Pickup(runeNumber);
                    }
                    else if(tutorialController.gamePhase == "MovementPlace")
                    {
                        tutorialController.MovementPhase_Place(runeNumber);
                    }
                    break;
                case 31:
                    if (tutorialController.gamePhase == "MovementPickup")
                    {
                        tutorialController.MovementPhase_Pickup(runeNumber);
                    }
                    else if (tutorialController.gamePhase == "MovementPlace")
                    {
                        tutorialController.MovementPhase_Place(runeNumber);
                    }
                    break;
            }
        }
    }

    public void AddRuneHighlight()
    {
        runeRenderer.material = highlightMaterial;
    }

    public void RemoveRuneHighlight()
    {
        runeRenderer.material = originalMaterial;
    }
}