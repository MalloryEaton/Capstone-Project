using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBoardUIController : MonoBehaviour {

    public GameObject exitButton;
    NetworkingController networking;

    private GameObject LoadingPanel;
    public GameObject MenuPanel;
    public InputField chatInput;

    public ChatScrollList csl;
    public GameObject scrollView;
    public GameLogicController glc;
    public Button chatButton;
    public GameObject chatComponent;
    public NetworkingController networkController;
    public Button chatScrollButton;

    public Sprite up;
    public Sprite down;

    public GameObject winMessage;
    public Text winMessageText;
    public GameObject phasePanel;
    public Text phaseText;
    public GameObject forfeitConfirmationPanel;
    public GameObject drawPanel;
    public GameObject forfeitPanel;
    public GameObject losePanel;

    void Start()
    {
        LoadingPanel = GameObject.Find("LoadingPanel");
        networking = FindObjectOfType(typeof(NetworkingController)) as NetworkingController;
        winMessage.SetActive(false);
        if(PlayerPrefs.GetString("GameType") == "Network")
        {
            chatComponent.SetActive(true);
        }
        else
        {
            chatComponent.SetActive(false);
        }

        chatScrollButton.enabled = false;
        phasePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        phasePanel.SetActive(false);
        forfeitConfirmationPanel.SetActive(false);
        drawPanel.SetActive(false);
        forfeitPanel.SetActive(false);
        losePanel.SetActive(false);
    }
    
    public void exitToMenu()
    {
        if(MenuPanel != null)
            hideMenu();
        if(forfeitConfirmationPanel != null)
            hideForfeitConfirmation();
        if (MenuPanel != null)
            hideMenu();
        if (PlayerPrefs.GetString("GameType") == "Network")
        {
            networking.SendForfeit();
            networking.LeaveRoom();
        }
        else
        {
            LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            LeanTween.cancelAll();
            StartCoroutine(LoadAsync(1));
        }
    }

    public void exitToMenuTutorial()
    {
        
            LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            LeanTween.cancelAll();
            StartCoroutine(LoadAsync(1));
        
    }

    private IEnumerator LoadAsync(int levelNum)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelNum);
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void displayMenu()
    {
        if(MenuPanel.GetComponent<Animator>().GetBool("isDisplayed") == false)
        {
            MenuPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            glc.menuIsOpen = true;
            chatInput.enabled = false;
            csl.enabled = false;
            chatButton.enabled = false;
        }
        else
        {
            hideMenu();
        }
    }

    public void hideMenu()
    {
        MenuPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        glc.menuIsOpen = false;
        chatInput.enabled = true;
        csl.enabled = true;
        chatButton.enabled = true;
    }

    public void displayLose()
    {
        losePanel.SetActive(true);
        losePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
    }

    public void hideLose()
    {
        losePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        losePanel.SetActive(false);
    }

    public void sendChat()
    {
        networkController.SendChat();
        chatInput.text = "";
    }

    public void submit()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sendChat();
        }
    }

    public void addMessage(string name, string message)
    {
        csl.updateChat(name, message);
        //chatInput.text = "";
    }

    public void displayWinMessage(string color)
    {
        winMessage.SetActive(true);
        winMessageText.text = color + " wins!"; 
        winMessage.GetComponent<Animator>().SetBool("isDisplayed", true);
    }
    public void hideWinMessage()
    {
        winMessage.GetComponent<Animator>().SetBool("isDisplayed", false);
        winMessage.SetActive(false);

        //show loading panel and load scene
        if (PlayerPrefs.GetString("GameType") == "Story")
        {
            LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            switch (PlayerPrefs.GetInt("StoryStage"))
            {
                case 0:
                    PlayerPrefs.SetInt("StoryStage", 1);
                    StartCoroutine(LoadAsync(11));
                    break;
                case 1:
                    PlayerPrefs.SetInt("StoryStage", 2);
                    StartCoroutine(LoadAsync(12));
                    break;
                case 2:
                    PlayerPrefs.SetInt("StoryStage", 3);
                    StartCoroutine(LoadAsync(13));
                    break;
                case 3:
                    PlayerPrefs.SetInt("StoryStage", 4);
                    StartCoroutine(LoadAsync(14));
                    break;
                case 4:
                    PlayerPrefs.SetInt("StoryStage", 5);
                    StartCoroutine(LoadAsync(15));
                    break;
                case 5:
                    PlayerPrefs.SetInt("StoryStage", 6);
                    StartCoroutine(LoadAsync(16));
                    break;
            }
        }
        else if (PlayerPrefs.GetString("GameType") == "Network")
        {
            LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            networkController.LeaveRoom();
        }
        else if (PlayerPrefs.GetString("GameType") != "Story")
        {
            LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            StartCoroutine(LoadAsync(1));
        }
    }

    public void reloadStage()
    {
        Debug.Log("RELOAD " + PlayerPrefs.GetInt("StoryStage"));
        StartCoroutine(LoadAsync((PlayerPrefs.GetInt("StoryStage")+11)));
    }

    public IEnumerator displayPhase(string phase)
    {
        phasePanel.SetActive(true);
        phaseText.text = phase;
        phasePanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        Debug.Log("Phase before");
        yield return new WaitForSeconds(2f);
        //waitForAnimation();
        phasePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        phasePanel.SetActive(false);
        Debug.Log("Phase after");
       // phasePanel.GetComponent<Animator>().SetBool("isDisplayed", false);
    }

    IEnumerator waitForAnimation()
    {
        yield return new WaitForSeconds(2f);
    }


    public void displayChatInput()
    {
        if(chatInput.GetComponent<Animator>().GetBool("isDisplayed") == false)
        {
            chatInput.GetComponent<Animator>().SetBool("isDisplayed", true);
            chatScrollButton.enabled = true;
            displayChatScroll();
            hideNotification();
        }
        else
        {
            chatInput.GetComponent<Animator>().SetBool("isDisplayed", false);
            scrollView.GetComponent<Animator>().SetBool("isDisplayed", false);
            chatScrollButton.enabled = false;
        }
    }

    public void displayChatScroll()
    {
        if(scrollView.GetComponent<Animator>().GetBool("isDisplayed") == false)
        {
            scrollView.GetComponent<Animator>().SetBool("isDisplayed", true);
            chatScrollButton.image.sprite = down;
        }
        else
        {
            scrollView.GetComponent<Animator>().SetBool("isDisplayed", false);
            chatScrollButton.image.sprite = up;
        }
    }

    public void displayForfeitConfirmation()
    {
        forfeitConfirmationPanel.SetActive(true);
        hideMenu();
        forfeitConfirmationPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        glc.menuIsOpen = true;
        chatInput.enabled = false;
        csl.enabled = false;
        chatButton.enabled = false;
    }

    public void hideForfeitConfirmation()
    {
        forfeitConfirmationPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        forfeitConfirmationPanel.SetActive(false);
        displayMenu();
    }

    public void displayDraw()
    {
        drawPanel.SetActive(true);
        drawPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
        glc.menuIsOpen = true;
        chatInput.enabled = false;
        csl.enabled = false;
        chatButton.enabled = false;

    }

    public void hideDraw()
    {
        drawPanel.SetActive(false);
        drawPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        glc.menuIsOpen = false;
        chatInput.enabled = true;
        csl.enabled = true;
        chatButton.enabled = true;
    }

    public void displayNotification()
    {
        chatButton.GetComponent<Animator>().SetBool("isNotification", true);
    }

    public void hideNotification()
    {
        chatButton.GetComponent<Animator>().SetBool("isNotification", false);
    }

    public void displayForfeit()
    {
        forfeitPanel.SetActive(true);
        forfeitPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
    }

    public void hideForfeit()
    {
        forfeitPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
        forfeitPanel.SetActive(false);
    }
}
