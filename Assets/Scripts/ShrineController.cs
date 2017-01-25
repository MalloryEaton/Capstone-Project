using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : MonoBehaviour {
    private GameController gameController;
    private Dictionaries dictionaries;

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
        StartCoroutine(ChangeShrine());
    }
    
    private IEnumerator ChangeShrine()
    {
        yield return new WaitForSeconds(2);
        if(gameController.isPlayersTurn)
        {
            GameObject shrine = (GameObject)Instantiate(dictionaries.shrines[gameController.playerColor], transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            GameObject shrine = (GameObject)Instantiate(dictionaries.shrines[gameController.opponentColor], transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
