using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatEntryScript : MonoBehaviour {
    public Text playerName;
    public Text message;

	// Use this for initialization
	void Start () {
		
	}

    public void Setup(string name, string msg)
    {
        playerName.text = name;
        message.text = msg;
    }
	

}
