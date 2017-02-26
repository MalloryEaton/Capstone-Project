using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbIntro : MonoBehaviour
{
    private Dictionaries dictionaries;
    private GameLogicController gameController;

    private void Awake()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        gameController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
    }

    public void FloatUp(short orbNumber, string color)
    {
        Vector3 moveTo = new Vector3();
        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.3f);
        if (!gameController.isNetworkGame || (gameController.isNetworkGame && gameController.isPlayer1))
        {
            if (color == gameController.player1Color)
            {
                moveTo = new Vector3(22.75f, 12.5f, 27.25f); //side 1
            }
            else
            {
                moveTo = new Vector3(1.25f, 12.5f, -3.25f); //side 2
            }
        }
        else if (gameController.isNetworkGame && !gameController.isPlayer1)
        {
            if (color == gameController.player1Color)
            {
                moveTo = new Vector3(1.25f, 12.5f, -3.25f); //side 2
            }
            else
            {
                moveTo = new Vector3(22.75f, 12.5f, 27.25f); //side 1
            }
        }
        LeanTween.move(gameObject, moveTo, 0.3f);
        LeanTween.delayedCall(0.5f, () =>
        {
            MoveToStartingLocation(orbNumber, color);
        });
    }

    public void MoveToStartingLocation(short orbNumber, string color)
    {
        if (!gameController.isNetworkGame || (gameController.isNetworkGame && gameController.isPlayer1))
        {
            if(color == gameController.player1Color)
            {
                LeanTween.move(gameObject, dictionaries.orbSide1Dictionary[orbNumber], 0.3f);
            }
            else
            {
                LeanTween.move(gameObject, dictionaries.orbSide2Dictionary[orbNumber], 0.3f);
            }
        }
        else if (gameController.isNetworkGame && !gameController.isPlayer1)
        {
            if (color == gameController.player1Color)
            {
                LeanTween.move(gameObject, dictionaries.orbSide2Dictionary[orbNumber], 0.3f);
            }
            else
            {
                LeanTween.move(gameObject, dictionaries.orbSide1Dictionary[orbNumber], 0.3f);
            }
        }
    }
}
