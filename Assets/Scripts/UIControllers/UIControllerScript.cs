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

    public bool networkGame = false;
    public bool localGame = false;
    public bool quickGame = false;
    public bool story = false;

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
            PlayerPrefs.SetString("GameType", "Story");
        }
        else if (panel == "difficulty") //quickplay
        {
            hide("main");
            hide("character");
            hide("bio");
            DifficultyPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            display("title");
            //PlayerPrefs.SetString("Difficulty", );IMPORTANT!!!!!!!!!!!
        }
        else if (panel == "multiplayer")
        {
            hide("main");
            hide("character");
            MultiplayerPanel.GetComponent<Animator>().SetBool("isDisplayed", true);

            PlayerPrefs.SetString("GameType", "TwoPlayers");
        }
        else if (panel == "main")
        {
            hide("difficulty");
            hide("multiplayer");
            hide("character");
            display("title");
            resetPrefs("GameType", "");
            networkGame = false;
            localGame = false;
            quickGame = false;
            story = false;
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
            hide("multiplayer");
            CharacterSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "level")
        {
            hide("character");
            hide("canvas");
            hide("bio");
            LevelSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "bio")
        {
            // BioPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "canvas")
        {
            MainPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "play") // Start game
        {
            SceneManager.LoadScene("GameBoard");
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
            LevelSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        }
    }

    public void backButtonSelected(string currentPanel)
    {
        if (currentPanel == "character")
        {
            if(CharacterSelectScript.currentPlayerColor == "Player1Color")
            {
                if(networkGame == true || localGame == true)
                {
                    display("multiplayer");
                }
                else if(quickGame == true)
                {
                    print("test");
                    display("difficulty");
                }
            }
            else
            {
                CharacterSelectScript.currentPlayerColor = "Player1Color";
                hide("bio");
            }
        }
    }

    public void setDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
        display("character");
    }

    public void goToNetworkLobby()
    {
        SceneManager.LoadScene("LobbyMenu");
    }

    public void networkGameSelected()
    {
        networkGame = true;
        PlayerPrefs.SetString("GameType", "Network");
        display("character");
    }

    public void localGameSelected()
    {
        localGame = true;
        PlayerPrefs.SetString("GameType", "Local");
        display("character");
    }

    public void quickGameSelected()
    {
        quickGame = true;
        PlayerPrefs.SetString("GameType", "AI");
        display("difficulty");
    }

    public void storySelected()
    {
        story = true;
        PlayerPrefs.SetString("GameType", "Story");
        display("story");
    }

    public void resetPrefs(string prefType, string prefSetting)
    {
        PlayerPrefs.SetString(prefType, prefSetting);
    }

    public void continueFromCharacter()
    {
        if(networkGame == true)
        {
            goToNetworkLobby();
        }
        else if(localGame == true)
        {
            if(CharacterSelectScript.currentPlayerColor == "Player1Color")
            {
                CharacterSelectScript.currentPlayerColor = "Player2Color";
                hide("bio");
            }
            else if(CharacterSelectScript.currentPlayerColor == "Player2Color")
            {
                display("level");
            }
        }
        else if(quickGame == true)
        {
            display("level");
        }
    }

    public void startGame()
    {
        SceneManager.LoadScene("GameBoard");
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
