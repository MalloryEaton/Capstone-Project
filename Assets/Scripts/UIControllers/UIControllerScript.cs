using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerScript : MonoBehaviour
{

    public GameObject DifficultyPanel;
    public GameObject MultiplayerPanel;
    public GameObject MainButtonPanel;
    public GameObject CharacterSelectPanel;
    public GameObject MainTitlePanel;


    void Awake()
    {
        MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        MainTitlePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
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
            MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            CharacterSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            //MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if(panel == "multiplayer")
        {
            MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "main")
        {
            DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            CharacterSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            MainTitlePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "character")
        {
            //DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            //MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            MainTitlePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            CharacterSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
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
            MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
            MainButtonPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
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
