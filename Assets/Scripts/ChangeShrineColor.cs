using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShrineColor : MonoBehaviour {
    //private string enemyColor = "purple";
    //private string playerColor = "green";

    private bool playerIsWinning = false;

    public List<GameObject> shrines = new List<GameObject>(8);
    private Dictionary<string, short> colorToNumber;

    private void initializeColorDictionary()
    {
        colorToNumber = new Dictionary<string, short>();
        colorToNumber.Add("black", 0);
        colorToNumber.Add("blue", 1);
        colorToNumber.Add("green", 2);
        colorToNumber.Add("orange", 3);
        colorToNumber.Add("purple", 4);
        colorToNumber.Add("red", 5);
        colorToNumber.Add("white", 6);
        colorToNumber.Add("yellow", 7);
    }

	// Use this for initialization
	void Start () {
        initializeColorDictionary();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        print(playerIsWinning);
        playerIsWinning = !playerIsWinning;

        if(playerIsWinning)
        {
            //GameObject shrine = (GameObject)Instantiate(shrines[colorToNumber[playerColor]], transform.position, transform.rotation);
            //Destroy(gameObject);
        }
        else
        {
            //GameObject shrine = (GameObject)Instantiate(shrines[colorToNumber[enemyColor]], transform.position, transform.rotation);
            //Destroy(gameObject);
        }
    }
}
