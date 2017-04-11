using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]

public class ChatListItem
{
    public Text playerName;
    public Text message;
}

public class ChatScrollList : MonoBehaviour {

    public Transform contentPanel;
    public GameObject chatEntry;
    public GameObject scrollView;

    // Use this for initialization
    void Start () {
		
	}
	
	public void updateChat(string name, string msg)
    {
        GameObject newChatEntry = Instantiate(chatEntry) as GameObject;
        Text [] labels = newChatEntry.GetComponentsInChildren<Text>();
        labels[0].text = msg;
        labels[1].text = name + ":";
        //ces.playerName.text = name;
        // ces.message.text = msg;
        newChatEntry.transform.SetParent(contentPanel);
        scrollView.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }
}
