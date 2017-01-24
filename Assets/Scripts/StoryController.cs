using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : MonoBehaviour {

    public GameObject StoryPanel;

    void Awake()
    {
        StoryPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        print("awake");
    }

    public void display()
    {
        StoryPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
    }

    public void hide()
    {
        StoryPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
