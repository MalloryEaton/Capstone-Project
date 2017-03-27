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
        if(!tutorialController.preventClick)
            AddRuneHighlight();
    }

    private void OnMouseExit()
    {
        if (!tutorialController.preventClick)
            RemoveRuneHighlight();
    }

    private void OnMouseDown()
    {
        if (!tutorialController.waitingOnAnimation)
        {
            switch(tutorialController.textIndex)
            {
                case 1:
                    if (runeNumber == 20)
                    {
                        tutorialController.MoveOrb(20, "Green_Orb_1");
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