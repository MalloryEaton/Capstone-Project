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
    public GameLogicController glc;
    public Button chatButton;
    public GameObject chatComponent;
    public NetworkingController networkController;


    void Start()
    {
        LoadingPanel = GameObject.Find("LoadingPanel");
        networking = FindObjectOfType(typeof(NetworkingController)) as NetworkingController;
        if(PlayerPrefs.GetString("GameType") == "Network")
        {
            chatComponent.SetActive(true);
        }
        else
        {
            chatComponent.SetActive(false);
        }
    }

    public void exitToMenu()
    {
        hideMenu();
        if (PlayerPrefs.GetString("GameType") == "Network")
        {
            networking.LeaveRoom();
        }
        else
        {
            LoadingPanel.GetComponent<Animator>().SetBool("isDisplayed", true);
            LeanTween.cancelAll();
            StartCoroutine(LoadAsync(1));
        }
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
            glc.waitingOnAnimation = true;
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
        glc.waitingOnAnimation = false;
        chatInput.enabled = true;
        csl.enabled = true;
        chatButton.enabled = true;
    }

    public void sendChat()
    {
        networkController.SendChat();
    }

    public void addMessage(string name, string message)
    {
        csl.updateChat(name, message);
        //chatInput.text = "";
    }
}
