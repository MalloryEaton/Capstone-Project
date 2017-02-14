using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : MonoBehaviour {
    private GameLogicController gameController;
    private Dictionaries dictionaries;

    private GameObject shrine; //used for instantiation

    public List<GameObject> shrines = new List<GameObject>(8);
    
	// Use this for initialization
	void Start ()
    {
        gameController = FindObjectOfType(typeof(GameLogicController)) as GameLogicController;
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
        if(gameController.isPlayer1Turn)
        {
            shrine = Instantiate(dictionaries.shrinesDictionary[gameController.player1Color], transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            shrine = Instantiate(dictionaries.shrinesDictionary[gameController.player2Color], transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
