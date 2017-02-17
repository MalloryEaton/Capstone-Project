using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour {
    
    private Animator anim;
    private GameLogicController gameController;

    void Start ()
    {
        anim = GetComponent<Animator>();
        gameController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
    }

    public void PlayAttack1Animation(GameObject rune)
    {
        float direction = 0;
        if(gameController.isNetworkGame)
        {
            if (gameController.isPlayer1 && gameController.isPlayer1Turn)
                direction = 180f;
            else if(!gameController.isPlayer1 && !gameController.isPlayer1Turn)
                direction = 0f;
        }
        else
            direction = gameController.isPlayer1 ? 180f : 0f;

        transform.LookAt(rune.transform);
        anim.Play("Attack1");

        LeanTween.delayedCall(0.7f, () =>
        {
            LeanTween.rotateY(gameObject, direction, 0.1f);
        });
    }
}
