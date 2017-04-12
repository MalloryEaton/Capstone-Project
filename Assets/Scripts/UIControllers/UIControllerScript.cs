using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerScript : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 mainCameraPosition;
    private Quaternion mainCameraRotation;

    public GameObject DifficultyPanel;
    public GameObject MultiplayerPanel;
    public GameObject MainButtonPanel;
    public GameObject CharacterSelectPanel;
    public GameObject MainTitlePanel;
    public GameObject MainPanel;
    public GameObject LevelSelectPanel;
    public GameObject BioPanel;
    public GameObject LogoPanel;
    public GameObject turnSelectPanel;
    public List<GameObject> TutorialSlides;

    private CameraMovementController cmc;

    public bool networkGame = false;
    public bool localGame = false;
    public bool quickGame = false;
    public bool story = false;
    public bool isLogoPanelActive = true;
    public int slideIndex;

    private GameObject LoadingPanel;

    public GameObject storyContinuePanel;

    void Awake()
    {
        mainCameraPosition = mainCamera.transform.position;
        mainCameraRotation = mainCamera.transform.rotation;

        LoadingPanel = GameObject.Find("LoadingPanel");
        CharacterSelectScript.currentPlayerColor = "Player1Color";
        cmc = FindObjectOfType(typeof(CameraMovementController)) as CameraMovementController;
        display("canvas");
        LogoPanel.SetActive(true);
        print("awake");
        slideIndex = 0;
        storyContinuePanel.SetActive(false);
        turnSelectPanel.SetActive(false);
        PlayerPrefs.DeleteKey("Player1Color");
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            if(isLogoPanelActive)
            {
                display("main");
            }
        }
    }

    public void display(string panel)
    {
        if (panel == "story")
        {

            if(PlayerPrefs.HasKey("StoryStage"))
            {
                storyContinuePanel.SetActive(true);
                storyContinuePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            }
            else
            {
                startNewStory();
            }


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
            hide("tutorial");
            hide("difficulty");
            hide("multiplayer");
            hide("character");
            hide("logo");
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
            //reset camera
            mainCamera.transform.position = mainCameraPosition;
            mainCamera.transform.rotation = mainCameraRotation;

            hide("level");
            //hide("title");
            hide("main");
            hide("difficulty");
            hide("multiplayer");
            if(localGame)
            {
                CharacterSelectScript.characterSelectScript.PageHeader.text = "Player 1 Character Select";
            }
            else
            {
                CharacterSelectScript.characterSelectScript.PageHeader.text = "Character Select";
            }
            if (networkGame == true)
            {
                CharacterSelectScript.currentPlayerColor = "PlayerColor";
            }
            else if (localGame == true)
            {
                CharacterSelectScript.currentPlayerColor = "Player1Color";
            }
            CharacterSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else if (panel == "level")
        {
            hide("character");
            hide("canvas");
            hide("bio");
            cmc.cameraInit();
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
            //load scene based on stage that camera is looking at
            SceneManager.LoadScene("GameBoard");
        }
        else if (panel == "tutorial")
        {
            LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            int sceneToLoad = 10;
            StartCoroutine(LoadAsync(sceneToLoad));

            //hide("tutorial");
            //hide("difficulty");
            //hide("multiplayer");
            //hide("character");
            //slideIndex = 0;
            //TutorialPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            //TutorialSlides[0].SetActive(true);
            //for (int i = 1; i < TutorialSlides.Count; i++)
            //{
            //    TutorialSlides[i].SetActive(false);
            //}
        }
        else if (panel == "logo")
        {
            hide("level");
            hide("title");
            hide("main");
            hide("difficulty");
            hide("multiplayer");
            LogoPanel.SetActive(true);
            isLogoPanelActive = true;
        }
    }

    public void continueStory()
    {
        PlayerPrefs.SetString("GameType", "Story");
        storyContinuePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        storyContinuePanel.SetActive(false);
        LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        switch (PlayerPrefs.GetInt("StoryStage"))
        {
            case 0:
               // PlayerPrefs.SetInt("StoryStage", 1);
                StartCoroutine(LoadAsync(11));
                break;
            case 1:
               // PlayerPrefs.SetInt("StoryStage", 2);
                StartCoroutine(LoadAsync(12));
                break;
            case 2:
                //PlayerPrefs.SetInt("StoryStage", 3);
                StartCoroutine(LoadAsync(13));
                break;
            case 3:
               // PlayerPrefs.SetInt("StoryStage", 4);
                StartCoroutine(LoadAsync(14));
                break;
            case 4:
                //PlayerPrefs.SetInt("StoryStage", 5);
                StartCoroutine(LoadAsync(15));
                break;
            case 5:
               // PlayerPrefs.SetInt("StoryStage", 6);
                StartCoroutine(LoadAsync(16));
                break;
                //case 6:
                //    StartCoroutine(LoadAsync(16));
                //    break;
        }
    }

    public void startNewStory()
    {
        storyContinuePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        storyContinuePanel.SetActive(false);
        LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        PlayerPrefs.SetString("GameType", "Story");
        PlayerPrefs.SetInt("StoryStage", 0);
        StartCoroutine(LoadAsync(11));
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
        else if(panel == "logo")
        {
            LogoPanel.SetActive(false);
            isLogoPanelActive = false;
        }
    }

    public void backButtonSelected(string currentPanel)
    {
        if (currentPanel == "character")
        {
            if(CharacterSelectScript.currentPlayerColor == "Player1Color" || CharacterSelectScript.currentPlayerColor == "PlayerColor")
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
            else if(PlayerPrefs.GetString("GameType") == "AI")
            {
                hide("character");
                displayTurnSelect();
            }
            {
                string player1Color = PlayerPrefs.GetString("Player1Color");
                Button player1Button = GameObject.Find(player1Color + "MageButton").GetComponent<Button>();
                player1Button.interactable = true;
                CharacterSelectScript.currentPlayerColor = "Player1Color";
                CharacterSelectScript.characterSelectScript.PageHeader.text = "Player 1 Character Select";
                hide("bio");
            }
        }
    }

    public void setDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
        hide("difficulty");
        displayTurnSelect();
        //display("character");
    }

    public void displayTurnSelect()
    {
        turnSelectPanel.SetActive(true);
        turnSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
    }

    public void hideTurnSelect()
    {
        turnSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        turnSelectPanel.SetActive(false);
    }

    public void hideStoryContinuePrompt()
    {
        storyContinuePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        storyContinuePanel.SetActive(false);
    }

    public void goToNetworkLobby()
    {
        SceneManager.LoadScene("LobbyMenu");
    }

    public void networkGameSelected()
    {
        networkGame = true;
        localGame = false;
        quickGame = false;
        story = false;
        PlayerPrefs.SetString("GameType", "Network");
        display("character");
    }

    public void localGameSelected()
    {
        localGame = true;
        networkGame = false;
        quickGame = false;
        story = false;
        PlayerPrefs.SetString("GameType", "Local");
        display("character");
    }

    public void quickGameSelected()
    {
        quickGame = true;
        networkGame = false;
        localGame = false;
        story = false;
        PlayerPrefs.SetString("GameType", "AI");
        display("difficulty");
    }

    public void DifficultySelected(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
    }

    public void storySelected()
    {
        story = true;
        networkGame = false;
        localGame = false;
        quickGame = false;
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
            //blah
            if(CharacterSelectScript.currentPlayerColor == "Player1Color")
            {
                print("Player 1 Color: " + PlayerPrefs.GetString("Player1Color"));
                string player1Color = PlayerPrefs.GetString("Player1Color");
                Button player1Button = GameObject.Find(player1Color + "MageButton").GetComponent<Button>();
                player1Button.interactable = false;

                CharacterSelectScript.currentPlayerColor = "Player2Color";
                CharacterSelectScript.characterSelectScript.PageHeader.text = "Player 2 Character Select";
                hide("bio");
                //hid
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
        LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        int sceneToLoad = -1;

        switch(PlayerPrefs.GetString("Stage"))
        {
            case "Forest":
                sceneToLoad = 4;
                break;
            case "Graveyard":
                sceneToLoad = 5;
                break;
            case "Desert":
                sceneToLoad = 6;
                break;
            case "Volcano":
                sceneToLoad = 7;
                break;
            case "Water":
                sceneToLoad = 8;
                break;
            case "Tower":
                sceneToLoad = 9;
                break;
        }

        StartCoroutine(LoadAsync(sceneToLoad));
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelNum);
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void quitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void setPlayerTurn(int turn)
    {
        if (turn == 1)
        {
            PlayerPrefs.SetString("AIGoesFirst", "false");
        }
        else if (turn == 2)
        {
            PlayerPrefs.SetString("AIGoesFirst", "true");
        }
        hideTurnSelect();
        display("character");
        Debug.Log(PlayerPrefs.GetString("AIGoesFirst"));
    }

}
