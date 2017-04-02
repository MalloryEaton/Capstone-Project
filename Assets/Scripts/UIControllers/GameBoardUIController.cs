using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBoardUIController : MonoBehaviour {

    public GameObject exitButton;
    NetworkingController networking;

    private GameObject LoadingPanel;
    public GameObject MenuPanel;

    void Start()
    {
        LoadingPanel = GameObject.Find("LoadingPanel");
        networking = FindObjectOfType(typeof(NetworkingController)) as NetworkingController;
    }

    public void exitToMenu()
    {
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
        }
        else
        {
            hideMenu();
        }
    }

    public void hideMenu()
    {
        MenuPanel.GetComponent<Animator>().SetBool("isDisplayed", false);
    }
}
