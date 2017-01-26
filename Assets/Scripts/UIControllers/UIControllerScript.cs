using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerScript : MonoBehaviour
{

    public GameObject StoryPanel;
    public GameObject DifficultyPanel;


    void Awake()
    {
        StoryPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        print("awake");
    }

    public void display(string panel)
    {    
        if (panel == "story")
        {
            print(panel);
            StoryPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "difficulty")
        {
            DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }

    }

    public void hide(string panel)
    {
        if (panel == "story")
        {
            StoryPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }
        else if (panel == "difficulty")
        {
            DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }

    }

    public void highlightLabel()
    {
       
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
