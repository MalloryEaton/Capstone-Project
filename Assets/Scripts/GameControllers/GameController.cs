using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public string gamePhase; //placement, movement, fly
    public bool isPlayersTurn;
    public string playerColor = "green";
    public string opponentColor = "purple";

    private Dictionaries dictionaries;

    // Use this for initialization
    void Start ()
    {
        dictionaries = FindObjectOfType(typeof(Dictionaries)) as Dictionaries;
        gamePhase = "placement";
        isPlayersTurn = true;
        Instantiate(dictionaries.shrines[playerColor], new Vector3(12,0,12), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ChangePlayerTurn()
    {
        isPlayersTurn = !isPlayersTurn;
    }
}
