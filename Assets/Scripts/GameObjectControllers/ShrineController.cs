﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : MonoBehaviour {
    private GameController gameController;
    private Dictionaries dictionaries;

    private GameObject shrine; //used for instantiation

    public List<GameObject> shrines = new List<GameObject>(8);
    
	// Use this for initialization
	void Start ()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //StartCoroutine(ChangeShrine());
        ChangeShrine();
    }
    
    //private IEnumerator ChangeShrine()
    private void ChangeShrine()
    {
        //yield return new WaitForSeconds(1);
        if(gameController.isPlayersTurn)
        {
            shrine = (GameObject)Instantiate(dictionaries.shrines[gameController.playerColor], transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            shrine = (GameObject)Instantiate(dictionaries.shrines[gameController.opponentColor], transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}