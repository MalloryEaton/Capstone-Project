using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerScript : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject DifficultyPanel;
    public GameObject MultiplayerPanel;
    public GameObject MainButtonPanel;
    public GameObject CharacterSelectPanel;
    public GameObject MainTitlePanel;
    public GameObject MainPanel;
    public GameObject LevelSelectPanel;
    public GameObject BioPanel;

    void Awake()
    {
        MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        MainTitlePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        display("canvas");
        //BioPanel.SetActive(false);
        print("awake");
    }

    public void display(string panel)
    {    
        if (panel == "story")
        {
            print(panel);
        }
        else if (panel == "difficulty")
        {
            hide("main");
            hide("character");
            hide("bio");
            DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            display("title");
        }
        else if(panel == "multiplayer")
        {
            hide("main");
            MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "main")
        {
            hide("difficulty");
            hide("multiplayer");
            hide("character");
            display("title");
            MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "title")
        {
            MainTitlePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "character")
        {
            display("canvas");
            hide("level");
            hide("title");
            hide("main");
            hide("difficulty");
            CharacterSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "level")
        {
            hide("character");
            hide("canvas");
            hide("bio");
            LevelSelectPanel.SetActive(true);
        }
        else if (panel == "bio")
        {
           // BioPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "canvas")
        {
            MainPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }

    }

    public void hide(string panel)
    {
        if (panel == "story")
        {

        }
        else if (panel == "difficulty")
        {
            DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }
        else if (panel == "multiplayer")
        {
            MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }
        else if (panel == "main")
        {
            //MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }
        else if (panel == "title")
        {
            MainTitlePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }
        else if (panel == "character")
        {
            CharacterSelectScript.isCharacterSelected = false;
            CharacterSelectScript.characterSelectScript.ResetMages(false);
            CharacterSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
          //  BioPanel.SetActive(false);
        }
        else if (panel == "canvas")
        {
            MainPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }
        else if (panel == "bio")
        {
            CharacterSelectScript.isCharacterSelected = false;
            CharacterSelectScript.characterSelectScript.ResetMages(false);
            BioPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }
        else if(panel == "level")
        {
            LevelSelectPanel.SetActive(false);
        }
    }

    public void goToNetworkLobby()
    {
        SceneManager.LoadScene("LobbyMenu");
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
