using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 mainCameraPosition;
    private Quaternion mainCameraRotation;

    public GameObject DifficultyPanel;
    public GameObject MultiplayerPanel;
    public GameObject CharacterSelectPanel;
    public GameObject MainTitlePanel;
    public GameObject MainPanel;
    public GameObject LevelSelectPanel;
    public GameObject BioPanel;
    public GameObject LogoPanel;
    public GameObject BackgroundPanel;
    public GameObject turnSelectPanel;

    private CameraMovementController cmc;

    public bool networkGame = false;
    public bool localGame = false;
    public bool quickGame = false;
    public bool story = false;
    public bool isLogoPanelActive = true;
    public int slideIndex;

    private GameObject LoadingPanel;

    public GameObject storyContinuePanel;

    private float offScreenX;
    private float onScreenX;

    void Awake()
    {
        mainCameraPosition = mainCamera.transform.position;
        mainCameraRotation = mainCamera.transform.rotation;

        LoadingPanel = GameObject.Find("LoadingPanel");
        CharacterSelectScript.currentPlayerColor = "Player1Color";
        cmc = FindObjectOfType(typeof(CameraMovementController)) as CameraMovementController;
        LogoPanel.SetActive(true);
        storyContinuePanel.SetActive(false);
    }

    private void Start()
    {
        offScreenX = -2000;
        onScreenX = -440;
        HidePanels();
        PlayerPrefs.DeleteKey("Player1Color");
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (isLogoPanelActive)
            {
                LogoPanel.SetActive(false);
                isLogoPanelActive = false;
                ShowPanel("Main");
            }
        }
    }

    public void ShowPanel(string panel)
    {
        HidePanels();

        switch(panel)
        {
            case "Main":
                MainPanel.GetComponent<RectTransform>().localPosition = new Vector3(onScreenX, 0, 0);
                break;
            case "Difficulty":
                DifficultyPanel.GetComponent<RectTransform>().localPosition = new Vector3(onScreenX, 0, 0);
                break;
            case "Multiplayer":
                MultiplayerPanel.GetComponent<RectTransform>().localPosition = new Vector3(onScreenX, 0, 0);
                break;
            case "Character":
                BackgroundPanel.SetActive(true);
                mainCamera.transform.position = mainCameraPosition;
                mainCamera.transform.rotation = mainCameraRotation;
                if (localGame)
                {
                    CharacterSelectScript.characterSelectScript.PageHeader.text = "Select Player 1 Character";
                }
                else
                {
                    CharacterSelectScript.characterSelectScript.PageHeader.text = "Select Character";
                }
                if (networkGame == true)
                {
                    CharacterSelectScript.currentPlayerColor = "PlayerColor";
                }
                else if (localGame == true)
                {
                    CharacterSelectScript.currentPlayerColor = "Player1Color";
                }
                CharacterSelectPanel.SetActive(true);
                break;
            case "Level":
                BackgroundPanel.SetActive(false);
                cmc.cameraInit();
                LevelSelectPanel.SetActive(true);
                break;
        }
    }

    public void HidePanels(string panel = "none")
    {
        if (panel == "none")
        {
            MainPanel.GetComponent<RectTransform>().localPosition = new Vector3(offScreenX, 0, 0);
            DifficultyPanel.GetComponent<RectTransform>().localPosition = new Vector3(offScreenX, 0, 0);
            MultiplayerPanel.GetComponent<RectTransform>().localPosition = new Vector3(offScreenX, 0, 0);
            CharacterSelectPanel.SetActive(false);
            LevelSelectPanel.SetActive(false);
            turnSelectPanel.SetActive(false);
        }
        else if (panel == "bio")
        {
            CharacterSelectScript.isCharacterSelected = false;
            CharacterSelectScript.characterSelectScript.ResetMages(false);
        }
        CharacterSelectScript.isCharacterSelected = false;
        CharacterSelectScript.characterSelectScript.ResetMages(false);
    }

    public void startTutorial()
    {
        LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        StartCoroutine(LoadAsync(10));
    }

    public void displayStory()
    {
        if (PlayerPrefs.HasKey("StoryStage"))
        {
            storyContinuePanel.SetActive(true);
            storyContinuePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else
        {
            startNewStory();
        }
    }

    public void continueStory()
    {
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
                StartCoroutine(LoadAsync(11));
                break;
            case 3:
               // PlayerPrefs.SetInt("StoryStage", 4);
                StartCoroutine(LoadAsync(12));
                break;
            case 4:
                //PlayerPrefs.SetInt("StoryStage", 5);
                StartCoroutine(LoadAsync(13));
                break;
            case 5:
               // PlayerPrefs.SetInt("StoryStage", 6);
                StartCoroutine(LoadAsync(14));
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

    public void backButtonSelected(string currentPanel)
    {
        if (currentPanel == "Character")
        {
            if (CharacterSelectScript.currentPlayerColor == "Player1Color" || CharacterSelectScript.currentPlayerColor == "PlayerColor")
            {
                if (networkGame == true || localGame == true)
                {
                    ShowPanel("Multiplayer");
                }
                else if (quickGame == true)
                {
                    ShowPanel("Difficulty");
                }
            }
            else
            {
                string player1Color = PlayerPrefs.GetString("Player1Color");
                Button player1Button = GameObject.Find(player1Color + "MageButton").GetComponent<Button>();
                player1Button.interactable = true;
                CharacterSelectScript.currentPlayerColor = "Player1Color";
                CharacterSelectScript.characterSelectScript.PageHeader.text = "Select Player 1 Character";
            }
        }
        else if(currentPanel == "Level")
        {
            ShowPanel("Character");
            if (localGame == true)
            {
                string player1Color = PlayerPrefs.GetString("Player1Color");
                Button player1Button = GameObject.Find(player1Color + "MageButton").GetComponent<Button>();
                player1Button.interactable = false;
                CharacterSelectScript.currentPlayerColor = "Player2Color";
                CharacterSelectScript.characterSelectScript.PageHeader.text = "Select Player 2 Character";
            }
            else
            {
                ShowPanel("Character");
            }
        }
    }

    public void setDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
        HidePanels();
        displayTurnSelect();
    }

    public void displayTurnSelect()
    {
        turnSelectPanel.SetActive(true);
        turnSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
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
        ShowPanel("Character");
    }

    public void localGameSelected()
    {
        localGame = true;
        networkGame = false;
        quickGame = false;
        story = false;
        PlayerPrefs.SetString("GameType", "Local");
        ShowPanel("Character");
    }

    public void quickGameSelected()
    {
        quickGame = true;
        networkGame = false;
        localGame = false;
        story = false;
        PlayerPrefs.SetString("GameType", "AI");
        //display("difficulty");
        ShowPanel("Difficulty");
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
        if (PlayerPrefs.HasKey("StoryStage") && PlayerPrefs.GetInt("StoryStage") != 0)
        {
            storyContinuePanel.SetActive(true);
            storyContinuePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        }
        else
        {
            startNewStory();
        }
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
                CharacterSelectScript.characterSelectScript.PageHeader.text = "Select Player 2 Character";
                HidePanels("bio");
                //hid
            }
            else if(CharacterSelectScript.currentPlayerColor == "Player2Color")
            {
                ShowPanel("Level");
            }
        }
        else if(quickGame == true)
        {
            ShowPanel("Level");
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
        ShowPanel("Character");
    }
    public void hideTurnSelect()
    {
        turnSelectPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        turnSelectPanel.SetActive(false);
    }
}
