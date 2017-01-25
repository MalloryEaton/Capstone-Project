using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{
    private GameController gameController;
    private Dictionaries dictionaries;
    private bool isPlayersTurn = true;

    public short runeNumber;
    private bool hasBeenClicked = false;
    private Renderer r;

    private short numberOfMaterials = 10;

    private Material highlightMaterial;
    private Material originalMaterial;

    

    void setRandomRotation()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 1000f), 0);
    }

    void setRandomMaterial()
    {
        short num = (short)Random.Range(0, numberOfMaterials);
        highlightMaterial = dictionaries.highlightMaterials[num];
        r.material = originalMaterial = dictionaries.originalMaterials[num];
    }

    // Use this for initialization
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        r = GetComponent<Renderer>();
        setRandomRotation();
        setRandomMaterial();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //detect click
    private void OnMouseDown()
    {
        if (tag == "Empty")
        {
            if (gameController.isPlayersTurn)
            {
                GameObject greenOrb = (GameObject)Instantiate(dictionaries.orbs[gameController.playerColor], dictionaries.orbPositions[runeNumber], Quaternion.identity);
                tag = "Taken";
            }
            else //opponent's turn
            {
                print(dictionaries.orbPositions[runeNumber]);
                GameObject purpleOrb = (GameObject)Instantiate(dictionaries.orbs[gameController.opponentColor], dictionaries.orbPositions[runeNumber], Quaternion.identity);
                tag = "Taken";
            }
            gameController.ChangePlayerTurn();
        }
        //hasBeenClicked = !hasBeenClicked;
        //if (hasBeenClicked)
        //{
        //    r.material = highlightMaterial;
        //}
        //else
        //{
        //    r.material = originalMaterial;
        //}
    }
}