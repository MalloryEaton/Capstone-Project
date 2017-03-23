using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour {
    
    private Animator anim;
    private GameLogicController gameController;

    void Awake ()
    {
        anim = GetComponent<Animator>();
        gameController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
        //PlayLevitateAnimation();
    }

    public void PlayAttack1Animation(GameObject rune)
    {
        float direction = 0;
        if(gameController.isNetworkGame && gameController.isPlayer1)
        {
            direction = gameController.isPlayer1Turn ? 180f : 0f;
        }
        else if(gameController.isNetworkGame && !gameController.isPlayer1)
        {
            direction = gameController.isPlayer1Turn ? 0f : 180f;
        }
        else
            direction = gameController.isPlayer1Turn ? 180f : 0f;

        transform.LookAt(rune.transform);
        anim.Play("Attack1");

        LeanTween.delayedCall(0.7f, () =>
        {
            LeanTween.rotateY(gameObject, direction, 0.1f);
        });
    }

    public void PlayLevitateAnimation()
    {
        anim.Play("LevitateStart");
    }

    public void PlayLandingAnimation()
    {
        anim.StopPlayback();
        anim.Play("Landing");
        if (gameController.showHints)
        {
            if (!gameController.isNetworkGame && !gameController.isAIGame)
            {
                gameController.DisplayText("It's Player 1's Turn", 2);
            }
            else if(gameController.isNetworkGame)
            {
                if(gameController.isPlayer1)
                    gameController.DisplayText("It's Your Turn", 2);
                else
                    gameController.DisplayText("It's Your Opponent's Turn", 2);
            }
            else if (gameController.isAIGame)
            {
                gameController.DisplayText("It's Your Turn", 2);
            }
        }
    }
}
